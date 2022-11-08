using System.Collections.Generic;
using Helpers;
using UnityEngine;


namespace _ToonBlastCore.Scripts.Managers{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioSource audioSource2;
        public AudioClip hitAudio;
        public AudioClip duckAudio;
        public AudioClip balloonAudio;
        public AudioClip winAudio;
        public AudioClip loseAudio;


        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            EventManager.StartListening("onWin", OnWin);
            EventManager.StartListening("onLose", OnLose);
            EventManager.StartListening("onHit", OnHit);
            EventManager.StartListening("playDuckAudio", PlayDuckAudio);
            EventManager.StartListening("playBalloonAudio", PlayBalloonAudio);
        }

        private void OnGameStart(Dictionary<string, object> message)
        {

        }

        private void OnWin(Dictionary<string, object> message)
        {
            PlayOneShot(winAudio);
        }

        private void OnLose(Dictionary<string, object> message)
        {
            PlayOneShot(loseAudio);
        }

        private void OnHit(Dictionary<string, object> message)
        {
            PlayOneShot(hitAudio);
        }

        private void PlayDuckAudio(Dictionary<string, object> message)
        {
            PlayOneShot(duckAudio);
        }

        private void PlayBalloonAudio(Dictionary<string, object> message)
        {
            PlayOneShot(balloonAudio);
        }


        public void PlayOneShot(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }

        private void OnDisable()
        {
            EventManager.StopListening("onGameStart", OnGameStart);
            EventManager.StopListening("onWin", OnWin);
            EventManager.StopListening("onLose", OnLose);
            EventManager.StopListening("onHit", OnHit);
            EventManager.StopListening("playDuckAudio", PlayDuckAudio);
            EventManager.StopListening("playBalloonAudio", PlayBalloonAudio);
        }

    }
}
