using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CliffJump.Data;
using CliffJump.Input;
using CliffJump.UI;
using CliffJump.UI.Views;
using CliffJump.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace CliffJump.Controllers
{
    public class JumpController : MonoBehaviour
    {
        public event Action<float> JumpSucceeded;
        public event Action JumpFailed;
        
        [Header("References")]
        [SerializeField] private JumpView view;
        [SerializeField] private TimerPlus timer;

        [Header("Intro")]
        [SerializeField] private float preIntroDuration = 1;
        [SerializeField] private float introDuration = 2;

        [Header("Outro")]
        [SerializeField] private float outroDuration = 1;
        [SerializeField] private Animator characterAnimator;
        private static readonly int SuccessTrigger = Animator.StringToHash("Success");
        private static readonly int FailureTrigger = Animator.StringToHash("Failure");

        [Header("UI")]
        [SerializeField] private OverlayText overlayText;
        [SerializeField] private FeedbackOverlay feedback;

        [Header("Input")]
        [SerializeField] private int qteLength = 3;
        [SerializeField] private QTEAction[] qteActions;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 3;

        private readonly QTEListener qteListener = new();

        private void OnEnable()
        {
            characterAnimator.ResetTrigger(SuccessTrigger);
            characterAnimator.ResetTrigger(FailureTrigger);
            
            timer.Elapsed += OnTimerElapsed;

            qteListener.Progress += OnQteProgress;
            qteListener.Succeeded += OnQteSucceeded;
            qteListener.Failed += OnQteFailed;

            StartCoroutine(DoIntro());
        }

        private IEnumerator DoIntro()
        {
            yield return new WaitForSeconds(preIntroDuration);
            
            overlayText.DisplayText("JUMP");

            yield return new WaitForSeconds(introDuration);

            StartQTE();
        }

        private void StartQTE()
        {
            var actions = qteActions
                .Select(action => action.ActionReference.ToInputAction())
                .ToArray();
            
            view.ClearView();

            var inputActions = GenerateQTEQueue();
            qteListener.Listen(actions, inputActions);
            
            timer.StartTimer(timerDuration);
        }

        private Queue<InputAction> GenerateQTEQueue()
        {
            var inputActions = new Queue<InputAction>();

            for (var i = 0; i < qteLength; i++)
            {
                var action = qteActions[Random.Range(0, qteActions.Length)];
                inputActions.Enqueue(action.ActionReference.ToInputAction());
                
                view.AddQTELabel(action);
            }

            return inputActions;
        }

        private void OnQteProgress()
        {
            feedback.DoPositiveFeedback();
            view.UpdateActiveLabel();
        }

        private void OnQteSucceeded()
        {
            feedback.DoPositiveFeedback();
            QTEFinished();

            characterAnimator.SetTrigger(SuccessTrigger);

            StartCoroutine(DoOutro(true, timer.TimeRemaining));
        }

        private void OnQteFailed()
        {
            feedback.DoNegativeFeedback();
            QTEFinished();

            characterAnimator.SetTrigger(FailureTrigger);

            StartCoroutine(DoOutro(false));
        }

        private void OnTimerElapsed()
        {
            feedback.DoNegativeFeedback();
            QTEFinished();

            characterAnimator.SetTrigger(FailureTrigger);

            StartCoroutine(DoOutro(false));
        }

        private IEnumerator DoOutro(bool success, float timeRemaining = 0f)
        {
            yield return new WaitForSeconds(outroDuration);

            if (success)
                JumpSucceeded?.Invoke(timeRemaining);
            else
                JumpFailed?.Invoke();
        }

        private void QTEFinished()
        {
            timer.Elapsed -= OnTimerElapsed;

            qteListener.Unlisten();
            
            view.ClearView();
        }

        private void OnDisable()
        {
            timer.Elapsed -= OnTimerElapsed;

            qteListener.Unlisten();

            qteListener.Progress -= OnQteProgress;
            qteListener.Succeeded -= OnQteSucceeded;
            qteListener.Failed -= OnQteFailed;
        }
    }
}