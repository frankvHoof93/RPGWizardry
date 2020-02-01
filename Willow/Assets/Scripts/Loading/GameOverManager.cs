using UnityEngine;

namespace nl.SWEG.RPGWizardry.Loading
{
    public class GameOverManager : MonoBehaviour
    {
        /// <summary>
        /// Loads Main Menu
        /// </summary>
        public void LoadMainMenu()
        {
            SceneLoader.Instance.LoadMenuScene(true);
        }
    }
}