using UnityEngine;

namespace nl.SWEG.RPGWizardry.Entities.Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    public class EnemyData : ScriptableObject
    {
        public string Name => enemyName;
        public int Health => enemyHealth;
        public int Attack => enemyAttack;
        public float Speed => enemySpeed;

        [SerializeField]
        private string enemyName;
        [SerializeField]
        private int enemyHealth;
        [SerializeField]
        private int enemyAttack;
        [SerializeField]
        private float enemySpeed;
    }
}