using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Managers
{
    public class MatchCheckerManager : MonoSingleton<MatchCheckerManager>
    {
        [SerializeField] private int activeMatchChecks;
        [SerializeField] private List<MatchData> matchDataList = new List<MatchData>();
        private Coroutine completionCheckCoroutine;
        private float completionDelay = 0.5f;

        public event Action AllMatchesCompletedEvent;

        #region Public API

        public void RegisterMatchCheck()
        {
            activeMatchChecks++;
        }

        public void UnregisterMatchCheck()
        {
            activeMatchChecks = Mathf.Max(0, activeMatchChecks - 1);

            if (completionCheckCoroutine != null)
                StopCoroutine(completionCheckCoroutine);

            completionCheckCoroutine = StartCoroutine(CheckForCompletionAfterDelay());
        }

        // public void RegisterMatchCheck(MatchData matchData)
        // {
        //     if (matchDataList.Contains(matchData)) return;
        //     
        //     matchDataList.Add(matchData);
        //     activeMatchChecks++;
        // }
        //
        // public void UnregisterMatchCheck(MatchData matchData)
        // {
        //     if(!matchDataList.Contains(matchData)) return;
        //     
        //     matchDataList.Remove(matchData);
        //     activeMatchChecks = Mathf.Max(0, activeMatchChecks - 1);
        //
        //     if (completionCheckCoroutine != null)
        //         StopCoroutine(completionCheckCoroutine);
        //
        //     completionCheckCoroutine = StartCoroutine(CheckForCompletionAfterDelay());
        // }

        public void AddAllMatchesCompletedListener(Action listener)
        {
            AllMatchesCompletedEvent += listener;
        }

        public void RemoveAllMatchesCompletedListener(Action listener)
        {
            AllMatchesCompletedEvent -= listener;
        }

        public bool IsMatchingInProgress()
        {
            return activeMatchChecks > 0;
        }

        #endregion

        #region Private

        private IEnumerator CheckForCompletionAfterDelay()
        {
            yield return new WaitForSeconds(completionDelay);

            if (matchDataList.Count == 0)
                AllMatchesCompletedEvent?.Invoke();
        }

        #endregion
    }
}

[Serializable]
public class MatchData
{
    public InnerPiece activePiece;
    public InnerPiece passivePiece;
}