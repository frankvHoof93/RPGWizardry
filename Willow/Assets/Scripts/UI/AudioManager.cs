using nl.SWEG.Willow.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.Willow.UI
{
    /// <summary>
    /// Plays Background-Music and Audio-Effects
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Audio-Source that plays background music
        /// </summary>
        [SerializeField]
        private AudioSource musicSource;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables

        /// <summary>
        /// Creates an audio source, plays a clip at a slightly randomized pitch,
        /// Then starts a coroutine which deletes the audio-source after the clip
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

        /// <summary>
        /// Starts music when entering a scene that contains this script
        /// </summary>
        private void Start()
        {
            musicSource.Play();
        }
    }
}