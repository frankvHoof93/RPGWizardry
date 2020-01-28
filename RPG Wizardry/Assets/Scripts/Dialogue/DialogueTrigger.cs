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
        public DialogueData finishPuzzle;

        private void Start()
        {
            startTutorialDialogue(); //Run the Dialogue Queue function

            DialogueManager.Instance.toggleTextBox(false); //Turn off the Textbox used for Referencing
            SceneManager.sceneLoaded += loadMainMenu;  //Adding Event listener which checks for the scene load
            DataManager.spellunlocked += finishPuzzleDialogue; //Adding the listener for the unlockpuzzle event
            Room.clearedRoom += roomClearedSlimes; //Adding the listener to checking for the room clear

            //Adding the Event Listeneres for the Page casting and Page Pickup
            PlayerManager.Instance.CastingManager.AddCastListener(castedBookerangDialogue);
            PlayerManager.Instance.Inventory.AddPageListener(pickedUpPageDialogue);

           

        }

        /// <summary>
        ///Runs the start tutorial sentences queue
        /// </summary>
        public void startTutorialDialogue()
        {
            // Find the Dialogue Manager and start dialogue with given Dialogue variable
            DialogueManager.Instance.StartDialogue(startTutorial);
        }

        /// <summary>
        ///Runs the Bookerang casting sentences queue
        /// </summary>
        public void castedBookerangDialogue(ushort index, float cooldown)
        {
            DialogueManager.Instance.StartDialogue(castedBookerang);
            PlayerManager.Instance.CastingManager.RemoveCastListener(castedBookerangDialogue); //Removes the listener so this function isn't called again
        }

        /// <summary>
        ///Runs the sentences queue for when the slimes are cleared (Room clear)
        /// </summary>
        public void roomClearedSlimes()
        {
            DialogueManager.Instance.StartDialogue(slimeRoomCleared);
            Room.clearedRoom -= roomClearedSlimes;
            Room.clearedRoom += bookKilledDialogue; //Adds the listener, which checks for the room cleared, to the book room
        }


        /// <summary>
        ///Runs the sentences queue when the book is killed
        /// </summary>
        public void bookKilledDialogue()
        {
            DialogueManager.Instance.StartDialogue(bookKilled);
            Room.clearedRoom -= bookKilledDialogue;
        }
        public void pickedUpPageDialogue(uint newAmount, int change)
        {
            DialogueManager.Instance.StartDialogue(pickedUpPage);
            PlayerManager.Instance.MovementManager.ToggleMovement(true);
            PlayerManager.Instance.Inventory.RemovePageListener(pickedUpPageDialogue);
            GameUIManager.Instance.ToggelPause(true);
            DialogueManager.Instance.toggleTextBox(true);
        }
        public void enteredMenuDialogue()
        {
            GameUIManager.Instance.ToggelPause(false);
            DialogueManager.Instance.toggleTextBox(false);
            DialogueManager.Instance.StartDialogue(enteredMenu);
            SceneManager.sceneLoaded -= loadMainMenu;
        }
        public void enteredSpellListDialogue()
        {
            DialogueManager.Instance.StartDialogue(enteredSpellList);

            Transform spellUTF = MenuManager.Instance.GameMenu.transform.Find("Menu-Items/Spell List"); //Looking for the button Spell List in Main Menu
            Button btn = spellUTF.GetComponent<Button>();
            btn.onClick.RemoveListener(enteredSpellListDialogue);
            StartCoroutine(AttachToButtonUnlock());
        }

        public void enteredNewSpellDialogue()
        {
            DialogueManager.Instance.StartDialogue(enteredNewSpell);


            Transform buttonunlock = MenuManager.Instance.SpellListCanvas.transform.GetChild(1); //Looking for the button Spell List in Main Menu     
            Button btn = buttonunlock.GetComponent<Button>();
            btn.onClick.RemoveListener(enteredNewSpellDialogue);

            StartCoroutine(AttachToSpellCrafting());


        }
        public void entersPuzzleDialogue()
        {

            DialogueManager.Instance.StartDialogue(entersPuzzle);


            Transform spellcrafting = MenuManager.Instance.SpellCanvas.transform.Find("Spell Unlock Button"); //Looking for the button Spell List in Main Menu
            Button btn = spellcrafting.GetComponent<Button>();
            btn.onClick.RemoveListener(entersPuzzleDialogue);
        }

        public void finishPuzzleDialogue()
        {

            DialogueManager.Instance.StartDialogue(finishPuzzle);
            PlayerManager.Instance.MovementManager.ToggleMovement(false);
            DataManager.spellunlocked -= finishPuzzleDialogue;
            GameUIManager.Instance.ToggelPause(true);

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
            yield return null;

            Transform spellTF = MenuManager.Instance.GameMenu.transform.Find("Menu-Items/Spell List"); //Looking for the button Spell List in Main Menu
            Button btn = spellTF.GetComponent<Button>();
            btn.onClick.AddListener(enteredSpellListDialogue);


        }

        private IEnumerator AttachToButtonUnlock()
        {
            yield return null;

            Transform buttonunlock = MenuManager.Instance.SpellListCanvas.transform.GetChild(1); //Looking for the button Spell List in Main Menu
            Button btn = buttonunlock.GetComponent<Button>();
            btn.onClick.AddListener(enteredNewSpellDialogue);

        }

        private IEnumerator AttachToSpellCrafting()
        {
            yield return null;

            Transform spellcrafting = MenuManager.Instance.SpellCanvas.transform.Find("Spell Unlock Button"); //Looking for the button Spell List in Main Menu
            Button btn = spellcrafting.GetComponent<Button>();
            btn.onClick.AddListener(entersPuzzleDialogue);

        }
    }
}
