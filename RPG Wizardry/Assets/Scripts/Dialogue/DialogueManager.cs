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

        public Queue<string> sentences;

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
            if (dialogue.name != null)
            {
                nameText.text = dialogue.name;
            }
            else
            {
                nameText.text = "";
            }

            if (dialogue.Sprite != null)
            {
                characterImage.sprite = dialogue.Sprite;
            }
            else
            {
                characterImage.sprite = null;
            }

            // Open the dialogue box and clear the previous sentences that could be in the sentences array
            animator.SetBool("IsOpen", true);
            sentences = new Queue<string>();

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

            // Prevents sentences from displaying correctly when spamming the DisplayNextSentence key
            StopCoroutine(TypeSentence(sentence));

            // Type queued sentence
            StartCoroutine(TypeSentence(sentence));
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

        /// <summary>
        /// End dialogue and clear queue
        /// </summary>
        void EndDialogue()
        {
            // Closes dialogue box
            animator.SetBool("IsOpen", false);
            sentences.Clear();
        }
    }
}