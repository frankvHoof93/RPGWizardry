using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.GameWorld.Levels;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.UI.Dialogue;
using nl.SWEG.Willow.UI.Game;
using nl.SWEG.Willow.UI.Menu;
using nl.SWEG.Willow.Utils;
using nl.SWEG.Willow.Utils.Behaviours;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nl.SWEG.Willow.Loading
{
    /// <summary>
    /// Handles Loading & Unloading of Scenes
    /// </summary>
    public class SceneLoader : SingletonBehaviour<SceneLoader>
    {
        #region Methods
        #region Public
        /// <summary>
        /// Loads MainMenu-Scene
        /// </summary>
        /// <param name="forceLoad">Force full reload of Main Menu (Unloads current Run)</param>
        public void LoadMenuScene(bool forceLoad = false)
        {
            if (forceLoad)
            {
                UnloadMenuScene();
                UnloadGameSceneSingletons();
                SceneManager.sceneLoaded += InitMenu;
                SceneManager.LoadScene(Constants.MainMenuSceneName, LoadSceneMode.Single); // Load Scene SINGLE
            }
            else
            {
                Scene activeScene = SceneManager.GetActiveScene();
                if (activeScene.name == Constants.InitSceneName) // Initializing
                {
                    SceneManager.sceneLoaded += InitMenu;
                    SceneManager.LoadScene(Constants.MainMenuSceneName, LoadSceneMode.Single); // Load Scene SINGLE
                }
                else if (activeScene.name != Constants.MainMenuSceneName) // In-Game
                {
                    SceneManager.sceneLoaded += InitMenu;
                    SceneManager.LoadScene(Constants.MainMenuSceneName, LoadSceneMode.Additive); // Load Scene ADDITIVE
                }
            }
        }

        /// <summary>
        /// Loads Game-Scene
        /// </summary>
        public void LoadGameScene()
        {
            Scene gameScene = SceneManager.GetSceneByName(Constants.GameSceneName);
            // If GameScene is NOT Loaded:
            if (!gameScene.isLoaded)
            {
                SceneManager.sceneLoaded += GameManager.Instance.InitGame; // Start a new Game (when scene is loaded)
                SceneManager.LoadSceneAsync(Constants.GameSceneName, LoadSceneMode.Single); // Single-Load Scene
                if (MenuManager.Exists)
                    Destroy(MenuManager.Instance.gameObject);
            }
            else
            {
                UnloadMenuScene(); // Unload Menu-Scene
            }
        }

        /// <summary>
        /// Loads GameOver-Scene
        /// </summary>
        public void LoadGameOverScene()
        {
            GameUIManager.Instance.SetCursor(GameUIManager.CursorType.Cursor);
            UnloadGameSceneSingletons();            
            SceneManager.LoadScene(Constants.GameOverSceneName, LoadSceneMode.Additive);
        }

        /// <summary>
        /// Loads the boss scene
        /// </summary>
        public void LoadBossScene()
        {
            // TODO: Remove this if actually spawning a Boss
            GameUIManager.Instance.SetCursor(GameUIManager.CursorType.Cursor);
            UnloadGameSceneSingletons();
            // TODOEND
            SceneManager.LoadScene(Constants.VictorySceneName, LoadSceneMode.Single);
        }
        #endregion

        #region Private
        /// <summary>
        /// Initializes MainMenu after Scene-Load
        /// </summary>
        /// <param name="loadedScene">Scene that was loaded (Menu-Scene)</param>
        /// <param name="loadMode">LoadSceneMode for Scene-Load (Single/Additive)</param>
        private void InitMenu(Scene loadedScene, LoadSceneMode loadMode)
        {
            if (loadedScene.name != Constants.MainMenuSceneName)
                return;
            bool isAdditive = loadMode == LoadSceneMode.Additive;
            if (isAdditive)
                SceneManager.SetActiveScene(loadedScene);
            MenuManager.Instance.Init(loadMode == LoadSceneMode.Additive);
            SceneManager.sceneLoaded -= InitMenu;
        }

        /// <summary>
        /// Unloads Menu-Scene, destroying appropriate Singletons
        /// </summary>
        private void UnloadMenuScene()
        {
            if (MenuManager.Exists)
                Destroy(MenuManager.Instance.gameObject);
            Scene menuScene = SceneManager.GetSceneByName(Constants.MainMenuSceneName); // Only works if scene is Loaded (provides invalid, non-loaded scene otherwise)
            if (menuScene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(menuScene);
            }
        }

        /// <summary>
        /// Unloads Singletons for GameScene
        /// </summary>
        private void UnloadGameSceneSingletons()
        {
            if (CameraManager.Exists)
                Destroy(CameraManager.Instance.gameObject);
            if (DialogueManager.Exists)
                Destroy(DialogueManager.Instance.gameObject);
            if (FloorManager.Exists)
                Destroy(FloorManager.Instance.gameObject);
            if (GameUIManager.Exists)
            {
                Destroy(GameUIManager.Instance.HUD.gameObject);
                Destroy(GameUIManager.Instance.gameObject);
            }
            if (LootSpawner.Exists)
            {
                Transform parent = LootSpawner.Instance.transform.parent;
                Destroy(LootSpawner.Instance.gameObject);
                Destroy(parent.gameObject);
            }
            if (PlayerManager.Exists)
                Destroy(PlayerManager.Instance.gameObject);
        }
        #endregion
        #endregion
    }
}