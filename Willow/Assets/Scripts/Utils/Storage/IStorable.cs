namespace nl.SWEG.Willow.Utils.Storage
{
    /// <summary>
    /// Interface for items that can be stored on the User's device
    /// </summary>
    public interface IStorable
    {
        /// <summary>
        /// Saves Object to Disk
        /// </summary>
        /// <param name="path">(Relative) Path for Object</param>
        void Save(string path);
        /// <summary>
        /// Loads Object-Values from Disk
        /// </summary>
        /// <param name="path">(Relative) Path for Object</param>
        void Load(string path);
    }
}