using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Managers
{
    public class MatchCheckerManager : MonoSingleton<MatchCheckerManager>
    {
        public event Action AllMatchesCompletedEvent;
        
        [SerializeField] private List<InnerPiece> registeredPieces = new();
        private Coroutine completionCheckCoroutine;
        private float completionDelay = 0.5f;

        #region Public API

        public void RegisterMatchCheck(InnerPiece piece)
        {
            if (registeredPieces.Contains(piece)) return;

            registeredPieces.Add(piece);

            if (completionCheckCoroutine != null)
                StopCoroutine(completionCheckCoroutine);
        }

        public void UnregisterMatchCheck(InnerPiece p)
        {
            if (!registeredPieces.Contains(p)) return;

            registeredPieces.Remove(p);

            if (completionCheckCoroutine != null)
                StopCoroutine(completionCheckCoroutine);

            completionCheckCoroutine = StartCoroutine(CheckForCompletionAfterDelay());
        }

        public void AddAllMatchesCompletedListener(Action listener)
        {
            AllMatchesCompletedEvent += listener;
        }

        #endregion

        #region Private
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CheckForCompletionAfterDelay()
        {
            yield return new WaitForSeconds(completionDelay);

            if (registeredPieces.Count == 0)
                AllMatchesCompletedEvent?.Invoke();
        }
        #endregion
    }
}