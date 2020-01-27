using nl.SWEG.RPGWizardry.Entities.Enemies;
using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.UI.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        // Set Dialogue variables
        public DialogueData startTutorial;
        public DialogueData castedBookerang;
        public DialogueData slimeRoomCleared;
        public DialogueData bookKilled;
        public DialogueData pickedUpPage;
        public DialogueData enteredMenu;
        public DialogueData enteredSpellList;
        public DialogueData enteredNewSpell;
        public DialogueData entersPuzzle;



        private void Start()
        {
            startTutorialDialogue();

            Room.clearedRoom += roomClearedSlimes;
            PlayerManager.Instance.CastingManager.AddCastListener(castedBookerangDialogue);
            PlayerManager.Instance.Inventory.AddPageListener(pickedUpPageDialogue);
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

        public void roomClearedSlimes()
        {
            DialogueManager.Instance.StartDialogue(slimeRoomCleared);
            Room.clearedRoom -= roomClearedSlimes;
            Room.clearedRoom += bookKilledDialogue;
        }


        public void bookKilledDialogue()
        {
            DialogueManager.Instance.StartDialogue(bookKilled);
            Room.clearedRoom -= bookKilledDialogue;
        }
        public void pickedUpPageDialogue(uint newAmount, int change)
        {
            DialogueManager.Instance.StartDialogue(pickedUpPage);
            PlayerManager.Instance.Inventory.RemovePageListener(pickedUpPageDialogue);
            PlayerManager.Instance.MovementManager.ToggleMovement(true);
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
}
