using nl.SWEG.Willow.Loading;
using nl.SWEG.Willow.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.Willow.UI.Game
{
    /// <summary>
    /// Handles General UI-Input during GamePlay
    /// </summary>
    public class GameUIManager : SingletonBehaviour<GameUIManager>
    {
        #region Inner Types
        /// <summary>
        /// Creating openmenu event
        /// </summary>
        public delegate void OpenMenu();
        #endregion

        #region Variables
        /// <summary>
        /// Heads-Up Display for Player
        /// </summary>
        public PlayerHUD HUD => hud;
        /// <summary>
        /// Heads-Up Display for Player
        /// </summary>
        [SerializeField]
        [Tooltip("Heads-Up Display for Player")]
        private PlayerHUD hud;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Checks for Input
        /// </summary>
        private void Update()
        {
            //if this script is disabled, you cannot pause the game through user-input
            CheckPlayerInput();
        }
        #endregion

        #region Private
        /// <summary>
        /// Checks PlayerInput for opening/closing of Pause-Menu
        /// </summary>
        private void CheckPlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.TogglePause();
                if (GameManager.Instance.Paused) // Game was running, open Menu // TODOCLEAN: Check active scene instead
                    SceneLoader.Instance.LoadMenuScene();
                else // Game was Paused. Close Menu
                    SceneLoader.Instance.LoadGameScene();
            }
        }
        #endregion
        #endregion
    }
}