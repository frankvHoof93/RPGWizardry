using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Loading;
using nl.SWEG.Willow.Utils;
using nl.SWEG.Willow.Utils.Behaviours;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nl.SWEG.Willow.UI.Game
{
    /// <summary>
    /// Handles General UI-Input & Feedback during GamePlay
    /// </summary>
    public class GameUIManager : SingletonBehaviour<GameUIManager>
    {
        #region InnerTypes
        /// <summary>
        /// Type of Cursor for Mouse
        /// </summary>
        public enum CursorType : byte
        {
            /// <summary>
            /// Hand-Cursor
            /// </summary>
            Cursor = 0,
            /// <summary>
            /// Crosshair-Cursor
            /// </summary>
            Crosshair = 1
        }
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
        /// Cursor during Gameplay
        /// </summary>
        [SerializeField]
        private Texture2D crosshair;
        /// <summary>
        /// Cursor for Menus
        /// </summary>
        [SerializeField]
        private Texture2D cursor;
        /// <summary>
        /// Hotspot for Crosshair-Cursor
        /// </summary>
        private Vector2 crosshairHotspot => new Vector2(crosshair?.width / 2f ?? 0, crosshair?.height / 2f ?? 0);
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Sets which Cursor to Display
        /// </summary>
        /// <param name="cursor">CursorType for Cursor to Display</param>
        public void SetCursor(CursorType cursor)
        {
            switch (cursor)
            {
                case CursorType.Cursor:
                    Cursor.SetCursor(this.cursor, Vector2.zero, CursorMode.Auto);
                    break;
                case CursorType.Crosshair:
                    Cursor.SetCursor(crosshair, crosshairHotspot, CursorMode.Auto);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Unity
        /// <summary>
        /// Sets Cursor to Crosshair
        /// </summary>
        private void Start()
        {
            SetCursor(CursorType.Crosshair);
        }

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
                if (SceneManager.GetActiveScene().name == Constants.GameSceneName) // Game was running, open Menu
                {
                    SceneLoader.Instance.LoadMenuScene();
                    SetCursor(CursorType.Cursor);
                    if (!GameManager.Instance.Paused)
                        GameManager.Instance.TogglePause(); // Pause Game
                }
                else if (SceneManager.GetActiveScene().name == Constants.MainMenuSceneName) // Game was Paused. Close Menu
                {
                    SceneLoader.Instance.LoadGameScene();
                    SetCursor(CursorType.Crosshair);
                    if (GameManager.Instance.Paused)
                        GameManager.Instance.TogglePause(); // Unpause Game
                }
            }
        }
        #endregion
        #endregion
    }
}