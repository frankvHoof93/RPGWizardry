using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Loading;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Player.Movement;
using nl.SWEG.Willow.UI.Game;
using nl.SWEG.Willow.Utils;
using nl.SWEG.Willow.Utils.Behaviours;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nl.SWEG.Willow
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        [SerializeField]
        private Texture2D crosshair;
        [SerializeField]
        private Texture2D cursor;

        private Vector2 crosshairHotspot;

        #region InnerTypes
        public enum GameState
        {
            Menu = 0,
            GamePlay = 1,
            GameOver = 2
        }
        #endregion

        #region Variables
        /// <summary>
        /// Current GameState
        /// </summary>
        public GameState State { get; private set; } = GameState.Menu;
        /// <summary>
        /// Whether the Game is currently Paused
        /// </summary>
        public bool Paused { get; private set; } = false;

        #region Editor
        /// <summary>
        /// Prefab for Player
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab for Player")]
        private GameObject playerPrefab;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Spawns Player
        /// </summary>
        /// <param name="position">Position (WorldSpace) to spawn Player at</param>
        public void SpawnPlayer(Vector3 position)
        {
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = position;
            GameUIManager.Instance.HUD.Initialize();
        }
        /// <summary>
        /// Toggles Game-Pause
        /// </summary>
        public void TogglePause()
        {
            Paused = !Paused;
            if (Paused) // This is nasty, but it should work. It pauses ALL active tweens, but still allows new tweens to be started/ran.
                LeanTween.pauseAll(); 
            else
                LeanTween.resumeAll(); // Resumes ALL tweens (including the ones that are NOT in GameState.Playing
            if (Paused && PlayerManager.Exists)
                PlayerManager.Instance.GetComponent<MovementManager>().FreezeMovement();
            //Set cursor to cursor if paused, crosshair if unpaused
            Cursor.SetCursor(Paused ? cursor : crosshair, Paused ? Vector2.zero : crosshairHotspot, CursorMode.Auto);
        }

        public void EndGame(bool gameOver)
        {
            State = GameState.GameOver;
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            if (gameOver)
            {
                StartCoroutine(GameOver());
            }
            else
            {
                // TODO: Save Game
                // TODO: Load Main Menu
            }
        }
        #endregion

        #region SceneLoad
        /// <summary>
        /// Initiazes Game after GameScene has finished Loading
        /// </summary>
        /// <param name="arg0">Scene that was Loaded</param>
        /// <param name="arg1"></param>
        internal void InitGame(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= InitGame;
            if (arg0.name != Constants.GameSceneName)
                return; // GameScene was not loaded Scene
            if (arg1 != LoadSceneMode.Single)
                return; // GameScene was not loaded Single (Menu-Exit)
            FloorManager.Instance.LoadFloor();
            State = GameState.GamePlay;
            Paused = false;
            Cursor.SetCursor(crosshair,crosshairHotspot,CursorMode.Auto);
        }

        /// <summary>
        /// Restarts game (un-pauses) after menu-exit
        /// </summary>
        /// <param name="arg0">Scene that was unloaded (Menu-Scene)</param>
        /// <param name="arg1">Loading-Mode for unloaded scene</param>
        internal void OnExitMenu(Scene arg0)
        {
            SceneManager.sceneUnloaded -= OnExitMenu;
            Paused = false;
            if (CameraManager.Exists && !CameraManager.Instance.AudioListener.enabled)
                CameraManager.Instance.ToggleAudio();
        }
        #endregion

        #region Unity
        private void Start()
        {
            crosshairHotspot = new Vector2(crosshair.width / 2f, crosshair.height / 2f);
        }
        #endregion

        #region Private
        private IEnumerator GameOver()
        {
            // TODO: Animation
            // TODO: Delete save game
            yield return new WaitForSeconds(2f);
            CameraManager.Instance.ToggleAudio();
            SceneLoader.Instance.LoadGameOverScene();
        }
        #endregion
        #endregion
    }
}