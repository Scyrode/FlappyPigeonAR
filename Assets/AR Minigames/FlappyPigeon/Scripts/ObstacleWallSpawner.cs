using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scyout.FlappyPigeon
{
    public class ObstacleWallSpawner : MonoBehaviour
    {
        public GameObject obstacle;
        public GameObject wallBoundary;

        public float delay;

        private List<List<GameObject>> obstacleWallList = new List<List<GameObject>>();
        private List<GameObject> wallBoundaryList = new List<GameObject>();

        private float timer = -1;
        private bool startObstacleSpawning = false;

        public delegate void InstantiateWallAction(GameObject obj);
        public static event InstantiateWallAction onInstantiateWall;

        private void OnEnable()
        {
            EventManager.onGameStart += ObstacleWallSpawnerEnabler;
            EventManager.onGameEnd += ObstacleWallSpawnerDisabler;
        }

        private void OnDisable()
        {
            EventManager.onGameStart -= ObstacleWallSpawnerEnabler;
            EventManager.onGameEnd -= ObstacleWallSpawnerDisabler;
        }

        // Update is called once per frame
        void Update()
        {
            if (startObstacleSpawning)
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    timer = delay;

                    if (obstacleWallList.Count < 3)
                    {
                        SpawnWall();
                    }
                    else
                    {
                        ResetFirstWall();
                    }
                }
            }
        }

        void ObstacleWallSpawnerEnabler()
        {
            foreach (var obstacleWall in obstacleWallList)
            {
                foreach (var obstacle in obstacleWall)
                {
                    Destroy(obstacle);
                }
                obstacleWall.Clear();
            }
            obstacleWallList.Clear();

            foreach (var wallBoundary in wallBoundaryList)
            {
                Destroy(wallBoundary);
            }
            wallBoundaryList.Clear();

            startObstacleSpawning = true;
        }

        void ObstacleWallSpawnerDisabler()
        {
            startObstacleSpawning = false;
        }

        void SpawnWall()
        {
            List<GameObject> obstacleWallSpawned = new List<GameObject>();

            // instatiate a wall boundary
            var wallBoundarySpawned = Instantiate(wallBoundary, gameObject.transform);

            // instantiate the obstacles
            for (int i = 0; i < 9; i++)
            {
                var obstacleClone = Instantiate(obstacle, wallBoundarySpawned.transform);
                obstacleWallSpawned.Add(obstacleClone);
            }

            SetWallPos(obstacleWallSpawned, wallBoundarySpawned);

            wallBoundaryList.Add(wallBoundarySpawned);
            obstacleWallList.Add(obstacleWallSpawned);

            onInstantiateWall?.Invoke(wallBoundarySpawned);
        }

        void ResetFirstWall()
        {
            List<GameObject> obstacleWallSpawned = obstacleWallList[0];
            GameObject wallBoundarySpawned = wallBoundaryList[0];

            obstacleWallList.RemoveAt(0);
            wallBoundaryList.RemoveAt(0);

            SetWallPos(obstacleWallSpawned, wallBoundarySpawned);

            obstacleWallList.Add(obstacleWallSpawned);
            wallBoundaryList.Add(wallBoundarySpawned);
        }

        void SetWallPos(List<GameObject> obstacleWallSpawned, GameObject wallBoundarySpawned)
        {
            // set the transform of the wallBoundary
            wallBoundarySpawned.transform.localPosition = Vector3.zero;

            // set the transform of the obstacles
            // given that the place for the first and last obstacle
            // must be occupied by an obstacle
            int random = Random.Range(2, 7);

            List<int> noObstacleIndexArray = new List<int>();

            for (int i = 0; i < 4; i++)
            {
                noObstacleIndexArray.Add(random);
                random++;
            }

            int counter = 0;
            int obstacleIndex = 0;

            do
            {
                if (!noObstacleIndexArray.Contains(counter))
                {
                    obstacleWallSpawned[obstacleIndex].transform.localPosition = new Vector3(0.0f, counter * 0.5f, 0.0f);

                    obstacleIndex++;
                }

                counter++;
            } while (counter < 13);
        }
    }
}
