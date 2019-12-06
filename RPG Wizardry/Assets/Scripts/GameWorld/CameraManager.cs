﻿using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        /// <summary>
        /// Transform of the player
        /// </summary>
        private Transform playerTransform;

        #region Methods
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
                playerTransform.position.x, playerTransform.position.y, playerTransform.position.z-10);
        }
        #endregion
    }
}