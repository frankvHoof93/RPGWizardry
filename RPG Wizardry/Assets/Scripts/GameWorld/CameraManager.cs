using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        #region Fields
        /// <summary>
        /// Transform of the player
        /// </summary>
        private Transform playerTransform;

        /// <summary>
        /// How long it takes to fade in or out.
        /// </summary>
        [SerializeField]
        [Range(0.00f, 2f)]
        private float FadeTime;
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Starts the switchRoom coroutine.
        /// </summary>
        /// <param name="previous">The room the player is currently in.</param>
        /// <param name="next">The room the player needs to go.</param>
        /// <param name="spawn">the place where the player needs to end up.</param>
        public void SwitchRoom(GameObject previous, GameObject next, Transform spawn)
        {
            StartCoroutine(switchRoom(previous, next, spawn));
        }
        #endregion

        #region Private
        /// <summary>
        /// moves the player between 2 rooms, and handles room visibility accordingly.
        /// </summary>
        /// <param name="previous">The room the player is currently in.</param>
        /// <param name="next">The room the player needs to go.</param>
        /// <param name="spawn">the place where the player needs to end up.</param>
        /// <returns></returns>
        private IEnumerator switchRoom(GameObject previous, GameObject next, Transform spawn)
        {
            //Make sure the player cant move
            GameManager.Instance.Locked = true;

            //Fade the screen out
            LeanTween.value(gameObject, UpdateShader, 1, 0, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //Enable the new room
            next.SetActive(true);

            //Move the player to new room
            if (PlayerManager.Exists)
            {
                PlayerManager.Instance.transform.position = spawn.position;
            }

            //Disable the old room
            previous.SetActive(false);

            //Fade the screen back in
            LeanTween.value(gameObject, UpdateShader, 0, 1, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //Make sure the player can move again
            GameManager.Instance.Locked = false;

            //Activate enemies in new room
        }

        /// <summary>
        /// Updates the shader used for room transitioning.
        /// </summary>
        /// <param name="value">A value between 0 and 1.</param>
        private void UpdateShader(float value)
        {
            print(value);

            //TODO: tie this value to the fade shader.
        }
        #endregion

        #region Unity
        /// <summary>
        /// Gets reference to player transform
        /// </summary>
        // Start is called before the first frame update
        private void Start()
        {
            if (PlayerManager.Exists)
            {
                playerTransform = PlayerManager.Instance.transform;
            }
        }

        /// <summary>
        /// Every frame, updates current position of camera to match player
        /// </summary>
        private void Update()
        {
            transform.position = new Vector3(
                Mathf.Round(playerTransform.position.x * 1000.0f) / 1000.0f,
                Mathf.Round(playerTransform.position.y * 1000.0f) / 1000.0f,
                Mathf.Round(playerTransform.position.z - 500.00f * 1000.0f) / 1000.0f);
        }
        #endregion
        #endregion
    }
}