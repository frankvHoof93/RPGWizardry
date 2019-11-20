namespace nl.SWEG.RPGWizardry.Utils.Storage
{
    public interface IJSON<T> where T : class
    {
        string ToJSON();
        T FromJSON(string json);
    }
}