using nl.SWEG.RPGWizardry.Entities.Enemies;
using UnityEngine.SceneManagement;
using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Player;
using System.Collections;
using System.Collections.Generic;
using nl.SWEG.RPGWizardry.Utils;
using UnityEngine;
using UnityEngine.UI;

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
            DialogueManager.Instance.toggleTextBox(false);

            

            Room.clearedRoom += roomClearedSlimes;
            PlayerManager.Instance.CastingManager.AddCastListener(castedBookerangDialogue);
            PlayerManager.Instance.Inventory.AddPageListener(pickedUpPageDialogue);
            SceneManager.sceneLoaded += loadMainMenu;
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
            PlayerManager.Instance.MovementManager.ToggleMovement(true);
            DialogueManager.Instance.toggleTextBox(true);
        }
        public void enteredMenuDialogue()
        {
            DialogueManager.Instance.toggleTextBox(false);
            DialogueManager.Instance.StartDialogue(enteredMenu);
            PlayerManager.Instance.MovementManager.ToggleMovement(false);
            SceneManager.sceneLoaded -= loadMainMenu;
        }
        public void enteredSpellListDialogue()
        {       
            DialogueManager.Instance.StartDialogue(enteredSpellList);

           // Transform spellUTF = MenuManager.Instance.GameMenu.transform.Find("Spell List"); //Looking for the button Spell List in Main Menu
           // Button btn = spellUTF.GetComponent<Button>();
            //btn.onClick.RemoveListener(enteredSpellListDialogue);
        }

        public void enteredNewSpellDialogue()
        {
            DialogueManager.Instance.StartDialogue(enteredNewSpell);
        }
        public void entersPuzzleDialogue()
        {
            DialogueManager.Instance.StartDialogue(entersPuzzle);
        }

        private void loadMainMenu(Scene arg0, LoadSceneMode arg1)
        {
            if(arg0.name != Constants.MainMenuSceneName)
            {
                return;
            }
            enteredMenuDialogue();

           /* Transform spellTF = MenuManager.Instance.GameMenu.transform.Find("Spell List"); //Looking for the button Spell List in Main Menu
            Button btn = spellTF.GetComponent<Button>();
            btn.onClick.AddListener(enteredSpellListDialogue);*/
        }
    }
}
