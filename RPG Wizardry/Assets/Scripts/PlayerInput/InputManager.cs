using nl.SWEG.RPGWizardry.Avatar.Movement;
using nl.SWEG.RPGWizardry.GameLogic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        #region Variables
        #region Public

        //outgoing movement values
        public Vector3 InputMovement { get; private set; }
        //outgoing aiming values
        public Vector3 InputAiming { get; private set; }

        public int MovementMultiplier = 1;
        #endregion

        #region Private
        //serialized for easy inspector switching (DEBUG)
        [SerializeField]
        private GameState gameState = GameState.GamePlay;
        /// <summary>
        /// ControlScheme for Input
        /// </summary>
        [SerializeField]
        private ControlScheme controlScheme;
        #endregion
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Checks inputs based on gamestate
        /// </summary>
        private void Update()
        {
            switch (gameState)
            {
                case GameState.GamePlay:
                    MovementInputs();
                    AimingInputs();
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
            InputMovement = new Vector3(Input.GetAxis("Horizontal") * MovementMultiplier,
                Input.GetAxis("Vertical") * MovementMultiplier, 0.0f);
        }


        private void AimingInputs()
        {
            //collect aiming input
            InputAiming = new Vector3(Input.GetAxis("RightX"),
                Input.GetAxis("RightY"), 0.0f);
        }
        #endregion
        #endregion
    }
}