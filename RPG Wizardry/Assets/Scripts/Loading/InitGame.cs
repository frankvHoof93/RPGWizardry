using UnityEngine;
using UnityEngine.SceneManagement;

public class InitGame : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
    }
}