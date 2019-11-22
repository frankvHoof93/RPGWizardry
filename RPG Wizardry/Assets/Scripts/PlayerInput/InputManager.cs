using nl.SWEG.RPGWizardry.GameLogic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.PlayerInput
{
    [RequireComponent(typeof(InputState))]
    public class InputManager : MonoBehaviour
    {
        #region Variables

        #region Private
        private InputState inputState;

        //setting to increase movement speed
        [SerializeField]
        private int movementMultiplier = 1;

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
        /// Gets inputstate component reference
        /// </summary>
        private void Start()
        {
            inputState = GetComponent<InputState>();
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
                    AimingInputs();
                    ButtonInputs();
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// collects unity movement input while in gameplay state and saves in inputstate
        /// </summary>
        private void MovementInputs()
        {
            //collect movement input, multiply for effectiveness
            //same for keyboard and controller
            inputState.MovementData = new Vector3(Input.GetAxis("Horizontal") * movementMultiplier,
                Input.GetAxis("Vertical") * movementMultiplier, 0.0f);
        }

        /// <summary>
        /// collects unity aiming input while in gameplay state and saves in inputstate
        /// </summary>
        private void AimingInputs()
        {
            //on controller, use the right stick
            if (controlScheme == ControlScheme.Controller)
            {
                inputState.AimingData = new Vector3(Input.GetAxis("RightX"),
                    Input.GetAxis("RightY"), 0.0f);
                
            }
            //on keyboard, use the mouse
            else if (controlScheme == ControlScheme.Keyboard)
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
                inputState.AimingData = lookPos - transform.position;
            }
        }

        private void ButtonInputs()
        {
            //collect button states
            //same for keyboard and controller
            inputState.Cast1 = Input.GetButton("Fire1");
        }
        #endregion
        #endregion
    }
}