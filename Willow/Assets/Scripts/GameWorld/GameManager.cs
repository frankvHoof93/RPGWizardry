﻿using nl.SWEG.Willow.GameWorld.Levels;
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
        public bool Paused { get; private set; }

        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Prefab for Player
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab for Player")]
        private GameObject playerPrefab;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
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
        /// Pauses Game (If not yet Paused)
        /// </summary>
        public void PauseGame()
        {
            if (!Paused)
                TogglePause();
        }

        /// <summary>
        /// Resumes Game (if Paused)
        /// </summary>
        public void ResumeGame()
        {
            if (Paused)
                TogglePause();
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
        /// Initializes Game after GameScene has finished Loading
        /// </summary>
        /// <param name="loadedScene">Scene that was Loaded</param>
        /// <param name="loadMode">Way in which Scene was loaded</param>
        internal void InitGame(Scene loadedScene, LoadSceneMode loadMode)
        {
            if (loadedScene.name != Constants.GameSceneName)
                return; // GameScene was not loaded Scene
            if (loadMode != LoadSceneMode.Single)
                return; // GameScene was not loaded Single (Menu-Exit)
            SceneManager.sceneLoaded -= InitGame;
            CameraManager.instance.Fade(0, 1);
            SpawnPlayer(FloorManager.Instance.GetSpawnPoint());
            State = GameState.GamePlay;
            Paused = false;
        }

        /// <summary>
        /// Restarts game (un-pauses) after menu-exit
        /// </summary>
        /// <param name="unloadedScene">Scene that was unloaded (Menu-Scene)</param>
        internal void OnExitMenu(Scene unloadedScene)
        {
            if (unloadedScene.name != Constants.MainMenuSceneName)
                return;
            SceneManager.sceneUnloaded -= OnExitMenu;
            Paused = false;
        }
        #endregion

        #region Unity
        /// <summary>
        /// Sets GameState to GameOver when Application Quits
        /// </summary>
        private void OnApplicationQuit()
        {
            State = GameState.GameOver;
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
            SceneLoader.Instance.LoadGameOverScene();
        }
        #endregion
        #endregion
    }
}