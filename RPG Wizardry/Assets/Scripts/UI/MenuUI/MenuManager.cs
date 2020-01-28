﻿using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Loading;
using nl.SWEG.RPGWizardry.Serialization;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI
{
    public class MenuManager : SingletonBehaviour<MenuManager>
    {
        #region Inner Types
        /// <summary>
        /// Delegate for Entering Main Menu event
        /// </summary>
        public delegate void OnMenu();
        /// <summary>
        /// Delegate for Entering Main Menu event
        /// </summary>
        public delegate void OnSpellMenu();
        #endregion

        #region Variables
        /// <summary>
        /// Background for Menu. Only shown if Game is not running
        /// </summary>
        [SerializeField]
        [Tooltip("Background for Menu. Only shown if Game is not running")]
        private GameObject background;
        #region MainMenu
        [Header("Main Menu")]
        /// <summary>
        /// Panel with MainMenu-Options
        /// </summary>
        [SerializeField]
        [Tooltip("Panel with MainMenu-Options")]
        private GameObject mainMenuPanel;
        /// <summary>
        /// Load Game-Button. Disabled if no Save-File exists
        /// </summary>
        [SerializeField]
        [Tooltip("Load Game-Button. Disabled if no Save-File exists")]
        private Button loadGameButton;
        #endregion

        #region GameMenu
        public GameObject GameMenu => gameMenuPanel;



        [Header("Game Menu")]
        /// <summary>
        /// Panel with PauseMenu-Options
        /// </summary>
        [SerializeField]
        private GameObject gameMenuPanel;
        #endregion

        #region 
        [Header("Spell Canvases")]

        public GameObject SpellListCanvas;
        public GameObject SpellCanvas;
        public GameObject ScrollpageCanvas;
        #endregion
        #endregion

        #region Methods
        #region Public
        public void Init(bool additiveLoad)
        {
            background.SetActive(!additiveLoad);
            if (CameraManager.Exists)
                CameraManager.Instance.ToggleAudio();
            if (!additiveLoad)
                InitMainMenu();
            else
                InitGameMenu();
        }

        public void StartGame()
        {
            SceneLoader.Instance.LoadGameScene();
        }

        public void EndRun()
        {
            SceneLoader.Instance.LoadMenuScene(true);
        }

        public void QuitGame(bool saveGame)
        {
            if (saveGame)
                Debug.Log("Saving Game");
            Debug.Log("Quit");
            Application.Quit();
        }

        public void OpenSettingsMenu()
        {

        }
        #endregion

        #region Unity
        private void OnEnable()
        {
            mainMenuPanel.SetActive(false);
            gameMenuPanel.SetActive(false);
        }
        #endregion

        #region Private

        /// <summary>
        /// Event called when Main Menu is entered
        /// </summary>
        private event OnMenu onMenuEnter;
        private event OnMenu onMenuExit;
        private event OnSpellMenu onSpellMenuEnter;

        private void InitGameMenu()
        {
            gameMenuPanel.SetActive(true);
        }

        private void InitMainMenu()
        {
            mainMenuPanel.SetActive(true);
            loadGameButton.interactable = SaveManager.HasSave();
        }
        #endregion

       

        #endregion
    }
}