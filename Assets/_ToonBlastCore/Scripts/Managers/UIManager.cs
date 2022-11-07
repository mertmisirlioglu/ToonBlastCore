using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace _ToonBlastCore.Scripts.Managers
{
    public class UIManager : Helpers.Singleton<UIManager>
    {
        [Header("Canvases and Screens")]
        [SerializeField] private GameObject GameUICanvas;
        [SerializeField] private GameObject HomeUICanvas;
        [SerializeField] private GameObject GameContainer;
        [SerializeField] private GameObject WinScreen;
        [SerializeField] private GameObject LosePopup;


        [Header("First Goal")]
        public Image FirstGoalImage;
        [SerializeField] private TextMeshProUGUI FirstGoalValueText;
        [SerializeField] private GameObject FirstGoalTick;

        [Header("Second Goal")]
         public Image SecondGoalImage;
        [SerializeField] private TextMeshProUGUI SecondGoalValueText;
        [SerializeField] private GameObject SecondGoalTick;


        [Header("Move Count")]
        [SerializeField] private TextMeshProUGUI moveText;


        private void Start()
        {
            EventManager.StartListening("UpdateUI", UpdateUI);
            EventManager.StartListening("onGoalDone", OnGoalDone);
            EventManager.StartListening("onLose", OnLose);
            EventManager.StartListening("onWin", OnWin);
        }

        public void StartGame()
        {
            HomeUICanvas.SetActive(false);
            GameUICanvas.SetActive(true);
            GameContainer.SetActive(true);
            EventManager.TriggerEvent("onGameStart", null);
        }

        public void Restart()
        {
            LosePopup.SetActive(false);
            GameContainer.SetActive(true);
            EventManager.TriggerEvent("onGameRestart", null);
        }



        private void OnLose(Dictionary<string, object> message)
        {
            GameContainer.SetActive(false);
            LosePopup.SetActive(true);
        }

        private void OnWin(Dictionary<string, object> message)
        {
            GameContainer.SetActive(false);
            GameUICanvas.SetActive(false);
            WinScreen.SetActive(true);
        }

        public void NextLevel()
        {
            EventManager.TriggerEvent("loadNextLevel", null);
        }

        private void OnGoalDone(Dictionary<string, object> message)
        {
            if ((bool)message["isFirstGoal"])
            {
                FirstGoalValueText.enabled = false;
                FirstGoalTick.SetActive(true);
            }
            else
            {
                SecondGoalValueText.enabled = false;
                SecondGoalTick.SetActive(true);
            }
        }

        private void UpdateUI(Dictionary<string, object> message)
        {
            FirstGoalValueText.SetText(LevelManager.Instance.firstGoalValue.ToString());
            SecondGoalValueText.SetText(LevelManager.Instance.secondGoalValue.ToString());
            moveText.SetText(LevelManager.Instance.remainingMoves.ToString());
        }

        public void SetGoalsAndMoveCount()
        {
            var level = LevelManager.Instance.levelList[LevelManager.Instance.currentLevel];

            FirstGoalImage.sprite = LevelManager.Instance.enumToTilesDictionary[level.firstGoalTile]
                .GetComponent<SpriteRenderer>().sprite;
            FirstGoalValueText.SetText(level.firstGoalValue.ToString());

            SecondGoalImage.sprite = LevelManager.Instance.enumToTilesDictionary[level.secondGoalTile]
                .GetComponent<SpriteRenderer>().sprite;
            SecondGoalValueText.SetText(level.secondGoalValue.ToString());

            moveText.SetText(level.totalMove.ToString());
        }
    }
}
