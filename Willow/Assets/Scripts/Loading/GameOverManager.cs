using UnityEngine;

namespace nl.SWEG.Willow.Loading
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