namespace nl.SWEG.RPGWizardry.Utils.Storage
{
    public interface IStorable
    {
        void Save(string path);

        void Load(string path);
    }
}