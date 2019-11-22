using System.Collections;
using System.Collections.Generic;
using nl.SWEG.RPGWizardry.PlayerInput;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Avatar.Combat
{
    [RequireComponent(typeof(InputState))]
    public class CastingManager : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnLocation;
        public GameObject projectilePrefab;
        private InputState inputState;

        // Start is called before the first frame update
        void Start()
        {
            inputState = GetComponent<InputState>();
        }

        // Update is called once per frame
        void Update()
        {
            if (inputState.Cast1)
            {
                SpawnProjectile(projectilePrefab);
            }
        }

        void SpawnProjectile(GameObject projectile)
        {
            Instantiate(projectile, spawnLocation.position, spawnLocation.rotation);
        }
    }

}