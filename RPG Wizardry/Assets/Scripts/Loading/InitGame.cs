using nl.SWEG.RPGWizardry.Loading;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Utils
{
    public class InitGame : MonoBehaviour
    {
        /// <summary>
        /// Runs Intro, then Loads Main Menu
        /// </summary>
        private void Start()
        {
            // TODO: run Intro
            SceneLoader.Instance.LoadMenuScene(); // Start game by loading menu-scene
        }
    }
}