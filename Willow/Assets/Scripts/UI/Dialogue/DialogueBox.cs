using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.Willow.UI.Dialogue
{
    /// <summary>
    /// UI-Object that displays Dialogue
    /// </summary>
    public class DialogueBox : MonoBehaviour
    {
        #region Variables
        #region Editor
        /// <summary>
        /// Text-Object for Character-Name
        /// </summary>
        [SerializeField]
        [Tooltip("Text-Object for Character-Name")]
        private TextMeshProUGUI nameText;
        /// <summary>
        /// Text-Object for Dialogue-Strings
        /// </summary>
        [SerializeField]
        [Tooltip("Text-Object for Dialogue-Strings")]
        private TextMeshProUGUI dialogueText;
        /// <summary>
        /// Image for Character
        /// </summary>
        [SerializeField]
        [Tooltip("Image for Character")]
        private Image characterImage;
        #endregion

        #region Private
        /// <summary>
        /// Sentences in current Dialogue
        /// </summary>
        private readonly Queue<string> sentences = new Queue<string>();
        /// <summary>
        /// Coroutine for Sentence being typed
        /// </summary>
        private Coroutine currentCoroutine;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Starts new Dialogue, displaying the first sentence
        /// </summary>
        /// <param name="data">Data for Dialogue</param>
        public void StartDialogue(DialogueData data)
        {
            nameText.text = data.Name;
            characterImage.sprite = data.Sprite;
            sentences.Clear();
            // Queue dialogue to sentences array
            for (int i = 0; i < data.Sentences.Length; i++)
                sentences.Enqueue(data.Sentences[i]);
            DisplayNextSentence();
        }

        /// <summary>
        /// Displays next sentence in Dialogue
        /// </summary>
        /// <returns>True if dialogue has been finished</returns>
        public bool DisplayNextSentence()
        {
            // Checks if there are sentences left to display
            if (sentences.Count == 0)
                return true;
            // Get sentence from list
            string sentence = sentences.Dequeue();
            if (currentCoroutine != null)
            {
                // Prevents sentences from displaying correctly when spamming the DisplayNextSentence key
                StopCoroutine(currentCoroutine);
            }
            // Type queued sentence
            currentCoroutine = StartCoroutine(TypeSentence(sentence));
            return false;
        }

        /// <summary>
        /// Clears currently displayed Dialogue
        /// </summary>
        public void ClearDialogue()
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            sentences.Clear();
            dialogueText.text = string.Empty;
            nameText.text = string.Empty;
            characterImage.sprite = null;
        }
        #endregion

        #region Private
        /// <summary>
        /// Display string one character at a time
        /// </summary>
        /// <param name="sentence">String to display</param>
        private IEnumerator TypeSentence(string sentence)
        {
            // Reset the dialogue box text to blank
            dialogueText.text = string.Empty;
            // Add one letter to the text each frame
            foreach (char letter in sentence)
            {
                yield return null;
                dialogueText.text += letter;
            }
        }
        #endregion
        #endregion
    }
}