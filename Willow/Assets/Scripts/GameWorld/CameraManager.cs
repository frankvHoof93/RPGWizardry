using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.UI.CameraEffects;
using nl.SWEG.Willow.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld
{
    /// <summary>
    /// Manages Camera within the GameWorld
    /// </summary>
    [RequireComponent(typeof(Camera), typeof(AudioListener), typeof(ScreenFade))]
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        #region Variables
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
        /// Whether the Camera is currently Fading In or Out
        /// </summary>
        public bool Fading { get; private set; }
        #endregion

        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Duration for Fading In or Out
        /// </summary>
        [SerializeField]
        [Range(0.00f, 2f)]
        [Tooltip("Duration for Fading In or Out")]
        private float FadeTime;
        /// <summary>
        /// Time-amount for Smoothing Movement
        /// </summary>
        [SerializeField]
        [Range(0f, 1f)]
        private float smoothTime = .3f;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Script used for Fading
        /// </summary>
        private ScreenFade screenFader;
        /// <summary>
        /// Movement-Velocity
        /// </summary>
        private Vector3 velocity = Vector3.zero;
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
        /// Fades the camera visibility between 2 values
        /// </summary>
        /// <param name="from">Starting Value for Fade</param>
        /// <param name="to">Ending Value for Fade</param>
        public void Fade(float from, float to)
        {
            Fading = true;
            screenFader.enabled = true;
            LeanTween.value(gameObject, UpdateShader, from, to, FadeTime);
        }

        /// <summary>
        /// Fades the camera visibility between 2 values for a specific duration
        /// </summary>
        /// <param name="from">Starting Value for Fade</param>
        /// <param name="to">Ending Value for Fade</param>
        public void Fade(float from, float to, float fadeTime)
        {
            Fading = true;
            screenFader.enabled = true;
            LeanTween.value(gameObject, UpdateShader, from, to, fadeTime);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Grabs References to required Components
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Camera = GetComponent<Camera>();
            screenFader = GetComponent<ScreenFade>();
            AudioListener = GetComponent<AudioListener>();
        }

        /// <summary>
        /// Updates current position of Camera to follow Player
        /// </summary>
        private void Update()
        {
            if (!PlayerManager.Exists)
                return;
            // Round the position of the Player
            Transform playerTF = PlayerManager.Instance.transform;
            Vector3 playerPos = new Vector3(
                Mathf.Round(playerTF.position.x * 1000.0f) / 1000.0f,
                Mathf.Round(playerTF.position.y * 1000.0f) / 1000.0f,
                playerTF.position.z - 100.0f);
            // Convert the mouse position to WorldSpace
            Vector3 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
            // Place the Camera between the Player and Mouse
            Vector3 mouseDir = mousePos - playerPos;
            mouseDir.z = playerTF.position.z - 100.0f;
            if (mouseDir.magnitude < 1)
                mouseDir.Normalize();
            // Set Camera to final Position
            Vector3 cameraPos = playerPos + mouseDir.normalized * 0.5f * Mathf.Sqrt(mouseDir.magnitude);
            transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, smoothTime);
        }
        #endregion

        #region Private
        /// <summary>
        /// Updates the shader used for Fading
        /// </summary>
        /// <param name="value">A value between 0 and 1</param>
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
            else
                Fading = true;
        }
        #endregion
        #endregion
    }
}