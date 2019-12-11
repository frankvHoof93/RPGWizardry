using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nl.SWEG.RPGWizardry.Player;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomSwitcher : MonoBehaviour
    {
        #region Variables     
        /// <summary>
        /// How long it takes to fade in or out.
        /// </summary>
        [SerializeField]
        [Range(0.00f, 2f)]
        private float FadeTime;

        public GameObject Room;

        public GameObject TargetRoom;

        public Transform TargetSpawn;
        
        //camera for fading

        //transform spawnposition
        //original room to disable?
        //new room to activate
        //player to move
        //private PlayerManager
        #endregion

        #region Methods
        #region Private
        /// <summary>
        /// Switches the player 
        /// </summary>
        private IEnumerator SwitchRoom()
        {
            //Lock player controls or game
            GameManager.Instance.Locked = true;

            //Fade camera to black
            LeanTween.value(gameObject, UpdateFadeShader, 1, 0, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //Enable new room
            TargetRoom.SetActive(true);

            //Move player to new room
            if (PlayerManager.Exists)
            {
                PlayerManager.Instance.transform.position = TargetSpawn.position;
            }

            //Disable old room
            Room.SetActive(false);

            //Fade player back in
            LeanTween.value(gameObject, UpdateFadeShader, 0, 1, FadeTime);
            yield return new WaitForSeconds(FadeTime);

            //Unlock player controls or game
            GameManager.Instance.Locked = false;

            //activate enemies in new room

        }

        private void UpdateFadeShader(float value)
        {
            print(value);

            //TODO: tie this value to the fade shader.
        }
        #endregion
        #region Unity
        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            if (PlayerManager.Exists)
            {

            }
        }

        /// <summary>
        /// Checks if the player is hitting the room switch trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            print("collide");
            StartCoroutine(SwitchRoom());
        }
        #endregion
        #endregion
    }
}