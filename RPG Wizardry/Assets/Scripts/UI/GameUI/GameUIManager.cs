using nl.SWEG.RPGWizardry.Loading;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.UI.GameUI
{
    public class GameUIManager : SingletonBehaviour<GameUIManager>
    {
        #region Inner Types
        /// <summary>
        /// Creating openmenu event
        /// </summary>
        public delegate void openMenu();
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

        /// <summary>
        /// Event called when menu is opened
        /// </summary>
        private event openMenu openMenuTutorial;
        #endregion

        #region Eventlisteners
        /// <summary>
        /// Adds Listener to openMenu-Event
        /// </summary>
        /// <param name="listener">Listener to Add</param>
        public void AddMenuListener(openMenu listener)
        {
            openMenuTutorial += listener;
        }
        /// <summary>
        /// Removes Listener from OpenMenu-event
        /// </summary>
        /// <param name="listener">Listener to Remove</param>
        public void RemoveMenuListener(openMenu listener)
        {
            openMenuTutorial -= listener;
        }
        #endregion

        #region Methods
        #region Unity

        private void Start()
        {
            enabled = false;
        }

        /// <summary>
        /// Checks for Input
        /// </summary>
        private void Update()
        {
            //if this script is disabled, you cannot pause
            CheckPlayerInput();
        }
        #endregion

        #region Private
        /// <summary>
        /// Checks PlayerInput for opening/closing Pause-Menu
        /// </summary>
        private void CheckPlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //Added the Pause Allowed check
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