using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scyout.FlappyPigeon
{
    public class EventManager : MonoBehaviour
    {
        public delegate void InstatiateAction(GameObject obj);
        public static event InstatiateAction onInstatiate;

        public delegate void InstatiatePersistentAction(GameObject obj);
        public static event InstatiatePersistentAction onInstantiatePersistent;

        public delegate void GameStartAction();
        public static event GameStartAction onGameStart;

        public delegate void GameEndAction();
        public static event GameEndAction onGameEnd;

        public delegate void IncrementScoreAction();
        public static event IncrementScoreAction onIncrementScore;

        private void OnEnable()
        {
            StartButtonBehaviour.onStartButtonPress += CallOnGameStart;
            ObstacleWallSpawner.onInstantiateWall += CallOnInstatiate;
            GroundSpawner.onInstatiateGround += CallOnInstatiatePersistent;
            BirdBehaviour.onObstacleWallPassed += CallIncrementScore;
            BirdBehaviour.onBirdDeath += CallOnGameEnd;
        }

        private void OnDisable()
        {
            StartButtonBehaviour.onStartButtonPress -= CallOnGameStart;
            ObstacleWallSpawner.onInstantiateWall -= CallOnInstatiate;
            GroundSpawner.onInstatiateGround -= CallOnInstatiatePersistent;
            BirdBehaviour.onObstacleWallPassed -= CallIncrementScore;
            BirdBehaviour.onBirdDeath -= CallOnGameEnd;
        }

        void CallIncrementScore()
        {
            onIncrementScore?.Invoke();
        }

        void CallOnInstatiate(GameObject obj)
        {
            onInstatiate?.Invoke(obj);
        }

        void CallOnInstatiatePersistent(GameObject obj)
        {
            onInstantiatePersistent?.Invoke(obj);
        }

        void CallOnGameStart()
        {
            onGameStart?.Invoke();
        }

        void CallOnGameEnd()
        {
            onGameEnd?.Invoke();
        }
    }

}