using UnityEngine;

/// <summary>
/// Tools are Objects that are used during development
/// <para>
///     These Objects are not used in the Application itself (or only used for analysis)
/// </para>
/// </summary>
namespace nl.SWEG.Willow.Utils.Tools
{
    /// <summary>
    /// Adds notes a Game Object, in order to document things for yourself or for others
    /// </summary>
    public class Notepad : MonoBehaviour
    {
        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// <b>Editor</b>: The Author of the note(s)
        /// </summary>
        [SerializeField]
        [Tooltip("Author of the note(s)")]
        private string author;

        /// <summary>
        /// <b>Editor</b>: The notes in this Notepad
        /// <para>
        ///     Displayed using 3-15 lines (before scrollbar appears)
        /// </para>
        /// </summary>
        [TextArea(3, 15)]
        [SerializeField]
        [Tooltip("The notes in this Notepad")]
        private string notes;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Methods
        /// <summary>
        /// <b>ContextMenu</b>: Copies contents of Notes to Clipboard
        /// </summary>
        [ContextMenu("Cut Contents")]
        private void CutContents()
        {
            GUIUtility.systemCopyBuffer = notes;
            notes = string.Empty;
        }

        /// <summary>
        /// <b>ContextMenu</b>: Copies contents of Notes to Clipboard
        /// </summary>
        [ContextMenu("Copy Contents")]
        private void CopyContents()
        {
            GUIUtility.systemCopyBuffer = notes;
        }

        /// <summary>
        /// <b>ContextMenu</b>: Pastes contents of Notes from Clipboard
        /// </summary>
        [ContextMenu("Paste Contents")]
        private void PasteContents()
        {
            notes = GUIUtility.systemCopyBuffer;
        }
        #endregion
    }
}