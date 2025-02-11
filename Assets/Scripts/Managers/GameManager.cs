using System;
using System.Collections.Generic;
using Signals;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Self Variable

        #region Private Variables

        private List<string> tags;

        #endregion
        
        #endregion
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            GameSignals.Instance.onGameLoseOrWin += OnGameLoseOrWin;
            GameSignals.Instance.onTeamTag += OnTeamTag;
            GameSignals.Instance.onGamePause += GamePause;
        }

        private void UnsubscribeEvents()
        {
            GameSignals.Instance.onGameLoseOrWin -= OnGameLoseOrWin;
            GameSignals.Instance.onTeamTag -= OnTeamTag;
            GameSignals.Instance.onGamePause -= GamePause;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void OnTeamTag(string teamtag)
        {
            tags.Add(teamtag);
        }

        private void OnGameLoseOrWin(string teamtag)
        {
            if (teamtag == "Team1")
            {
                Debug.Log("oyun kaybedildi");
            }
            else if (tags.Count <= 1)
            {
                Debug.Log("oyunu kazandın.");
            }
            GamePause(true);
        }

        private void GamePause(bool pause)
        {
            if (pause)
                Time.timeScale = 0f; // Oyunu duraklat
            else
                Time.timeScale = 1f; // Oyun Devam Edecek
        }
    }
}