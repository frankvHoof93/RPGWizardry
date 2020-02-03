using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Utils.Behaviours;
using System.Collections;
using UnityEngine;

namespace nl.SWEG.Willow.UI.CameraEffects
{
    /// <summary>
    /// Shakes Camera
    /// </summary>
    public class ScreenShake : SingletonBehaviour<ScreenShake>
    {
        #region Variables
        /// <summary>
        /// Transform to shake
        /// </summary>
        private Transform camTransform;
        /// <summary>
        /// Shake-Routine currently operating
        /// </summary>
        private Coroutine currentShake;
        #endregion

        #region Methods
        /// <summary>
        /// Starts a screen shake
        /// </summary>
        /// <param name="duration">Duration of shake</param>
        /// <param name="intensity">Intensity of shake</param>
        public void Shake(float intensity, float duration)
        {
            if (currentShake != null)
                StopCoroutine(ShakeLoop(intensity, duration));
            StartCoroutine(ShakeLoop(intensity, duration));
        }

        /// <summary>
        /// Retrieves Camera-Transform
        /// </summary>
        protected void Start()
        {
            if (camTransform == null)
                camTransform = CameraManager.Instance.Camera.transform;
        }

        /// <summary>
        /// Shakes the screen
        /// </summary>
        private IEnumerator ShakeLoop(float intensity, float duration)
        {
            while (duration > 0)
            {
                camTransform.localPosition += Random.insideUnitSphere * intensity;
                duration -= Time.deltaTime;
                yield return null;
            }
        }
        #endregion
    }
}