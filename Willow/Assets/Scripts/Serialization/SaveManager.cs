using nl.SWEG.RPGWizardry.Utils;
using System.IO;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Serialization
{
    public static class SaveManager
    {
        public static bool HasSave()
        {
            return File.Exists(Application.persistentDataPath + '\\' + Constants.SaveFile);
        }
    }
}
