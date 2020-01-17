using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    /// <summary>
    /// A special door that loads into the boss scene
    /// </summary>
    public class BossDoor : Door
    {
        /// <summary>
        /// When the player enters the collider, unloads the game room, and loads the Boss room
        /// </summary>
        /// <param name="collision">The object entering the trigger</param>
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            //Make sure the game is paused
            if (!GameManager.Instance.Paused)
                GameManager.Instance.TogglePause();

            //Fade the screen out

            CameraManager.Instance.Fade(1, 0);
            while (CameraManager.Instance.Fading)
            {
                yield return null;
            }

            Loading.SceneLoader.Instance.LoadBossScene();
        }
    }
}