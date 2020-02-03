using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.GameWorld.Levels.Rooms;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Sorcery;
using nl.SWEG.Willow.UI.Dialogue;
using nl.SWEG.Willow.UI.Game;
using nl.SWEG.Willow.UI.Menu;
using nl.SWEG.Willow.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace nl.SWEG.Willow.Tutorial
{
    /// <summary>
    /// Handles Tutorial-Messages
    /// </summary>
    public class TutorialManager : MonoBehaviour
    {
        // TODOCLEAN: Check when delegates are added/removed
        // TODOCLEAN: Could the Menu-Part of the SpellCrafting-Tutorial be turned into a (manully run) Coroutine? (IEnumerator.Next())
        #region InnerTypes
        /// <summary>
        /// Steps in Tutorial
        /// </summary>
        private enum TutorialSteps
        {
            Start = 0,
            CastedBookerang = 1,
            SlimeRoomCleared = 2,
            BookKilled = 3,
            PickedUpPage = 4,
            EnteredMenu = 5,
            EnteredSpellList = 6,
            EnteredNewSpell = 7,
            EntersPuzzle = 8,
            FinishPuzzle = 9
        }
        #endregion

        #region Variables
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Data for Dialogues attached to Tutorial-Steps
        /// </summary>
        [SerializeField]
        [Tooltip("Data for Dialogues attached to Tutorial-Steps")]
        private DialogueData[] dialogues;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Sets initial listeners
        /// </summary>
        private void Start()
        {
            StartTutorialDialogue(); //Run the Dialogue Queue function            
            SceneManager.sceneLoaded += LoadMainMenu;  //Adding Event listener which checks for the scene load
            PlayerManager.Instance.Inventory.AddUnlockListener(FinishPuzzleDialogue); //Adding the listener for the unlockpuzzle event
            Room.clearedRoom += RoomClearedSlimes; //Adding the listener to checking for the room clear
            //Adding the Event Listeneres for the Page casting and Page Pickup
            PlayerManager.Instance.CastingManager.AddCastListener(CastedBookerangDialogue);
            PlayerManager.Instance.Inventory.AddPageListener(PickedUpPageDialogue);
        }
        #endregion

        #region Private
        /// <summary>
        /// Runs the Bookerang casting sentences queue
        /// </summary>
        private void CastedBookerangDialogue(ushort index, float cooldown)
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.CastedBookerang]);
            PlayerManager.Instance.CastingManager.RemoveCastListener(CastedBookerangDialogue); //Removes the listener so this function isn't called again
        }

        /// <summary> 
        /// Runs the sentences queue for when the slimes are cleared (Room clear)
        /// </summary>
        private void RoomClearedSlimes()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.SlimeRoomCleared]);
            Room.clearedRoom -= RoomClearedSlimes;
            Room.clearedRoom += BookKilledDialogue; //Adds the listener, which checks for the room cleared, to the book room
        }

        /// <summary>
        /// Runs the sentences queue when the book is killed
        /// </summary>
        private void BookKilledDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.BookKilled]);
            Room.clearedRoom -= BookKilledDialogue;
        }

        /// <summary>
        /// Runs Dialogue after picking up Page
        /// </summary>
        /// <param name="page">Page that was picked up</param>
        private void PickedUpPageDialogue(SpellPage page)
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.PickedUpPage]);
            if (!GameManager.Instance.Paused)
                GameManager.Instance.TogglePause();
            PlayerManager.Instance.Inventory.RemovePageListener(PickedUpPageDialogue);
            //enable GameUIManager to allow pausing
            GameUIManager.Instance.enabled = true;
            DialogueManager.Instance.ShowInstructionPrompt("Please press \"ESC\" to open Greg");
        }

        /// <summary>
        /// Runs Dialogue after entering PauseMenu
        /// </summary>
        private void EnteredMenuDialogue()
        {
            //disable GameUIManager to disallow pausing
            GameUIManager.Instance.enabled = false;
            DialogueManager.Instance.HideInstructionPrompt();
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EnteredMenu]);
            SceneManager.sceneLoaded -= LoadMainMenu;
        }

        /// <summary>
        /// Runs Dialogue after Entering SpellList
        /// </summary>
        private void EnteredSpellListDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EnteredSpellList]);

            Transform spellTransform = MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Spell List");
            Button btn = spellTransform.GetComponent<Button>();
            btn.onClick.RemoveListener(EnteredSpellListDialogue);
            StartCoroutine(AttachToButtonUnlock());
        }

        /// <summary>
        /// Runs Dialogue for ...
        /// </summary>
        private void EnteredNewSpellDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EnteredNewSpell]);
            Transform buttonunlock = MenuManager.Instance.SpellListCanvas.Find("Book/Left Page").GetChild(1);  // Spell Tab for new Spell
            Button btn = buttonunlock.GetComponent<Button>();
            btn.onClick.RemoveListener(EnteredNewSpellDialogue);
            StartCoroutine(AttachToSpellCrafting());
        }

        /// <summary>
        /// Runs Dialogue for Entering Puzzle
        /// </summary>
        private void EntersPuzzleDialogue()
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EntersPuzzle]);
            Transform spellcrafting = MenuManager.Instance.SpellCanvas.transform.Find("Book/Page Right/Spell Unlock Button"); // Unlock Button for Spell
            Button btn = spellcrafting.GetComponent<Button>();
            btn.onClick.RemoveListener(EntersPuzzleDialogue);
        }

        /// <summary>
        /// Runs Dialogue after finishing Puzzle
        /// </summary>
        private void FinishPuzzleDialogue(SpellPage spellPage)
        {
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.FinishPuzzle]);
            PlayerManager.Instance.Inventory.RemoveUnlockListener(FinishPuzzleDialogue);
            //enable GameUIManager to allow pausing
            GameUIManager.Instance.enabled = true;
        }

        /// <summary>
        /// Runs the StartTutorial Dialogue
        /// </summary>
        private void StartTutorialDialogue()
        {
            // Find the Dialogue Manager and start dialogue with given Dialogue variable
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.Start]);
        }

        /// <summary>
        /// Attaches delegate after Menu-Scene loads
        /// </summary>
        /// <param name="loadedScene">Scene that was loaded</param>
        /// <param name="loadMode">Mode in which Scene was loaded</param>
        private void LoadMainMenu(Scene loadedScene, LoadSceneMode loadMode)
        {
            if (loadedScene.name != Constants.MainMenuSceneName)
                return;
            EnteredMenuDialogue();
            StartCoroutine(AttachToButtonSpell());
        }

        /// <summary>
        /// Attaches delegate to Spell-Button
        /// </summary>
        private IEnumerator AttachToButtonSpell()
        {
            // Wait until there's a MenuManager (race condition)
            yield return new WaitUntil(() => MenuManager.Exists);

            Transform spellTransform = MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Spell List");
            Button btn = spellTransform.GetComponent<Button>();
            btn.onClick.AddListener(EnteredSpellListDialogue);
        }

        /// <summary>
        /// Attaches delegate to Unlock
        /// </summary>
        private IEnumerator AttachToButtonUnlock()
        {
            // Wait until there's a MenuManager (race condition)
            yield return new WaitUntil(() => MenuManager.Exists);
            Transform buttonunlock = MenuManager.Instance.SpellListCanvas.Find("Book/Left Page").GetChild(1); // Spell Tab for new Spell
            Button btn = buttonunlock.GetComponent<Button>();
            btn.onClick.AddListener(EnteredNewSpellDialogue);
        }

        /// <summary>
        /// Attaches delegate to SpellCrafting-Menu
        /// </summary>
        private IEnumerator AttachToSpellCrafting()
        {
            // Wait until there's a MenuManager (race condition)
            yield return new WaitUntil(() => MenuManager.Exists);
            Transform spellcrafting = MenuManager.Instance.SpellCanvas.Find("Book/Page Right/Spell Unlock Button"); // Unlock Button for Spell
            Button btn = spellcrafting.GetComponent<Button>();
            btn.onClick.AddListener(EntersPuzzleDialogue);
        }
        #endregion
        #endregion
    }
}