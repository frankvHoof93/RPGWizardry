using nl.SWEG.RPGWizardry.Avatar.Movement;
using nl.SWEG.RPGWizardry.GameLogic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        #region Variables
        #region Public
        public int MovementMultiplier = 1;
        #endregion

        #region Private
        //serialized for easy inspector switching (DEBUG)
        [SerializeField]
        private GameState gameState = GameState.GamePlay;
        private MovementManager movementManager;
        #endregion
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Grabs reference to MovementManager
        /// </summary>
        private void Start()
        {
            movementManager = GetComponent<MovementManager>();
        }

        /// <summary>
        /// Checks inputs based on gamestate
        /// </summary>
        private void Update()
        {
            switch (gameState)
            {
                case GameState.GamePlay:
                    MovementInputs();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// collects unity input while in gameplay state
        /// </summary>
        private void MovementInputs()
        {
            //collect movement input, multiply for effectiveness
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal") * MovementMultiplier,
                Input.GetAxis("Vertical") * MovementMultiplier, 0.0f);
            //send to movementmanager
            movementManager.movementInput = movement;
        }
        #endregion
        #endregion
    }
}