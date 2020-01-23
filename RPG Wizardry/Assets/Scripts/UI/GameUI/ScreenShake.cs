using System.Collections;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    public class ScreenShake : SingletonBehaviour<ScreenShake>
    {
        /// <summary>
        /// Transform of the camera to shake.
        /// </summary>
        private Transform camTransform;

        /// <summary>
        /// How long the object should shake for.
        /// </summary>
        private float shakeDuration;

        /// <summary>
        /// Amplitude of the shake. A larger value shakes the camera harder.
        /// </summary>
        private float shakeAmount;

        /// <summary>
        /// gets the transform of the camera.
        /// </summary>
        protected void Start()
        {
            if (camTransform == null)
                camTransform = CameraManager.Instance.Camera.transform;
        }

        /// <summary>
        /// Starts a screen shake.
        /// </summary>
        /// <param name="duration">the shake duration</param>
        /// <param name="intensity">the shake intensity</param>
        public void Shake(float intensity, float duration)
        {
            //Set the variables
            shakeDuration = duration;
            shakeAmount = intensity;

            //Start the shake if it isn't shaking already
            if (shakeDuration > 0)
            {
                StartCoroutine(ShakeLoop());
            }
        }

        /// <summary>
        /// Shakes the screen.
        /// </summary>
        private IEnumerator ShakeLoop()
        {
            while (shakeDuration > 0)
            {
                camTransform.localPosition = Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime;

                yield return null;
            }

            camTransform.localPosition = Vector3.zero;
        }
    }
}