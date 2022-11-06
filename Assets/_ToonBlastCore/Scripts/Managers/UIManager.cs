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
        [Header("First Goal")]
        public Image FirstGoalImage;
        [SerializeField] private TextMeshProUGUI FirstGoalValueText;

        [Header("Second Goal")]
         public Image SecondGoalImage;
        [SerializeField] private TextMeshProUGUI SecondGoalValueText;

        [Header("Move Count")]
        [SerializeField] private TextMeshProUGUI moveText;


        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            EventManager.StartListening("UpdateUI", UpdateUI);
            EventManager.StartListening("onLose", OnLose);
        }

        private void OnGameStart(Dictionary<string, object> message)
        {
            SetGoalsAndMoveCount();
        }

        private void OnTileClicked(Dictionary<string, object> message)
        {
        }

        private void OnLose(Dictionary<string, object> message)
        {
        }

        private void UpdateUI(Dictionary<string, object> message)
        {
            FirstGoalValueText.SetText(LevelManager.Instance.firstGoalValue.ToString());
            SecondGoalValueText.SetText(LevelManager.Instance.secondGoalValue.ToString());
            moveText.SetText(LevelManager.Instance.remainingMoves.ToString());
        }

        private void SetGoalsAndMoveCount()
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
