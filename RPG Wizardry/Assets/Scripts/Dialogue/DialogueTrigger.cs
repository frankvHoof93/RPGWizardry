using nl.SWEG.RPGWizardry.Entities.Enemies;
using nl.SWEG.RPGWizardry.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Set Dialogue variables
    public Dialogue startTutorial;
    public Dialogue castedBookerang;
    public Dialogue slimesKilled;
    public Dialogue pickedUpDust;
    public Dialogue bookKilled;
    public Dialogue pickedUpPage;
    public Dialogue enteredMenu;
    public Dialogue enteredSpellList;
    public Dialogue enteredNewSpell;
    public Dialogue entersPuzzle;

    private void Start()
    {
        startTutorialDialogue();

        PlayerManager.Instance.CastingManager.AddCastListener(castedBookerangDialogue);
        PlayerManager.Instance.Inventory.AddDustListener(dustPickupDialogue);

        //AEnemy.Killed += slimeKilledDialogue; 
    }

    public void startTutorialDialogue()
    {
        // Find the Dialogue Manager and start dialogue with given Dialogue variable
        DialogueManager.Instance.StartDialogue(startTutorial);
    }

    public void castedBookerangDialogue(ushort index, float cooldown)
    {
        DialogueManager.Instance.StartDialogue(castedBookerang);
        PlayerManager.Instance.CastingManager.RemoveCastListener(castedBookerangDialogue);
    }

    public void dustPickupDialogue(uint newAmount, int change)
    {

        DialogueManager.Instance.StartDialogue(pickedUpDust);
        PlayerManager.Instance.Inventory.RemoveDustListener(dustPickupDialogue);
    }
    public void slimeKilledDialogue()
    {
        DialogueManager.Instance.StartDialogue(slimesKilled);
        //AEnemy.Killed -= slimeKilledDialogue;
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
