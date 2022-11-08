using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using _ToonBlastCore.Scripts.Mechanic;
using Helpers;
using Level;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _ToonBlastCore.Scripts.Managers
{
    public class LevelManager : Singleton<LevelManager>
    {
        public LevelScriptableObject[] levelList;
        public LevelScriptableObject currentLevelObject;

        public int currentLevel;

        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            EventManager.StartListening("loadNextLevel", LoadNextLevel);
            EventManager.StartListening("onGameRestart", RestartLevel);

            currentLevel = PlayerPrefs.GetInt("level");
        }

        private void OnGameStart(Dictionary<string, object> message)
        {
            GetLevelData();
            EventManager.TriggerEvent("onLevelLoaded", null);
        }

        private void GetLevelData()
        {
            if (currentLevel >= levelList.Length)
            {
                currentLevel = Random.Range(0, levelList.Length - 1);
            }

            currentLevelObject = levelList[currentLevel];
        }

        private void LoadNextLevel(Dictionary<string, object> message)
        {
            currentLevel++;
            PlayerPrefs.SetInt("level",currentLevel);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void RestartLevel(Dictionary<string, object> message)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
            EventManager.StopListening("loadNextLevel", LoadNextLevel);
            EventManager.StopListening("onGameRestart", RestartLevel);
        }

    }
}
