using nl.SWEG.RPGWizardry.GameLogic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.PlayerInput
{
    
    public class InputManager : MonoBehaviour
    {
        #region Variables
        #region Public
        

        //outgoing movement values
        public Vector3 MovementData { get; private set; }
        //outgoing aiming values
        public Vector3 AimingData { get; private set; }

        //setting to increase movement speed
        [SerializeField]
        private int MovementMultiplier = 1;
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
        /// collects unity movement input while in gameplay state
        /// </summary>
        private void MovementInputs()
        {
            //collect movement input, multiply for effectiveness
            //same for keyboard and controller
            MovementData = new Vector3(Input.GetAxis("Horizontal") * MovementMultiplier,
                Input.GetAxis("Vertical") * MovementMultiplier, 0.0f);
        }

        /// <summary>
        /// collects unity aiming input while in gameplay state
        /// </summary>
        private void AimingInputs()
        {
            //on controller, use the right stick
            if (controlScheme == ControlScheme.Controller)
            {
                AimingData = new Vector3(Input.GetAxis("RightX"),
                    Input.GetAxis("RightY"), 0.0f);

            }
            //on keyboard, use the mouse
            else if (controlScheme == ControlScheme.Keyboard)
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
                AimingData = lookPos - transform.position;
            }
        }
        #endregion
        #endregion
    }
}