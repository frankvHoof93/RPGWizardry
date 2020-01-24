using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Set Dialogue variables
    public Dialogue startTutorial;
    public Dialogue castedBookerang;
    public Dialogue slimesKilled;
    public Dialogue bookKilled;
    public Dialogue pickedUpPage;
    public Dialogue enteredMenu;
    public Dialogue enteredSpellList;
    public Dialogue enteredNewSpell;
    public Dialogue entersPuzzle;

    bool castBookerang = false;

    private void Start()
    {
        startTutorialDialogue();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && castBookerang == false)
        {
            castedBookerangDialogue();
            castBookerang = true;
        }
    }


    public void startTutorialDialogue()
    {
        // Find the Dialogue Manager and start dialogue with given Dialogue variable
        DialogueManager.Instance.StartDialogue(startTutorial);
    }

    public void castedBookerangDialogue()
    {
        DialogueManager.Instance.StartDialogue(castedBookerang);
    }

    public void slimesKilledDialogue()
    {
        DialogueManager.Instance.StartDialogue(slimesKilled);
    }


    public void bookKilledDialogue()
    {
        DialogueManager.Instance.StartDialogue(bookKilled);
    }
    public void pickedUpPageDialogue()
    {
        DialogueManager.Instance.StartDialogue(pickedUpPage);
    }
    public void enteredMenuDialogue()
    {
        DialogueManager.Instance.StartDialogue(enteredMenu);
    }
    public void enteredSpellListDialogue()
    {
        DialogueManager.Instance.StartDialogue(enteredSpellList);
    }
    public void enteredNewSpellDialogue()
    {
        DialogueManager.Instance.StartDialogue(enteredNewSpell);
    }
    public void entersPuzzleDialogue()
    {
        DialogueManager.Instance.StartDialogue(entersPuzzle);
    }
}
