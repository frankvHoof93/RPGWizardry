using UnityEngine;

namespace nl.SWEG.RPGWizardry.Utils.Attributes
{
    public class TagSelectorAttribute : PropertyAttribute
    {
        /// <summary>
        /// Whether to use the default property drawer (or the custom drawer)
        /// </summary>
        public bool UseDefaultTagFieldDrawer = false;
    }
}