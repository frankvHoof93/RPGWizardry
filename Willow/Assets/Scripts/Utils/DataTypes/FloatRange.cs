namespace nl.SWEG.RPGWizardry.Utils.DataTypes
{
    [System.Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public float Random => UnityEngine.Random.Range(Min, Max);
    }
}
