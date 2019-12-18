using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nl.SWEG.RPGWizardry.Utils.Behaviours;

namespace nl.SWEG.RPGWizardry
{
    public class GameManager : SingletonBehaviour<GameManager>
    {
        #region Variables
        public GameState GameState;
        public bool Locked;
        #endregion
    }

    public enum GameState
    {
        Menu = 0,
        GamePlay = 1
    }
}