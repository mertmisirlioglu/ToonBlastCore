using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _ToonBlastCore.Scripts.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            EventManager.StartListening("onGameEnd", OnGameEnd);
            EventManager.StartListening("onScoreChange", OnScoreChange);
        }

        private void OnGameStart(Dictionary<string, object> message)
        {

        }

        private void OnGameEnd(Dictionary<string, object> message)
        {

        }
        private void OnScoreChange(Dictionary<string, object> message)
        {

        }

        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
            EventManager.StopListening("onGameEnd", OnGameEnd);
            EventManager.StopListening("onScoreChange", OnScoreChange);
        }


    }
}
