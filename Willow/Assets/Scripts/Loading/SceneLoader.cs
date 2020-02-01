using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.UI;
using nl.SWEG.Willow.UI.Game;
using nl.SWEG.Willow.Utils;
using nl.SWEG.Willow.Utils.Behaviours;
using nl.SWEG.Willow.UI.Dialogue;
using UnityEngine;
using UnityEngine.SceneManagement;
using nl.SWEG.Willow.UI.Menu;

namespace nl.SWEG.Willow.Loading
{
    public class SceneLoader : SingletonBehaviour<SceneLoader>
    {
        #region Methods
        #region Public
        /// <summary>
        /// Loads MainMenu-Scene
        /// </summary>
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
            }
            UnloadMenuScene(); // Unload Menu-Scene
        }
        /// <summary>
        /// Loads GameOver-Scene
        /// </summary>
        public void LoadGameOverScene()
        {
            SceneManager.LoadScene(Constants.GameOverSceneName, LoadSceneMode.Additive);
            UnloadGameSceneSingletons();
        }
        /// <summary>
        /// Loads the boss scene
        /// </summary>
        public void LoadBossScene()
        {
            UnloadGameSceneSingletons();
            SceneManager.LoadScene(Constants.VictorySceneName, LoadSceneMode.Single);
        }
        #endregion

        #region Private
        /// <summary>
        /// Initializes MainMenu after Scene-Load
        /// </summary>
        /// <param name="arg0">Scene that was loaded (Menu-Scene)</param>
        /// <param name="arg1">LoadSceneMode for Scene-Load (Single/Additive)</param>
        private void InitMenu(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != Constants.MainMenuSceneName)
                return;
            bool isAdditive = arg1 == LoadSceneMode.Additive;
            if (isAdditive)
                SceneManager.SetActiveScene(arg0);
            MenuManager.Instance.Init(arg1 == LoadSceneMode.Additive);
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
                if (GameManager.Exists)
                    SceneManager.sceneUnloaded += GameManager.Instance.OnExitMenu;
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
            if (CameraMover.Exists)
                Destroy(CameraMover.Instance.gameObject);
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