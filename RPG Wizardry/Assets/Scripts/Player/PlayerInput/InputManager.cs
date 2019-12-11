using nl.SWEG.RPGWizardry.GameWorld.GameLogic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Player.PlayerInput
{
    public class InputManager : MonoBehaviour
    {
        #region Variables
        #region Public
        /// <summary>
        /// Current State for Input
        /// </summary>
        public InputState State => inputState;
        #endregion

        #region Editor
        /// <summary>
        /// Current State of GamePlay
        /// TODO: Move elsewhere
        /// </summary>
        [SerializeField]
        private GameState gameState = GameState.GamePlay; //serialized for easy inspector switching (DEBUG)
        /// <summary>
        /// ControlScheme for Input-Reading
        /// </summary>
        [SerializeField]
        [Tooltip("ControlScheme for InputReading")]
        private ControlScheme controlScheme;
        #endregion

        #region Private
        /// <summary>
        /// Current State for Input
        /// </summary>
        private InputState inputState;
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
            //same for keyboard and controller
            inputState.MovementData = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }

        /// <summary>
        /// collects unity aiming input while in gameplay state and saves in inputstate
        /// </summary>
        private void AimingInputs()
        {
            //on controller, use the right stick
            if (controlScheme == ControlScheme.Controller)
                inputState.AimingData = new Vector2(Input.GetAxis("RightX"), Input.GetAxis("RightY")).normalized;
            //on keyboard, use the mouse
            else if (controlScheme == ControlScheme.Keyboard)
            {
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
                Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
                inputState.AimingData = (lookPos - transform.position).normalized;
            }
        }

        /// <summary>
        /// Collects button states
        /// </summary>
        private void ButtonInputs()
        {
            //same for keyboard and controller
            inputState.Cast1 = Input.GetButton("Fire1");
        }
        #endregion
        #endregion
    }
}