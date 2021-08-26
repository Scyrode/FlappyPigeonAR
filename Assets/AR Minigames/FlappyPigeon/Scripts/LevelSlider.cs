using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scyout.FlappyPigeon
{
    public class LevelSlider : MonoBehaviour
    {
        private List<GameObject> objectsToSlide = new List<GameObject>();
        private List<GameObject> persistentObjectsToSlide = new List<GameObject>();
        public float speed;

        private bool startSliding = false;

        private void OnEnable()
        {
            EventManager.onInstatiate += UpdateListOfObjects;
            EventManager.onInstantiatePersistent += AddPersistentObject;
            EventManager.onGameStart += LevelSliderEnabler;
            EventManager.onGameEnd += LevelSliderDisabler;
        }

        private void OnDisable()
        {
            EventManager.onInstatiate -= UpdateListOfObjects;
            EventManager.onInstantiatePersistent -= AddPersistentObject;
            EventManager.onGameStart -= LevelSliderEnabler;
            EventManager.onGameEnd -= LevelSliderDisabler;
        }

        void AddPersistentObject(GameObject obj)
        {
            // test
            if (persistentObjectsToSlide == null)
                Debug.Log("List is null");

            persistentObjectsToSlide.Add(obj);
            objectsToSlide.Add(obj);
        }

        void UpdateListOfObjects(GameObject obj)
        {
            objectsToSlide.Add(obj);
        }

        void LevelSliderEnabler()
        {
            objectsToSlide.Clear();
            foreach (var obj in persistentObjectsToSlide)
            {
                objectsToSlide.Add(obj);
            }
            startSliding = true;
        }

        void LevelSliderDisabler()
        {
            startSliding = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (startSliding)
            {
                if (objectsToSlide.Count != 0)
                {
                    foreach (var obj in objectsToSlide)
                    {
                        obj.transform.Translate(Vector3.back * Time.deltaTime * speed);
                    }
                }
            }
        }
    }

}