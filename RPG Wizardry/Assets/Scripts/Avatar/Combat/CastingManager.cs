using System.Collections;
using System.Collections.Generic;
using nl.SWEG.RPGWizardry.PlayerInput;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar.Combat
{
    [RequireComponent(typeof(InputState))]
    public class CastingManager : MonoBehaviour
    {
        #region Variables
        #region Public
        /// <summary>
        /// Prototype projectile; fill this with selected spell later
        /// </summary>
        public Projectile projectilePrefab;
        #endregion
        #region Private
        /// <summary>
        /// Transform of the object the projectiles need to spawn from
        /// </summary>
        [SerializeField]
        private Transform spawnLocation;
        /// <summary>
        /// Inputstate for getting button states
        /// </summary>
        private InputState inputState;
        /// <summary>
        /// Cooldown state during which you cannot cast spells
        /// </summary>
        private bool cooldown = false;
        #endregion
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Grabs inputstate reference for button presses
        /// </summary>
        void Start()
        {
            inputState = GetComponent<InputState>();
        }

        /// <summary>
        /// If the button is pressed and there's no cooldown, fire the spell
        /// </summary>
        void Update()
        {
            if (inputState.Cast1)
            {
                if (!cooldown)
                {
                    SpawnProjectile(projectilePrefab);
                }
            }
        }
        #endregion
        #region Private

        /// <summary>
        /// Spawns a projectile, then puts the casting system on cooldown based on the spell
        /// </summary>
        /// <param name="projectile">Projectile of the spell to cast; contains cooldown value</param>
        void SpawnProjectile(Projectile projectile)
        {
            Instantiate(projectile.gameObject, spawnLocation.position, spawnLocation.rotation);
            StartCoroutine(Cooldown(projectile.Cooldown));
        }

        /// <summary>
        /// Cooldown state during which no spells may be cast
        /// </summary>
        /// <param name="coolSeconds">Seconds the cooldown should remain active</param>
        /// <returns></returns>
        IEnumerator Cooldown(int coolSeconds)
        {
            cooldown = true;
            yield return new WaitForSeconds(coolSeconds);
            cooldown = false;

        }
        #endregion
        #endregion
    }
}