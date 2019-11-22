namespace nl.SWEG.RPGWizardry.Entities.Stats
{
    public delegate void OnHealthChange(ushort newHealth, ushort maxHealth, short change);

    public interface IHealth
    {
        ushort Health { get; }
        bool Heal(ushort amount);
        void Damage(ushort amount);
    }
}