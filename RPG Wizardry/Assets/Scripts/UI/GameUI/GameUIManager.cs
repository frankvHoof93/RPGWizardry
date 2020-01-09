using nl.SWEG.RPGWizardry.Loading;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class GameUIManager : SingletonBehaviour<GameUIManager>
    {
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
            CheckPlayerInput();
        }
        #endregion

        #region Private
        /// <summary>
        /// Checks PlayerInput for opening/closing Pause-Menu
        /// </summary>
        private void CheckPlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.TogglePause();
                if (GameManager.Instance.Paused) // Game was running, open Menu
                    SceneLoader.Instance.LoadMenuScene();
                else // Game was Paused. Close Menu
                    SceneLoader.Instance.LoadGameScene();
            }
        }
        #endregion
        #endregion
    }
}