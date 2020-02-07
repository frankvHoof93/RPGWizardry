using nl.SWEG.Willow.Utils.Behaviours;
using nl.SWEG.Willow.Utils.Functions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace nl.SWEG.Willow.UI.Dialogue
{
    /// <summary>
    /// Handles Dialogue-Popups
    /// </summary>
    public class DialogueManager : SingletonBehaviour<DialogueManager>
    {
        #region Variables
        #region Editor
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// UI-Object for Dialogue
        /// </summary>
        [SerializeField]
        [Tooltip("UI-Object for Dialogue")]
        private DialogueBox dialogueUI;
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
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Private
        /// <summary>
        /// Text-Object in InstructionPrompt
        /// </summary>
        private TextMeshProUGUI promptText;
        /// <summary>
        /// Action performed when Dialogue is completed
        /// </summary>
        private UnityAction onComplete;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>
        /// Begins dialogue queue
        /// </summary>
        /// <param name="dialogue">Data for Dialogue</param>
        /// <param name="performOnComplete">Action to perform on completion of Dialogue</param>
        public void StartDialogue(DialogueData dialogue, UnityAction performOnComplete = null)
        {
            enabled = true;
            onComplete = performOnComplete;
            HideInstructionPrompt(); //Turn off the TextBox
            dialogueUI.StartDialogue(dialogue);
            animator.SetBool("IsOpen", true);
        }

        /// <summary>
        /// Shows Instruction Prompt
        /// </summary>
        /// <param name="textToDisplay">Instruction-Text to display</param>
        public void ShowInstructionPrompt(string textToDisplay)
        {
            textBox.SetActive(true);
            promptText.text = textToDisplay;
            animatorTextbox.SetBool("isOpened", true);
        }

        /// <summary>
        /// Hides Instruction Prompt
        /// </summary>
        public void HideInstructionPrompt()
        {
            textBox.SetActive(false);
            animatorTextbox.SetBool("isOpened", false);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Finds reference to InstructionPrompt-Text
        /// </summary>
        private void Start()
        {
            promptText = textBox.GetComponentInChildren<TextMeshProUGUI>(true);
        }

        /// <summary>
        /// Checks if right mouse button is pressed to display the next sentence in the dialogue
        /// </summary>
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
                if (dialogueUI.DisplayNextSentence())
                    EndDialogue();
        }
        #endregion

        #region Private

        /// <summary>
        /// Ends dialogue and clears queue
        /// </summary>
        private void EndDialogue()
        {
            // Closes dialogue box
            animator.SetBool("IsOpen", false);
            onComplete?.Invoke();
            onComplete = null;
            StartCoroutine(CoroutineMethods.RunDelayed(() => dialogueUI.ClearDialogue(), .25f));
            enabled = false;
        }
        #endregion
        #endregion
    }
}