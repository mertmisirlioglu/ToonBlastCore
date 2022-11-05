using System;
using _ToonBlastCore.Scripts.Managers;
using Helpers;

namespace _ToonBlastCore.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {

        private void Start()
        {
            EventManager.TriggerEvent("onGameStart", null);
        }


    }
}
