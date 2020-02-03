using UnityEngine;

namespace nl.SWEG.Willow.Loading
{
    /// <summary>
    /// Handles UI for GameOver-Scene
    /// </summary>
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