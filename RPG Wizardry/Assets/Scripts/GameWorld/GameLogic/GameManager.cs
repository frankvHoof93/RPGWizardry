using System;
using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Utils;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nl.SWEG.RPGWizardry
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
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
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Toggles Game-Pause
        /// </summary>
        public void TogglePause()
        {
            Paused = !Paused;
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
            if (arg0.name != Constants.GameSceneName)
                return; // GameScene was not loaded Scene
            if (arg1 != LoadSceneMode.Single)
                return; // GameScene was not loaded Single (Menu-Exit)
            Debug.LogWarning("Start new Game");
            SceneManager.sceneLoaded -= InitGame;
        }
        /// <summary>
        /// Restarts game (un-pauses) after menu-exit
        /// </summary>
        /// <param name="arg0">Scene that was unloaded (Menu-Scene)</param>
        /// <param name="arg1">Loading-Mode for unloaded scene</param>
        internal void OnExitMenu(Scene arg0)
        {
            SceneManager.sceneUnloaded -= OnExitMenu;
            if (CameraManager.Exists && !CameraManager.Instance.AudioListener.enabled)
                CameraManager.Instance.ToggleAudio();
        }
        #endregion
        #endregion
    }
}