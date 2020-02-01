namespace nl.SWEG.Willow.Sorcery
{
    /// <summary>
    /// Pattern in which Projectiles can be created for Spells
    /// </summary>
    public enum SpellPattern : byte
    {
        /// <summary>
        /// Straight Line
        /// </summary>
        Line = 0,
        /// <summary>
        /// Cone-Shape
        /// </summary>
        Cone = 1,
        /// <summary>
        /// Circle around Position
        /// </summary>
        Circle = 2
    }
}