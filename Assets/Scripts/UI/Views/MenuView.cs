using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CliffJump.UI.Views
{
    public class MenuView : MonoBehaviour
    {
        public UnityEvent onStartGameRequested;

        public void StartGame()
        {
            // TODO: Disable UI

            StartCoroutine(DoStartGame());
        }

        private IEnumerator DoStartGame()
        {
            yield return null;
            
            onStartGameRequested?.Invoke();
        }
    }
}