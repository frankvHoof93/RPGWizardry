using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld.GameLogic
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        #region Variables
        public GameState GameState;
        public bool Locked;
        #endregion

        #region Methods
        #region Unity
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            Locked = false;
        }
        #endregion
        #endregion
    }

    public enum GameState
    {
        Menu = 0,
        GamePlay = 1
    }
}