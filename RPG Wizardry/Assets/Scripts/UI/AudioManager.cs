using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace nl.SWEG.RPGWizardry.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        //TO DO: play music
        [SerializeField]
        private AudioSource musicSource;

        private void Start()
        {
            musicSource.Play();
        }

        /// <summary>
        /// Creates an audio source, plays a clip at a slightly randomized pitch,
        /// Then starts a coroutine which deletes the audiosource after the clip
        /// </summary>
        public void PlaySound(AudioClip clip)
        {
            AudioSource effectSource = gameObject.AddComponent<AudioSource>();
            effectSource.volume = 0.5f;
            float rnd = Random.Range(-0.1f, 0.1f);
            effectSource.pitch += rnd;

            effectSource.PlayOneShot(clip);
            Destroy(effectSource, clip.length);
        }
    }
}

