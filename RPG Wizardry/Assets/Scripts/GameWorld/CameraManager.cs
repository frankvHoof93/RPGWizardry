using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;
using System.Collections;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Camera), typeof(AudioListener), typeof(ScreenFade))]
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        #region Fields
        #region Public
        /// <summary>
        /// Camera-Component for Camera
        /// </summary>
        public Camera Camera { get; private set; }
        /// <summary>
        /// Listener for Audio in Scene
        /// </summary>
        public AudioListener AudioListener { get; private set; }

        /// <summary>
        /// Is true when the camera fading in or out.
        /// </summary>
        public bool Fading { get; private set; }
        #endregion

        #region Editor
        /// <summary>
        /// How long it takes to fade in or out.
        /// </summary>
        [SerializeField]
        [Range(0.00f, 2f)]
        private float FadeTime;
        #endregion

        #region Private
        /// <summary>
        /// Camera-Velocity (when moving)
        /// </summary>
        private Vector3 velocity = Vector3.zero;
        /// <summary>
        /// Script used for Fading
        /// </summary>
        private ScreenFade screenFader;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Toggles AudioListener on Camera
        /// </summary>
        public void ToggleAudio()
        {
            AudioListener.enabled = !AudioListener.enabled;
        }        

        /// <summary>
        /// Fade the camera visibility between 2 values.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void Fade(float from, float to)
        {
            Fading = true;
            screenFader.enabled = true;
            LeanTween.value(gameObject, UpdateShader, from, to, FadeTime);
        }

        /// <summary>
        /// Overload of Fade that takes a custom fade time argument
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public void Fade(float from, float to, float fadeTime)
        {
            Fading = true;
            screenFader.enabled = true;
            LeanTween.value(gameObject, UpdateShader, from, to, fadeTime);
        }
        #endregion

        #region Private
        /// <summary>
        /// Updates the shader used for room transitioning.
        /// </summary>
        /// <param name="value">A value between 0 and 1.</param>
        private void UpdateShader(float value)
        {
            screenFader.SetValue(value);

            if (value == 0)
            {
                Fading = false;
            }
            else if (value == 1)
            {
                Fading = false;
                screenFader.enabled = false;
            }
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs Reference to Camera, and instantiate the screenfade shader.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Camera = GetComponent<Camera>();
            screenFader = GetComponent<ScreenFade>();
            AudioListener = GetComponent<AudioListener>();
        }
        #endregion
        #endregion
    }
}