using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scyout.FlappyPigeon
{
    public class GameController : MonoBehaviour
    {
        public TextMeshProUGUI currentScoreTextGUI;

        public TextMeshProUGUI yourScoreTextGUI;
        public TextMeshProUGUI yourScoreCounterGUI;

        public TextMeshProUGUI highestScoreTextGUI;
        public TextMeshProUGUI highestScoreCounterGUI;

        public Image gameIcon;
        public Image gestureIcon;

        public TextMeshProUGUI instructionTextGUI;

        public GameObject startButton;
        public GameObject glassPanel;

        private int score = 0;

        private void Start()
        {
            highestScoreCounterGUI.text = PlayerPrefs.GetInt("FP_HighestScore", 0).ToString();
        }

        private void OnEnable()
        {
            EventManager.onIncrementScore += IncrementScore;
            EventManager.onGameStart += StartGameObjectStates;
            EventManager.onGameEnd += RestartGameObjectStates;
        }

        private void OnDisable()
        {
            EventManager.onIncrementScore -= IncrementScore;
            EventManager.onGameStart -= StartGameObjectStates;
            EventManager.onGameEnd -= RestartGameObjectStates;
        }

        void IncrementScore()
        {
            score++;
            currentScoreTextGUI.text = "Score: " + score.ToString();
        }

        void RestartGameObjectStates()
        {
            glassPanel.SetActive(true);

            currentScoreTextGUI.gameObject.SetActive(false);

            yourScoreTextGUI.gameObject.SetActive(true);
            yourScoreCounterGUI.gameObject.SetActive(true);
            yourScoreCounterGUI.text = score.ToString();

            if (PlayerPrefs.GetInt("FP_HighestScore", 0) < score)
            {
                PlayerPrefs.SetInt("FP_HighestScore", score);
            }

            highestScoreTextGUI.gameObject.SetActive(true);
            highestScoreCounterGUI.gameObject.SetActive(true);
            highestScoreCounterGUI.text = PlayerPrefs.GetInt("FP_HighestScore", 0).ToString();

            gameIcon.gameObject.SetActive(true);
            gestureIcon.gameObject.SetActive(true);

            instructionTextGUI.gameObject.SetActive(true);

            startButton.SetActive(true);
            startButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart";
        }

        void StartGameObjectStates()
        {
            currentScoreTextGUI.gameObject.SetActive(true);

            yourScoreTextGUI.gameObject.SetActive(false);
            yourScoreCounterGUI.gameObject.SetActive(false);

            highestScoreTextGUI.gameObject.SetActive(false);
            highestScoreCounterGUI.gameObject.SetActive(false);

            gameIcon.gameObject.SetActive(false);
            gestureIcon.gameObject.SetActive(false);

            instructionTextGUI.gameObject.SetActive(false);

            startButton.SetActive(false);
            glassPanel.SetActive(false);
            score = 0;
            currentScoreTextGUI.text = "Score: " + score.ToString();
        }
    }

}