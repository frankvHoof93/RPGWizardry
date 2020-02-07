using UnityEngine;

namespace nl.SWEG.Willow.Utils.Attributes
{
    /// <summary>
    /// Attribute used to display all Tags in the Project in the Editor for a String-Field (Instance or Array)
    /// </summary>
    public class TagSelectorAttribute : PropertyAttribute
    {
        /// <summary>
        /// Whether to use the default property drawer (or the custom drawer)
        /// </summary>
        public bool UseDefaultTagFieldDrawer = false;
    }
}