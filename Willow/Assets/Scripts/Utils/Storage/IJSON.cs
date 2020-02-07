namespace nl.SWEG.Willow.Utils.Storage
{
    /// <summary>
    /// Interface for Objects that can be converted to/from JSON
    /// </summary>
    /// <typeparam name="T">Type for Object (for return-value)</typeparam>
    public interface IJSON<T> where T : class
    {
        /// <summary>
        /// Converts Object to JSON
        /// </summary>
        /// <returns>JSON-Representation for Object</returns>
        string ToJSON();
        /// <summary>
        /// Converts JSON to Object
        /// </summary>
        /// <param name="json">JSON-Representation for Object</param>
        /// <returns>Deserialized Object</returns>
        T FromJSON(string json);
    }
}