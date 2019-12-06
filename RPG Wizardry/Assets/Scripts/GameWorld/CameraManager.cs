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
        private Transform playerTransform;

        // Start is called before the first frame update
        void Start()
        {
            if (PlayerManager.Exists)
            {
                playerTransform = PlayerManager.Instance.GetComponent<Transform>();
                
            }
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(
                playerTransform.position.x, playerTransform.position.y, playerTransform.position.z-10);
            transform.position.Normalize();
        }
    }

}