using nl.SWEG.Willow.Entities.Enemies;
using nl.SWEG.Willow.GameWorld;
using nl.SWEG.Willow.GameWorld.Levels;
using nl.SWEG.Willow.GameWorld.Levels.Rooms;
using nl.SWEG.Willow.Player;
using nl.SWEG.Willow.Sorcery;
using nl.SWEG.Willow.UI.Dialogue;
using nl.SWEG.Willow.UI.Game;
using nl.SWEG.Willow.UI.Menu;
using nl.SWEG.Willow.Utils;
using nl.SWEG.Willow.Utils.Functions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        #region InnerTypes
        /// <summary>
        /// Steps in Tutorial
        /// </summary>
        private enum TutorialSteps
        {
            /// <summary>
            /// Start of Game
            /// </summary>
            GameStart = 0,
            /// <summary>
            /// Bookerang-Spell casted for the first time
            /// </summary>
            FirstCastBookerang = 1,
            /// <summary>
            /// First Kill of a Slime
            /// </summary>
            KilledFirstSlime = 2,
            /// <summary>
            /// First Kill of a Book
            /// </summary>
            KilledFirstBook = 3,
            /// <summary>
            /// Pickup of first Spell-Page
            /// </summary>
            PagePickup = 4,
            /// <summary>
            /// Entered Pause-Menu
            /// </summary>
            EnteredPauseMenu = 5,
            /// <summary>
            /// Entered Spell-List in Pause-Menu
            /// </summary>
            EnteredSpellList = 6,
            /// <summary>
            /// Entered Spell-Details in Pause-Menu
            /// </summary>
            EnteredSpellDetails = 7,
            /// <summary>
            /// Entered MiniGame for Spell
            /// </summary>
            EnteredSpellPuzzle = 8,
            /// <summary>
            /// Finished MiniGame for Spell
            /// </summary>
            FinishedSpellPuzzle = 9
        }
        #endregion

        #region Variables
        #region Constants
        /// <summary>
        /// Name of First Room
        /// </summary>
        private const string StartRoomName =        "StartRoom";
        /// <summary>
        /// Name of Room with first SlimeEnemy
        /// </summary>
        private const string FirstSlimeRoomName =   "FirstSlimeRoom";
        /// <summary>
        /// Name of Room with first BookEnemy
        /// </summary>
        private const string FirstBookRoomName =    "FirstBookRoom";
        /// <summary>
        /// Name of Room with Big Slime
        /// </summary>
        private const string BigSlimeRoomName =     "BigSlimeRoom";
        /// <summary>
        /// Name of Final Room in Tutorial
        /// </summary>
        private const string FinalRoomName =        "FinalRoom";
        #endregion

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
        /// Sets Listener for first Room-Load
        /// </summary>
        private void Start()
        {
            InitGameStartDialogue();
        }
        #endregion

        #region Private
        #region GameStart
        /// <summary>
        /// Initializes Listeners for GameStartDialogue
        /// </summary>
        private void InitGameStartDialogue()
        {
            // Attach Listener to RoomLoad
            FloorManager.Instance.AddRoomLoadListener(RunGameStartDialogue);
        }
        /// <summary>
        /// Runs GameStartDialogue for first Room
        /// </summary>
        /// <param name="loadedRoom">Room that was Loaded</param>
        private void RunGameStartDialogue(Room loadedRoom)
        {
            if (loadedRoom.name != StartRoomName)
                return;
            // Remove Listener from RoomLoad
            FloorManager.Instance.RemoveRoomLoadListener(RunGameStartDialogue);
            // Lock Movement & Casting
            PlayerManager.Instance.MovementManager.enabled = false;
            PlayerManager.Instance.CastingManager.enabled = false;
            // Lock Current Room
            FloorManager.Instance.CurrentRoom.CloseDoors();
            // Display dialogue after 1 second
            StartCoroutine(CoroutineMethods.RunDelayed(() =>
            {
                // Prevent opening Menu during Dialogue
                GameUIManager.Instance.enabled = false;
                // Run Dialogue
                DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.GameStart],
                    EndGameStartDialogue);
            }
            , 1f));
        }
        /// <summary>
        /// Disposes GameStartDialogue, initializes next step in Tutorial
        /// </summary>
        private void EndGameStartDialogue()
        {
            // Unlock Casting
            PlayerManager.Instance.CastingManager.enabled = true;
            // Unlock Menu
            GameUIManager.Instance.enabled = true;
            // Show Instruction Prompt
            DialogueManager.Instance.ShowInstructionPrompt("Left Click to Cast a Spell");
            // Initialize next step
            InitFirstCastDialogue();
        }
        #endregion

        #region FirstCastBookerang
        /// <summary>
        /// Initializes Listeners for FirstCastDialogue
        /// </summary>
        private void InitFirstCastDialogue()
        {
            // Attach Listener to Casting
            PlayerManager.Instance.CastingManager.AddCastListener(RunFirstCastDialogue);
        }
        /// <summary>
        /// Runs FirstCastDialogue
        /// </summary>
        /// <param name="index">Index in EquippedSpells for Cast</param>
        /// <param name="cooldown">Cooldown for cast Spell</param>
        private void RunFirstCastDialogue(ushort index, float cooldown)
        {
            // Hide Instruction Prompt
            DialogueManager.Instance.HideInstructionPrompt();
            // Remove Listener from Casting
            PlayerManager.Instance.CastingManager.RemoveCastListener(RunFirstCastDialogue);
            // Prevent opening Menu during Dialogue
            GameUIManager.Instance.enabled = false;
            // Display Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.FirstCastBookerang], 
                EndFirstCastDialogue);
        }
        /// <summary>
        /// Disposes FirstCastDialogue, initializes next step in Tutorial
        /// </summary>
        private void EndFirstCastDialogue()
        {
            // Unlock Movement
            PlayerManager.Instance.MovementManager.enabled = true;
            // Unlock Current Room
            FloorManager.Instance.CurrentRoom.OpenDoors();
            // Unlock Menu
            GameUIManager.Instance.enabled = true;
            // Initialize next step
            FloorManager.Instance.AddRoomLoadListener(InitFirstSlimeKillDialogue);
        }
        #endregion

        #region KilledFirstSlime
        /// <summary>
        /// Initializes Listeners for FirstSlimeKillDialogue
        /// </summary>
        /// <param name="loadedRoom">Room that was Loaded</param>
        private void InitFirstSlimeKillDialogue(Room loadedRoom)
        {
            if (loadedRoom.name != FirstSlimeRoomName)
                return;
            // Remove RoomLoad-Listener
            FloorManager.Instance.RemoveRoomLoadListener(InitFirstSlimeKillDialogue);
            // Add Listener to Enemy-Death
            IReadOnlyList<AEnemy> enemies = FloorManager.Instance.CurrentRoom.Enemies;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].AddDeathListener(RunFirstSlimeKillDialogue);
        }
        /// <summary>
        /// Runs FirstSlimeKillDialogue
        /// </summary>
        /// <param name="deadSlime">Slime that was killed</param>
        private void RunFirstSlimeKillDialogue(GameObject deadSlime)
        {
            // Remove Listeners from remaining Enemies
            IReadOnlyList<AEnemy> enemies = FloorManager.Instance.CurrentRoom.Enemies;
            for (int i = 0; i < enemies.Count; i++)
                enemies[i].RemoveDeathListener(RunFirstSlimeKillDialogue);
            // Pause GamePlay
            GameManager.Instance.PauseGame();
            // Prevent opening Menu during Dialogue
            GameUIManager.Instance.enabled = false;
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.KilledFirstSlime],
                EndFirstSlimeKillDialogue);
        }
        /// <summary>
        /// Disposes FirstSlimeKillDialogue, initializes next step in Tutorial
        /// </summary>
        private void EndFirstSlimeKillDialogue()
        {
            // Unlock Menu
            GameUIManager.Instance.enabled = true;
            // Resume Game
            GameManager.Instance.ResumeGame();
            // Initialize next step
            FloorManager.Instance.AddRoomLoadListener(InitFirstBookKillDialogue);
        }
        #endregion

        #region KilledFirstBook
        /// <summary>
        /// Initializes Listeners for FirstBookKillDialogue
        /// </summary>
        /// <param name="loadedRoom">Room that was Loaded</param>
        private void InitFirstBookKillDialogue(Room loadedRoom)
        {
            if (loadedRoom.name != FirstBookRoomName)
                return;
            // Remove RoomLoad-Listener
            FloorManager.Instance.RemoveRoomLoadListener(InitFirstBookKillDialogue);
            // Add Death-Listener to Book
            FloorManager.Instance.CurrentRoom.Enemies.OfType<BookEnemy>().First()?.AddDeathListener(RunFirstBookKillDialogue);
        }
        /// <summary>
        /// Runs FirstBookKillDialogue
        /// </summary>
        /// <param name="deadBook">Book that was killed</param>
        private void RunFirstBookKillDialogue(GameObject deadBook)
        {
            // Close Room, so Player cannot leave
            FloorManager.Instance.CurrentRoom.CloseDoors();
            // Initialize Page-Pickup (Done here because player is allowed to move during Dialogue)
            InitPagePickupDialogue();
            // Prevent opening of Menu during Dialogue
            GameUIManager.Instance.enabled = false;
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.KilledFirstBook],
                EndFirstBookKillDialogue);
        }
        /// <summary>
        /// Disposes FirstBookKillDialogue
        /// </summary>
        private void EndFirstBookKillDialogue()
        {
            // Unlock Menu
            GameUIManager.Instance.enabled = true;
        }
        #endregion

        #region PagePickup
        /// <summary>
        /// Initializes Listeners for PagePickupDialogue
        /// </summary>
        private void InitPagePickupDialogue()
        {
            // Add Listener for Page-Pickup
            PlayerManager.Instance.Inventory.AddPageListener(RunPagePickupDialogue);
        }
        /// <summary>
        /// Runs PagePickupDialogue
        /// </summary>
        /// <param name="pickedUp">Page that was picked up</param>
        private void RunPagePickupDialogue(SpellPage pickedUp)
        {
            // Remove Listener from Page-Pickup
            PlayerManager.Instance.Inventory.RemovePageListener(RunPagePickupDialogue);
            // Pause Game
            GameManager.Instance.PauseGame();
            // Prevent opening of Menu during Dialogue
            GameUIManager.Instance.enabled = false;
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.PagePickup],
                EndPagePickupDialogue);
        }
        /// <summary>
        /// Disposes PagePickupDialogue, initializes next step in Tutorial
        /// </summary>
        private void EndPagePickupDialogue()
        {
            // Unlock Menu
            GameUIManager.Instance.enabled = true;
            // Display Instruction Prompt
            DialogueManager.Instance.ShowInstructionPrompt("Please press \"ESC\" to open Greg");
            // Initialize next step
            InitEnterPauseMenuDialogue();
        }
        #endregion

        #region EnteredPauseMenu
        /// <summary>
        /// Initializes Listeners for PauseMenuDialogue
        /// </summary>
        private void InitEnterPauseMenuDialogue()
        {
            // Add Listener for Scene-Load
            SceneManager.sceneLoaded += RunEnterPauseDialogue;
        }
        /// <summary>
        /// Runs PauseMenuDialouge
        /// </summary>
        /// <param name="loadedScene">Scene that was Loaded</param>
        /// <param name="loadMode">Load-Mode for Scene</param>
        private void RunEnterPauseDialogue(Scene loadedScene, LoadSceneMode loadMode)
        {
            if (loadedScene.name != Constants.MainMenuSceneName)
                return;
            // Remove Listener from Scene-Load
            SceneManager.sceneLoaded -= RunEnterPauseDialogue;
            // Hide Instruction-Prompt
            DialogueManager.Instance.HideInstructionPrompt();
            // Prevent closing of Menu
            GameUIManager.Instance.enabled = false;
            // Resume after Scene has Initialized
            StartCoroutine(WaitForSceneLoadPauseDialogue());
        }
        /// <summary>
        /// Waits for MenuManager.Awake to run before starting Dialogue
        /// </summary>
        private IEnumerator WaitForSceneLoadPauseDialogue()
        {
            yield return new WaitUntil(() => MenuManager.Exists);
            // Initialize next step in Tutorial
            InitEnterSpellListDialogue();
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EnteredPauseMenu]);
        }
        #endregion

        #region EnteredSpellList
        /// <summary>
        /// Initializes Listeners for EnterSpellListDialogue
        /// </summary>
        private void InitEnterSpellListDialogue()
        {
            // Add Listener to SpellList-Button in Main Menu
            Transform spellTransform = MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Spell List");
            Button btn = spellTransform.GetComponent<Button>();
            btn.onClick.AddListener(RunEnterSpellListDialogue);
            // Disable all other displayed Buttons
            MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Monster Manual").GetComponent<Button>().interactable = false;
            MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Trinket Catalog").GetComponent<Button>().interactable = false;
            MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Settings").GetComponent<Button>().interactable = false;
        }
        /// <summary>
        /// Runs EnterSpellListDialogue, initializes next step in Tutorial
        /// </summary>
        private void RunEnterSpellListDialogue()
        {
            // Remove Listener from SpellList-Button
            Transform spellTransform = MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Spell List");
            Button btn = spellTransform.GetComponent<Button>();
            btn.onClick.RemoveListener(RunEnterSpellListDialogue);
            // Re-Enable disabled Buttons
            MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Monster Manual").GetComponent<Button>().interactable = true;
            MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Trinket Catalog").GetComponent<Button>().interactable = true;
            MenuManager.Instance.PauseMenuPanel.Find("Menu-Items/Settings").GetComponent<Button>().interactable = true;
            // Initialize next step in Tutorial
            InitEnterSpellDetailsDialogue();
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EnteredSpellList]);
        }
        #endregion

        #region EnteredSpellDetails
        /// <summary>
        /// Initializes Listeners for EnterSpellDetailsDialogue
        /// </summary>
        private void InitEnterSpellDetailsDialogue()
        {
            Transform leftPage = MenuManager.Instance.SpellListCanvas.Find("Book/Left Page");
            // Add Listener to Details-Button in SpellList
            leftPage.GetChild(1).GetComponent<Button>().onClick.AddListener(RunEnterSpellDetailsDialogue);
            // Disable all other displayed Buttons
            leftPage.GetChild(0).GetComponent<Button>().interactable = false;
            MenuManager.Instance.SpellListCanvas.Find("Book/Back Button").GetComponent<Button>().interactable = false;
        }
        /// <summary>
        /// Runs EnterSpellDetailsDialogue, initializes next step in Tutorial
        /// </summary>
        private void RunEnterSpellDetailsDialogue()
        {
            Transform leftPage = MenuManager.Instance.SpellListCanvas.Find("Book/Left Page");
            // Remove Listener from Details-Button in SpellList
            leftPage.GetChild(1).GetComponent<Button>().onClick.RemoveListener(RunEnterSpellDetailsDialogue);
            // Re-Enable disabled Buttons
            leftPage.GetChild(0).GetComponent<Button>().interactable = false;
            MenuManager.Instance.SpellListCanvas.Find("Book/Back Button").GetComponent<Button>().interactable = true;
            // Initialize next step in Tutorial
            InitEnterSpellPuzzleDialogue();
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EnteredSpellDetails]);
        }
        #endregion

        #region EnteredSpellPuzzle
        /// <summary>
        /// Initializes Listeners for EnterSpellPuzzleDialogue
        /// </summary>
        private void InitEnterSpellPuzzleDialogue()
        {
            // Add Listener to Puzzle-Button on Details-Page
            MenuManager.Instance.SpellCanvas.Find("Book/Page Right/Spell Unlock Button").GetComponent<Button>().onClick.AddListener(RunEnterSpellPuzzleDialogue);
            // Disable Back-Button on Details-Page
            MenuManager.Instance.SpellCanvas.Find("Book/Back Button").GetComponent<Button>().interactable = false;
        }
        /// <summary>
        /// Runs EnterSpellPuzzleDialogue, initializes next step in Tutorial
        /// </summary>
        private void RunEnterSpellPuzzleDialogue()
        {
            // Remove Listener from Puzzle-Button on Details-Page
            MenuManager.Instance.SpellCanvas.Find("Book/Page Right/Spell Unlock Button").GetComponent<Button>().onClick.RemoveListener(RunEnterSpellPuzzleDialogue);
            // Re-Enable Back-Button Details-Page
            MenuManager.Instance.SpellCanvas.Find("Book/Back Button").GetComponent<Button>().interactable = true;
            // Initialize next step in Tutorial
            InitFinishSpellPuzzleDialogue();
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.EnteredSpellPuzzle]);
        }
        #endregion

        #region FinishedSpellPuzzle
        /// <summary>
        /// Initializes FinishSpellPuzzleDialogue
        /// </summary>
        private void InitFinishSpellPuzzleDialogue()
        {
            // Add Listener to Spell-Unlock
            PlayerManager.Instance.Inventory.AddUnlockListener(RunFinishSpellPuzzleDialogue);
            // Disable Back-Button on Puzzle-Page
            MenuManager.Instance.ResearchCanvas.Find("Back Button").GetComponent<Button>().interactable = false;
        }
        /// <summary>
        /// Runs FinishSpellPuzzleDialogue
        /// </summary>
        /// <param name="unlockedSpell">Spell that was Unlocked</param>
        private void RunFinishSpellPuzzleDialogue(SpellPage unlockedSpell)
        {
            // Remove Listener from Spell-Unlock
            PlayerManager.Instance.Inventory.RemoveUnlockListener(RunFinishSpellPuzzleDialogue);
            // Re-Enable Back-Button on Puzzle-Page
            MenuManager.Instance.ResearchCanvas.Find("Back Button").GetComponent<Button>().interactable = true;
            // Run Dialogue
            DialogueManager.Instance.StartDialogue(dialogues[(int)TutorialSteps.FinishedSpellPuzzle],
                EndFinishSpellPuzzleDialogue);
        }
        /// <summary>
        /// Ends final step in Tutorial, relinquishing control to Player
        /// </summary>
        private void EndFinishSpellPuzzleDialogue()
        {
            // Allow closing of Menu
            GameUIManager.Instance.enabled = true;
            // Unlock Room, so Player can advance
            Room currRoom = FloorManager.Instance.CurrentRoom;
            if (currRoom.Enemies.Count == 0)
                currRoom.OpenDoors();
            // Destroy TutorialManager
            Destroy(gameObject);
        }
        #endregion
        #endregion
        #endregion
    }
}