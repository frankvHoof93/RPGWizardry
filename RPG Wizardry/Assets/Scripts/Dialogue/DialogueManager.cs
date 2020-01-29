using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace nl.SWEG.RPGWizardry.UI.Dialogue
{
    public class DialogueManager : SingletonBehaviour<DialogueManager>
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public Image characterImage;
        public Animator animator;
        public Animator animatorTextbox;

        [SerializeField] public GameObject textBox;

        private readonly Queue<string> sentences = new Queue<string>();

        private Coroutine currentCoroutine;
        
        /// <summary>
        /// Check if right mouse button is pressed
        /// </summary>
        private void Update()
        {
            // Check if key is pressed to display the next sentence in the dialogue
            if (Input.GetMouseButtonDown(1))
            {
                DisplayNextSentence();
            }
        }

        /// <summary>
        /// Begins dialogue queue
        /// </summary>
        /// <param name="dialogue">Scriptable dialogue object</param>
        public void StartDialogue(DialogueData dialogue)
        {
            SetTextboxVisibility(false); //Turn off the Textbox

            if (dialogue.Name != null)
            {
                nameText.text = dialogue.Name;
            }
            else
            {
                nameText.text = "";
            }

            characterImage.sprite = dialogue.Sprite;

            // Open the dialogue box and clear the previous sentences that could be in the sentences array
            animator.SetBool("IsOpen", true);
            sentences.Clear();

            // Queue dialogue to sentences array
            foreach (string sentence in dialogue.Sentences)
            {
                sentences.Enqueue(sentence);
            }

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
        /// Display string one character at a time
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        IEnumerator TypeSentence(string sentence)
        {
            // Reset the dialogue box text to blank
            dialogueText.text = "";

            // Makes an array of each character in sentence and adds each letter to the text 
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return null;
            }
        }

        public void SetTextboxVisibility(bool value)
        {
            textBox.SetActive(value);
            animatorTextbox.SetBool("isOpened", value);
        }

        /// <summary>
        /// End dialogue and clear queue
        /// </summary>
        private void EndDialogue()
        {
            // Closes dialogue box
            animator.SetBool("IsOpen", false);
            sentences.Clear();
        }
    }
}