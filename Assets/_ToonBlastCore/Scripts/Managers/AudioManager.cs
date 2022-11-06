using System.Collections.Generic;
using Helpers;
using UnityEngine;


namespace _ToonBlastCore.Scripts.Managers{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioSource audioSource;
        public AudioClip hitAudio;
        public AudioClip collectAudio;
        public AudioClip duckAudio;
        public AudioClip balloonAudio;

        private void Start()
        {
            EventManager.StartListening("onGameStart", OnGameStart);
            EventManager.StartListening("onWin", OnWin);
            EventManager.StartListening("onLose", OnLose);
            EventManager.StartListening("onHit", OnHit);
            EventManager.StartListening("onScoreChange", OnScoreChange);
        }

        private void OnGameStart(Dictionary<string, object> message)
        {

        }

        private void OnWin(Dictionary<string, object> message)
        {

        }

        private void OnLose(Dictionary<string, object> message)
        {

        }

        private void OnHit(Dictionary<string, object> message)
        {
            PlayOneShot(hitAudio);
        }

        private void OnScoreChange(Dictionary<string, object> message)
        {

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
            EventManager.StopListening("onScoreChange", OnScoreChange);
        }

    }
}
