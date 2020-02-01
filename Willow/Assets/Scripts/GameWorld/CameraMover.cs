using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class CameraMover : SingletonBehaviour<CameraMover>
    {
        /// <summary>
        /// Camera-Component for Camera
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// Camera-Velocity (when moving)
        /// </summary>
        private Vector3 velocity = Vector3.zero;

        /// <summary>
        /// Time-amount for Smoothing
        /// </summary>
        [SerializeField]
        [Range(0f, 1f)]
        private float smoothTime = .3f;

        /// <summary>
        /// gets the camera
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Camera = GetComponentInChildren<Camera>();
        }

        /// <summary>
        /// Every frame, updates current position of camera to match player
        /// </summary>
        private void Update()
        {
            if (!PlayerManager.Exists)
                return;

            //convert the position of the player to camera-position
            Transform playerTF = PlayerManager.Instance.transform;
            Vector3 playerPos = new Vector3(
                Mathf.Round(playerTF.position.x * 1000.0f) / 1000.0f,
                Mathf.Round(playerTF.position.y * 1000.0f) / 1000.0f,
                playerTF.position.z - 100.0f);

            //convert the mouse position to camera-position
            Vector3 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);

            //place the camera between the player and mouse coordinated
            Vector3 mouseDir = mousePos - playerPos;
            mouseDir.z = playerTF.position.z - 100.0f;

            //Normalize the magnitude of the mouse direction
            if (mouseDir.magnitude < 1)
                mouseDir.Normalize();

            Vector3 cameraPos = playerPos + mouseDir.normalized * 0.5f * Mathf.Sqrt(mouseDir.magnitude);
            transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, smoothTime);
        }
    }
}