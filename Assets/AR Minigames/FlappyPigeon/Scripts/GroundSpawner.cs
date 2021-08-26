using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scyout.FlappyPigeon
{
    public class GroundSpawner : MonoBehaviour
    {
        public Transform ground;
        public float offset;

        public delegate void InstatiateGroundAction(GameObject obj);
        public static event InstatiateGroundAction onInstatiateGround;

        private List<GameObject> groundList = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            // add first ground block
            SpawnGround(Vector3.zero);
            // add second ground block
            SpawnGround(Vector3.forward * offset);
        }

        // Update is called once per frame
        void Update()
        {
            if (groundList[1].transform.localPosition.z <= 0)
            {
                ResetGroundPos();
            }
        }

        void SpawnGround(Vector3 pos)
        {
            var groundClone = Instantiate(ground, transform.position, transform.rotation, gameObject.transform).gameObject;
            groundClone.transform.localPosition = pos;
            onInstatiateGround?.Invoke(groundClone);
            groundList.Add(groundClone);
        }

        void ResetGroundPos()
        {
            var ground = groundList[0];

            groundList.RemoveAt(0);

            ground.transform.localPosition = Vector3.forward * offset;

            groundList.Add(ground);
        }
    }

}