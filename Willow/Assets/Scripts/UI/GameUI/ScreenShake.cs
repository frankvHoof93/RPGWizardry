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
        /// The coroutine that's currently running.
        /// </summary>
        private Coroutine CurrentShake;

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
            if (CurrentShake != null)
                StopCoroutine(ShakeLoop(intensity, duration));

            StartCoroutine(ShakeLoop(intensity, duration));
        }

        /// <summary>
        /// Shakes the screen.
        /// </summary>
        private IEnumerator ShakeLoop(float intensity, float duration)
        {
            while (duration > 0)
            {
                camTransform.localPosition = Random.insideUnitSphere * intensity;
                duration -= Time.deltaTime;

                yield return null;
            }

            camTransform.localPosition = Vector3.zero;
        }
    }
}