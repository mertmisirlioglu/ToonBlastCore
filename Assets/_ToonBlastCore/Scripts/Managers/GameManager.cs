using System;
using _ToonBlastCore.Scripts.Managers;
using Helpers;

namespace _ToonBlastCore.Scripts.Managers
{
    public enum GameState
    {
        Playing,
        Stop,
        Ended
    }
    public class GameManager : Singleton<GameManager>
    {
        public static GameState gameState;
        private void Start()
        {
            EventManager.TriggerEvent("onGameStart", null);
        }


    }
}
