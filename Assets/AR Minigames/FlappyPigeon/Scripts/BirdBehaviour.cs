using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scyout.FlappyPigeon
{
    public class BirdBehaviour : MonoBehaviour
    {
        public ParticleSystem[] particleSystems;

        public float thrust = 7.5f;
        public float smooth = 5.0f;
        public float tiltAngle = 10.0f;
        public float increaseGravityFactor = 10.0f;
        public float delayAfterDeath = 3.0f;

        public delegate void BirdDeathAction();
        public static event BirdDeathAction onBirdDeath;

        public delegate void ObstacleWallPassedAction();
        public static event ObstacleWallPassedAction onObstacleWallPassed;

        Rigidbody rb;

        private bool isFlying;
        private bool isBirdAwake;
        private Vector3 initialPos;
        private AudioSource source;

        private void OnEnable()
        {
            EventManager.onGameStart += WakeUpBird;
            EventManager.onGameEnd += SleepBird;
        }

        private void OnDisable()
        {
            EventManager.onGameStart -= WakeUpBird;
            EventManager.onGameEnd -= SleepBird;
        }

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            source = GetComponent<AudioSource>();
            rb.isKinematic = true;
            initialPos = gameObject.transform.localPosition;
            SleepBird();
        }

        void Update()
        {
            if (isBirdAwake)
            {
                if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    isFlying = true;
                }

                Quaternion target = Quaternion.AngleAxis(-transform.InverseTransformDirection(rb.velocity).y * tiltAngle, transform.parent.InverseTransformDirection(transform.parent.transform.right));
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * smooth);

                if (isFlying)
                {
                    source.Play();
                }
            }
        }

        void FixedUpdate()
        {
            if (isBirdAwake)
            {
                if (isFlying)
                {
                    rb.velocity = transform.parent.up * thrust;
                    isFlying = false;
                }
            }

            rb.AddForce(-transform.parent.up * increaseGravityFactor);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Obstacle"))
            {
                if (isBirdAwake)
                {
                    // test
                    //Debug.Log("Bird died!");
                    onBirdDeath();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                if (isBirdAwake)
                {
                    onBirdDeath();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                if (isBirdAwake)
                {
                    onObstacleWallPassed();
                }
            }
        }

        void WakeUpBird()
        {
            isBirdAwake = true;
            rb.isKinematic = false;

            gameObject.transform.localPosition = initialPos;
            gameObject.transform.localRotation = Quaternion.identity;

            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play();
            }
        }

        void SleepBird()
        {
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Stop();
            }

            isBirdAwake = false;
        }
    }

}