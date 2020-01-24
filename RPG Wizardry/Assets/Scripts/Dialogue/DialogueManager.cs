using nl.SWEG.RPGWizardry.Utils.Behaviours;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SingletonBehaviour<DialogueManager>
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image characterImage;
    public Animator animator;

    public Queue<string> sentences;


    private void Update()
    {
        // Check if key is pressed to display the next sentence in the dialogue
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
       
        if (dialogue.name != null)
        {
            nameText.text = dialogue.name;
        }
        else
        {
            nameText.text = "";
        }

        if(dialogue.characterSprite != null)
        {
            characterImage.sprite = dialogue.characterSprite;
        }
        else
        {
            characterImage.sprite = null;
        }
        // Open the dialogue box and clear the previous sentences that could be in the sentences array
        animator.SetBool("IsOpen", true);
        sentences = new Queue<string>();
        


        // Queue dialogue to sentences array
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        // Checks if there are sentences left to display
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Remove sentence from list
        string sentence = sentences.Dequeue();

        // Prevents sentences from displaying correctly when spamming the DisplayNextSentence key
        StopAllCoroutines();

        // Type queued sentence
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        // Reset the dialogue box text to blank
        dialogueText.text = "";

        // Makes an array of each character in sentence and adds each letter to the text 
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        // Closes dialogue box
        animator.SetBool("IsOpen", false);
        sentences.Clear();
    }
}
