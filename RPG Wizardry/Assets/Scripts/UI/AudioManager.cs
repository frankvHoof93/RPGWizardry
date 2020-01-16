using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace nl.SWEG.RPGWizardry.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        /// <summary>
        /// Static event that can be called from any class to play an audio clip
        /// </summary>
        /// <param name="clip"></param>
        public delegate void PlaySFX(AudioClip clip);
        public static PlaySFX playSFX;

        //TO DO: play music
        private AudioSource musicSource;

        /// <summary>
        /// Links event to function
        /// </summary>
        void Start()
        {
            playSFX += PlaySound;
        }

        /// <summary>
        /// Creates an audio source, plays a clip at a slightly randomized pitch,
        /// Then starts a coroutine which deletes the audiosource after the clip
        /// </summary>
        private void PlaySound(AudioClip clip)
        {
            AudioSource effectSource = gameObject.AddComponent<AudioSource>();
            effectSource.volume = 0.5f;
            float rnd = Random.Range(-0.1f, 0.1f);
            effectSource.pitch += rnd;

            effectSource.PlayOneShot(clip);
            StartCoroutine(RemoveSourceAfterClip(effectSource, clip));
        }

        /// <summary>
        /// Removes an audiosource after a given clip's length
        /// </summary>
        private IEnumerator RemoveSourceAfterClip(AudioSource aSource,AudioClip clip)
        {
            yield return new WaitForSeconds(clip.length);
            Destroy(aSource);
        }
    }

}

