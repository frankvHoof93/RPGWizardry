using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nl.SWEG.RPGWizardry.Utils.Behaviours;

namespace nl.SWEG.RPGWizardry
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        #region InnerTypes
        public enum GameState
        {
            Menu = 0,
            GamePlay = 1
        }
        #endregion

        #region Variables
        public GameState State { get; private set; } = GameState.Menu;

        public bool Paused { get; private set; } = false;
        #endregion

        public void TogglePause()
        {
            Paused = !Paused;
        }

    }

    
}