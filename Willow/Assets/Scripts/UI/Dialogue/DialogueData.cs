﻿using UnityEngine;

namespace nl.SWEG.Willow.UI.Dialogue
{
    /// <summary>
    /// Data for a single piece of Dialogue
    /// </summary>
    [CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/Dialogue", order = 1)]
    public class DialogueData : ScriptableObject
    {
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Name of the character speaking the dialogue
        /// </summary>
        public string Name => characterName;
        [SerializeField]
        [Tooltip("Name of the character speaking the dialogue")]
        private string characterName;

        /// <summary>
        /// Sprite of the character speaking the dialogue
        /// <para>
        /// Leave empty to show text without portrait
        /// </para>
        /// </summary>
        public Sprite Sprite => characterSprite;
        [SerializeField]
        [Tooltip("Portrait of the character. Leave empty to just show text")]
        private Sprite characterSprite;

        /// <summary>
        /// List of dialogue string
        /// </summary>
        public string[] Sentences => sentenceStrings;
        [SerializeField]
        [Tooltip("Array of dialogue strings, in order displayed")]
        [TextArea(3, 10)]
        private string[] sentenceStrings;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
    }
}