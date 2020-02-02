using nl.SWEG.Willow.Loading;
using nl.SWEG.Willow.UI.Game;
using nl.SWEG.Willow.Utils;
using nl.SWEG.Willow.Utils.Behaviours;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nl.SWEG.Willow.GameWorld
{
    /// <summary>
    /// Handles GameState (e.g. Initialization, EndGame, etc.)
    /// </summary>
    public class GameManager : SingletonBehaviour<GameManager>
    {
        #region InnerTypes
        /// <summary>
        /// State of GamePlay
        /// </summary>
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
        }
        
        /// <summary>
        /// Ends current Run
        /// </summary>
        /// <param name="gameOver">Whether to move to the GameOver-Scene (Player Died)</param>
        public void EndGame(bool gameOver)
        {
            State = GameState.GameOver;
            GameUIManager.Instance.SetCursor(GameUIManager.CursorType.Cursor);
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
        /// <param name="loadedScene">Scene that was Loaded</param>
        /// <param name="loadMode">Way in which Scene was loaded</param>
        internal void InitGame(Scene loadedScene, LoadSceneMode loadMode)
        {
            SceneManager.sceneLoaded -= InitGame;
            if (loadedScene.name != Constants.GameSceneName)
                return; // GameScene was not loaded Scene
            if (loadMode != LoadSceneMode.Single)
                return; // GameScene was not loaded Single (Menu-Exit)
            State = GameState.GamePlay;
            Paused = false;
        }

        /// <summary>
        /// Restarts game (un-pauses) after menu-exit
        /// </summary>
        /// <param name="unloadedScene">Scene that was unloaded (Menu-Scene)</param>
        internal void OnExitMenu(Scene unloadedScene)
        { // TODOCLEAN: Check this
            SceneManager.sceneUnloaded -= OnExitMenu;
            Paused = false;
            if (CameraManager.Exists && !CameraManager.Instance.AudioListener.enabled)
                CameraManager.Instance.ToggleAudio();
        }
        #endregion
        
        #region Private
        /// <summary>
        /// Handles End of Game at Player Death
        /// </summary>
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