using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Serialization;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : SingletonBehaviour<MenuManager>
{
    [SerializeField]
    private GameObject background;


    [Header("Main Menu")]

    [SerializeField]
    private GameObject mainMenuPanel;
    [SerializeField]
    private Button loadGameButton;

    public void Init(bool additiveLoad)
    {
        background.SetActive(!additiveLoad);
        if (CameraManager.Exists)
            CameraManager.Instance.ToggleAudio();
        if (!additiveLoad)
            InitMainMenu();
        else
            InitGameMenu();
    }

    private void OnDisable()
    {
        mainMenuPanel.SetActive(false);
    }

    private void InitGameMenu()
    {
    }

    private void InitMainMenu()
    {
        mainMenuPanel.SetActive(true);
        loadGameButton.interactable = SaveManager.HasSave();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}