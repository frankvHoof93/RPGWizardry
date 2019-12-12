using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Camera))]
    public class CameraManager : SingletonBehaviour<CameraManager>
    {
        /// <summary>
        /// Camera-Component for Camera
        /// </summary>
        public Camera Camera { get; private set; }

        #region Methods
        /// <summary>
        /// Grabs Reference to Camera
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            Camera = GetComponent<Camera>();
        }

        /// <summary>
        /// Every frame, updates current position of camera to match player
        /// </summary>
        private void Update()
        {
            if (!PlayerManager.Exists)
                return;

            Transform playerTF = PlayerManager.Instance.transform;
            transform.position = new Vector3(
                playerTF.position.x, playerTF.position.y, playerTF.position.z-10);
        }
        #endregion
    }
}