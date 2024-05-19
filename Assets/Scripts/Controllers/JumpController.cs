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
        [SerializeField] private TimerBar timerBar;
        [SerializeField] private OverlayText overlayText;
        [SerializeField] private FeedbackOverlay feedback;

        [Header("Input")]
        [SerializeField] private int qteLength = 3;
        [SerializeField] private QTEAction[] qteActions;

        [Header("Timer")]
        [SerializeField] private float timerDuration = 3;

        private readonly Queue<Action> pendingActions = new();
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
            
            timerBar.Initialise(timerDuration);
            
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

        private void FixedUpdate()
        {
            if (qteListener.Enabled)
                timerBar.UpdateTimer(timer.TimeRemaining);

            while (pendingActions.Count > 0)
            {
                var action = pendingActions.Dequeue();
                action.Invoke();
            }
        }

        private void OnQteProgress()
        {
            pendingActions.Enqueue(() => feedback.DoPositiveFeedback());
            pendingActions.Enqueue(() => view.UpdateActiveLabel());
        }

        private void OnQteSucceeded()
        {
            pendingActions.Enqueue(() => feedback.DoPositiveFeedback());
            pendingActions.Enqueue(() => view.UpdateActiveLabel());
            pendingActions.Enqueue(() => StartCoroutine(DoOutro(true, timer.TimeRemaining)));
        }

        private void OnQteFailed()
        {
            pendingActions.Enqueue(() => feedback.DoNegativeFeedback());
            pendingActions.Enqueue(() => StartCoroutine(DoOutro(false)));
        }

        private void OnTimerElapsed()
        {
            pendingActions.Enqueue(() => feedback.DoNegativeFeedback());
            pendingActions.Enqueue(() => StartCoroutine(DoOutro(false)));
        }

        private IEnumerator DoOutro(bool success, float timeRemaining = 0f)
        {
            QTEFinished();
            
            characterAnimator.SetTrigger(success ? SuccessTrigger : FailureTrigger);

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
            
            timerBar.Hide();
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