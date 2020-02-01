namespace nl.SWEG.Willow.Entities.Stats
{
    /// <summary>
    /// Delegate for Health-Change
    /// </summary>
    /// <param name="newHealth">Health after Change</param>
    /// <param name="maxHealth">Max Health for Entity</param>
    /// <param name="change">Change that occurred</param>
    public delegate void OnHealthChange(ushort newHealth, ushort maxHealth, short change);
    public delegate void Die();
    /// <summary>
    /// Interface for Entities with Health
    /// </summary>
    public interface IHealth
    {
        /// <summary>
        /// Current Health of the Entity
        /// </summary>
        ushort Health { get; }
        /// <summary>
        /// Heals Entity
        /// </summary>
        /// <param name="amount">Amount to Heal for</param>
        /// <returns>True if healing was successful (Entity was not at full Health)</returns>
        bool Heal(ushort amount);
        /// <summary>
        /// Damages Entity
        /// </summary>
        /// <param name="amount">Amount to Damage for</param>
        void Damage(ushort amount);
    }
}