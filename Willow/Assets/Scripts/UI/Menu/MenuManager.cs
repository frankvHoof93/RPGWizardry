using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Loading;
using nl.SWEG.Willow.Serialization;
using nl.SWEG.Willow.Utils.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.Willow.UI.Menu
{
    /// <summary>
    /// Handles Main Menu & Pause Menu
    /// </summary>
    public class MenuManager : SingletonBehaviour<MenuManager>
    {
        #region Variables
        #region Public
        /// <summary>
        /// Transform for Canvas holding Spell-List
        /// </summary>
        public Transform SpellListCanvas => spellListCanvas;
        /// <summary>
        /// Transform for Canvas holding Spell-Details
        /// </summary>
        public Transform SpellCanvas => spellDetailsCanvas;
        /// <summary>
        /// Transform for Canvas for Research-MiniGame
        /// </summary>
        public Transform ResearchCanvas => researchCanvas;
        /// <summary>
        /// Panel for Main Menu
        /// </summary>
        public Transform MainMenuPanel => mainMenuPanel.transform;
        /// <summary>
        /// Panel for Pause-Menu
        /// </summary>
        public Transform PauseMenuPanel => pauseMenuPanel.transform;
        #endregion

        #region Editor
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
        [Header("Game Menu")]
        /// <summary>
        /// Panel with PauseMenu-Options
        /// </summary>
        [SerializeField]
        private GameObject pauseMenuPanel;
        #endregion

        #region Spell Canvases
        [Header("Spell Canvases")]
        /// <summary>
        /// Transform for Canvas holding Spell-List
        /// </summary>
        [SerializeField]
        [Tooltip("Transform for Canvas holding Spell-List")]
        private Transform spellListCanvas;
        /// <summary>
        /// Transform for Canvas holding Spell-Details
        /// </summary>
        [SerializeField]
        [Tooltip("Transform for Canvas holding Spell-Details")]
        private Transform spellDetailsCanvas;
        /// <summary>
        /// Transform for Canvas for Research-MiniGame
        /// </summary>
        [SerializeField]
        [Tooltip("Transform for Canvas for Research-MiniGame")]
        private Transform researchCanvas;
        #endregion
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Initializes Menu on Scene-Load
        /// </summary>
        /// <param name="loadPauseMenu">Whether to load Pause-Menu (or Main Menu)</param>
        public void Init(bool loadPauseMenu)
        {
            background.SetActive(!loadPauseMenu);
            if (CameraManager.Exists && CameraManager.Instance.AudioListener.enabled)
                CameraManager.Instance.ToggleAudio();
            if (!loadPauseMenu)
                InitMainMenu();
            else
                InitPauseMenu();
        }

        /// <summary>
        /// Starts a new run
        /// </summary>
        public void StartGame()
        {
            background.SetActive(false);
            SceneLoader.Instance.LoadGameScene();
        }

        /// <summary>
        /// Ends current run without saving
        /// </summary>
        public void EndRun()
        {
            SceneLoader.Instance.LoadMenuScene(true);
        }

        /// <summary>
        /// Quits Game
        /// <para>
        /// TODO: Save Game to File
        /// </para>
        /// </summary>
        /// <param name="saveGame">TODO: Whether to pause the game</param>
        public void QuitGame(bool saveGame)
        {
            if (saveGame)
                Debug.Log("Saving Game");
            Debug.Log("Quit");
            Application.Quit();
        }

        /// <summary>
        /// TODO: Opens Settings-Menu
        /// </summary>
        public void OpenSettingsMenu()
        {

        }
        #endregion

        #region Unity
        /// <summary>
        /// Deactivates all Menu-Items
        /// </summary>
        private void OnEnable()
        {
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
        }
        #endregion

        #region Private
        /// <summary>
        /// Initializes Pause-Menu
        /// </summary>
        private void InitPauseMenu()
        {
            pauseMenuPanel.SetActive(true);
        }

        /// <summary>
        /// Initializes Main Menu
        /// </summary>
        private void InitMainMenu()
        {
            mainMenuPanel.SetActive(true);
            loadGameButton.interactable = SaveManager.HasSave();
        }
        #endregion
        #endregion
    }
}