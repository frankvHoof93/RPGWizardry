using nl.SWEG.Willow.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.Willow.UI.Dialogue
{
    /// <summary>
    /// Handles Dialogue-Popups
    /// </summary>
    public class DialogueManager : SingletonBehaviour<DialogueManager>
    {
        // TODOCLEAN: Move UI-objects to seperate class & object
        // TODOCLEAN: Clean up ShowInstructionPrompt

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
        /// <summary>
        /// Animator for Dialogue-Box
        /// </summary>
        [SerializeField]
        [Tooltip("Animator for Dialogue-Box")]
        private Animator animator;
        /// <summary>
        /// Animator for Instruction-Prompt
        /// </summary>
        [SerializeField]
        [Tooltip("Animator for Instruction-Prompt")]
        private Animator animatorTextbox;
        /// <summary>
        /// Instruction-Prompt
        /// </summary>
        [SerializeField]
        [Tooltip("Instruction-Prompt")]
        private GameObject textBox;
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
        /// Begins dialogue queue
        /// </summary>
        /// <param name="dialogue">Data for Dialogue</param>
        public void StartDialogue(DialogueData dialogue)
        {
            enabled = true;
            ShowInstructionPrompt(false); //Turn off the Textbox
            nameText.text = dialogue.Name;
            characterImage.sprite = dialogue.Sprite;
            // Open the dialogue box and clear the previous sentences that could be in the sentences array
            animator.SetBool("IsOpen", true);
            sentences.Clear();
            // Queue dialogue to sentences array
            for (int i = 0; i < dialogue.Sentences.Length; i++)
                sentences.Enqueue(dialogue.Sentences[i]);
            DisplayNextSentence();
        }

        /// <summary>
        /// Get the next sentence from the queue, end dialogue if none remain
        /// </summary>
        public void DisplayNextSentence()
        {
            // Checks if there are sentences left to display
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            // Get sentence from list
            string sentence = sentences.Dequeue();
            if (currentCoroutine != null)
            {
                // Prevents sentences from displaying correctly when spamming the DisplayNextSentence key
                StopCoroutine(currentCoroutine);
            }
            // Type queued sentence
            currentCoroutine = StartCoroutine(TypeSentence(sentence));
        }

        /// <summary>
        /// Shows/Hides Instruction Prompt
        /// </summary>
        /// <param name="value">Show/Hide</param>
        public void ShowInstructionPrompt(bool value)
        {
            textBox.SetActive(value);
            animatorTextbox.SetBool("isOpened", value);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Checks if right mouse button is pressed to display the next sentence in the dialogue
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                DisplayNextSentence();
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
            // Makes an array of each character in sentence and adds each letter to the text 
            foreach (char letter in sentence)
            {
                yield return null;
                dialogueText.text += letter;
            }
        }

        /// <summary>
        /// Ends dialogue and clears queue
        /// </summary>
        private void EndDialogue()
        {
            // Closes dialogue box
            animator.SetBool("IsOpen", false);
            sentences.Clear();
            enabled = false;
        }
        #endregion
        #endregion
    }
}