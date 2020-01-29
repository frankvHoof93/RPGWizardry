using nl.SWEG.RPGWizardry.Entities.Enemies;
using UnityEngine.SceneManagement;
using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Player;
using System.Collections;
using nl.SWEG.RPGWizardry.UI.GameUI;
using System.Collections.Generic;
using nl.SWEG.RPGWizardry.Utils;
using nl.SWEG.RPGWizardry.ResearchData;
using UnityEngine;
using UnityEngine.UI;
using nl.SWEG.RPGWizardry.Player.Inventory;

namespace nl.SWEG.RPGWizardry.UI.Dialogue
{
    

    public class DialogueTrigger : MonoBehaviour
    {
        private enum DialoguePrompts
        {
            start = 0,
            castedBookerang = 1,
            slimeRoomCleared = 2,
            bookKilled = 3,
            pickedUpPage = 4,
            enteredMenu = 5,
            enteredSpellList = 6,
            enteredNewSpell = 7,
            entersPuzzle = 8,
            finishPuzzle = 9
        }

        // Set Dialogue variables
        [SerializeField]
        private DialogueData[] dialogues;

        private void Start()
        {
            startTutorialDialogue(); //Run the Dialogue Queue function
            
            SceneManager.sceneLoaded += loadMainMenu;  //Adding Event listener which checks for the scene load
            PlayerManager.Instance.Inventory.spellunlocked += finishPuzzleDialogue; //Adding the listener for the unlockpuzzle event
            Room.clearedRoom += roomClearedSlimes; //Adding the listener to checking for the room clear

            //Adding the Event Listeneres for the Page casting and Page Pickup
            PlayerManager.Instance.CastingManager.AddCastListener(castedBookerangDialogue);
            PlayerManager.Instance.Inventory.AddPageListener(pickedUpPageDialogue);
        }

        /// <summary>
        ///Runs the start tutorial sentences queue
        /// </summary>
        private void startTutorialDialogue()
        {
            // Find the Dialogue Manager and start dialogue with given Dialogue variable
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.start]);
        }

        /// <summary>
        ///Runs the Bookerang casting sentences queue
        /// </summary>
        public void castedBookerangDialogue(ushort index, float cooldown)
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.castedBookerang]);
            PlayerManager.Instance.CastingManager.RemoveCastListener(castedBookerangDialogue); //Removes the listener so this function isn't called again
        }

        /// <summary>
        ///Runs the sentences queue for when the slimes are cleared (Room clear)
        /// </summary>
        public void roomClearedSlimes()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.slimeRoomCleared]);
            Room.clearedRoom -= roomClearedSlimes;
            Room.clearedRoom += bookKilledDialogue; //Adds the listener, which checks for the room cleared, to the book room
        }


        /// <summary>
        ///Runs the sentences queue when the book is killed
        /// </summary>
        public void bookKilledDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.bookKilled]);
            Room.clearedRoom -= bookKilledDialogue;
        }
        public void pickedUpPageDialogue(uint newAmount, int change)
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.pickedUpPage]);
            PlayerManager.Instance.MovementManager.SetStunned(true);
            PlayerManager.Instance.Inventory.RemovePageListener(pickedUpPageDialogue);
            //enable GameUIManager to allow pausing
            GameUIManager.Instance.enabled = true;
            DialogueManager.Instance.ShowInstructionPrompt(true);
        }
        public void enteredMenuDialogue()
        {
            //disable GameUIManager to disallow pausing
            GameUIManager.Instance.enabled = false;
            DialogueManager.Instance.ShowInstructionPrompt(false);
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.enteredMenu]);
            SceneManager.sceneLoaded -= loadMainMenu;
        }
        public void enteredSpellListDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.enteredSpellList]);

            Transform spellUTF = MenuManager.Instance.GameMenu.transform.Find("Menu-Items/Spell List"); //Looking for the button Spell List in Main Menu
            Button btn = spellUTF.GetComponent<Button>();
            btn.onClick.RemoveListener(enteredSpellListDialogue);
            StartCoroutine(AttachToButtonUnlock());
        }

        public void enteredNewSpellDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.enteredNewSpell]);


            Transform buttonunlock = MenuManager.Instance.SpellListCanvas.transform.GetChild(1); //Looking for the second spell in the spell list    
            Button btn = buttonunlock.GetComponent<Button>();
            btn.onClick.RemoveListener(enteredNewSpellDialogue);

            StartCoroutine(AttachToSpellCrafting());


        }
        public void entersPuzzleDialogue()
        {

            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.entersPuzzle]);


            Transform spellcrafting = MenuManager.Instance.SpellCanvas.transform.Find("Spell Unlock Button"); //Looking for the button Spell List in Main Menu
            Button btn = spellcrafting.GetComponent<Button>();
            btn.onClick.RemoveListener(entersPuzzleDialogue);
        }

        public void finishPuzzleDialogue()
        {

            DialogueManager.Instance.StartDialogue(dialogues[(int)DialoguePrompts.finishPuzzle]);
            PlayerManager.Instance.MovementManager.SetStunned(false);
            PlayerManager.Instance.Inventory.spellunlocked -= finishPuzzleDialogue;
            //enable GameUIManager to allow pausing
            GameUIManager.Instance.enabled = true;

        }

        private void loadMainMenu(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name != Constants.MainMenuSceneName)
            {
                return;
            }
            enteredMenuDialogue();
            StartCoroutine(AttachToButtonSpell());
        }

        private IEnumerator AttachToButtonSpell()
        {
            // Wait until there's a MenuManager (race condition)
            yield return new WaitUntil(() => MenuManager.Exists);

            Transform spellTF = MenuManager.Instance.GameMenu.transform.Find("Menu-Items/Spell List"); //Looking for the button Spell List in Main Menu
            Button btn = spellTF.GetComponent<Button>();
            btn.onClick.AddListener(enteredSpellListDialogue);


        }

        private IEnumerator AttachToButtonUnlock()
        {
            // Wait until there's a MenuManager (race condition)
            yield return new WaitUntil(() => MenuManager.Exists);

            Transform buttonunlock = MenuManager.Instance.SpellListCanvas.transform.GetChild(1); //Looking for the button Spell List in Main Menu
            Button btn = buttonunlock.GetComponent<Button>();
            btn.onClick.AddListener(enteredNewSpellDialogue);

        }

        private IEnumerator AttachToSpellCrafting()
        {
            // Wait until there's a MenuManager (race condition)
            yield return new WaitUntil(() => MenuManager.Exists);

            Transform spellcrafting = MenuManager.Instance.SpellCanvas.transform.Find("Spell Unlock Button"); //Looking for the button Spell List in Main Menu
            Button btn = spellcrafting.GetComponent<Button>();
            btn.onClick.AddListener(entersPuzzleDialogue);

        }
    }
}
