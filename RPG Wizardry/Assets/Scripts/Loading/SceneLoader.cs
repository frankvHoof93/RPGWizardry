using nl.SWEG.RPGWizardry.UI;
using nl.SWEG.RPGWizardry.Utils;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine.SceneManagement;

namespace nl.SWEG.RPGWizardry.Loading
{
    public class SceneLoader : SingletonBehaviour<SceneLoader>
    {
        public void LoadMenuScene()
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

        private void InitMenu(Scene arg0, LoadSceneMode arg1)
        {
            bool isAdditive = arg1 == LoadSceneMode.Additive;
            if (isAdditive)
                SceneManager.SetActiveScene(arg0);
            MenuManager.Instance.Init(arg1 == LoadSceneMode.Additive);
            SceneManager.sceneLoaded -= InitMenu;
        }

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

    }
}