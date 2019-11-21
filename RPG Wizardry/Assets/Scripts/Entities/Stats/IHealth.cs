namespace nl.SWEG.RPGWizardry.Entities.Stats
{
    public interface IHealth
    {
        ushort Health { get; }
        void Heal(ushort amount);
        void Damage(ushort amount);
    }
}