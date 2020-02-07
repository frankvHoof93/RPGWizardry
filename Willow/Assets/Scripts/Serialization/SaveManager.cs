using nl.SWEG.Willow.Utils;
using System.IO;
using UnityEngine;

namespace nl.SWEG.Willow.Serialization
{
    /// <summary>
    /// TODO: Handles Writing and Reading of Save-Files
    /// </summary>
    public static class SaveManager
    {
        /// <summary>
        /// Whether a Save-File Exists
        /// </summary>
        /// <returns>True if a Save-File Exists</returns>
        public static bool HasSave()
        {
            return File.Exists(Application.persistentDataPath + '\\' + Constants.SaveFile);
        }
    }
}