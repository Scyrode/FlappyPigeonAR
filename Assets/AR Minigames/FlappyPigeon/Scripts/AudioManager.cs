using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scyout.FlappyPigeon
{
    public class AudioManager : MonoBehaviour
    {
        public Sound[] sounds;

        private void Awake()
        {
            foreach (Sound sound in sounds)
            {
                var soundGameObject = new GameObject(sound.name);
                soundGameObject.transform.parent = transform;
                sound.source = soundGameObject.AddComponent<AudioSource>();
                sound.source.name = sound.name;
                sound.source.clip = sound.clip;
                sound.source.loop = sound.loop;
                sound.source.volume = sound.volume;
            }
        }

        private void OnEnable()
        {
            EventManager.onGameStart += FlappyPigeonGameStart;
            EventManager.onGameEnd += FlappyPigeonGameEnd;
        }

        private void OnDisable()
        {
            EventManager.onGameStart -= FlappyPigeonGameStart;
            EventManager.onGameEnd -= FlappyPigeonGameEnd;
        }

        private void FlappyPigeonGameStart()
        {
            Sound gameStartSound = Array.Find(sounds, sound => sound.name == "GameStart");
            Sound gameOverSound = Array.Find(sounds, sound => sound.name == "GameOver");
            Sound themeSound = Array.Find(sounds, sound => sound.name == "Theme");

            if (gameStartSound != null)
                gameStartSound.source.Play();
            if (gameOverSound != null)
                gameOverSound.source.Stop();
            if (themeSound != null)
                themeSound.source.Play();
        }

        private void FlappyPigeonGameEnd()
        {
            Sound gameStartSound = Array.Find(sounds, sound => sound.name == "GameStart");
            Sound gameOverSound = Array.Find(sounds, sound => sound.name == "GameOver");
            Sound themeSound = Array.Find(sounds, sound => sound.name == "Theme");

            if (gameStartSound != null)
                gameStartSound.source.Stop();
            if (gameOverSound != null)
                gameOverSound.source.Play();
            if (themeSound != null)
                themeSound.source.Stop();
        }
    }

}