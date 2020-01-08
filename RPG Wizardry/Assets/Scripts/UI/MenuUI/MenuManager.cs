using nl.SWEG.RPGWizardry.GameWorld;
using nl.SWEG.RPGWizardry.Utils.Behaviours;
using UnityEngine;

public class MenuManager : SingletonBehaviour<MenuManager>
{
    [SerializeField]
    private GameObject background;

    public void Init(bool additiveLoad)
    {
        background.SetActive(!additiveLoad);
        if (CameraManager.Exists)
            CameraManager.Instance.ToggleAudio();
    }
}