namespace nl.SWEG.Willow.Utils.DataTypes
{
    /// <summary>
    /// Range-Value
    /// </summary>
    [System.Serializable]
    public struct FloatRange
    {
        /// <summary>
        /// Minimum value for Range
        /// </summary>
        public float Min;
        /// <summary>
        /// Maximum value for Range
        /// </summary>
        public float Max;
        /// <summary>
        /// Random value in Range (inclusive bounds)
        /// </summary>
        public float Random => UnityEngine.Random.Range(Min, Max);
    }
}
