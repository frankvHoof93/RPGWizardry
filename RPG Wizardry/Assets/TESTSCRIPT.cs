using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TESTSCRIPT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log(SceneManager.sceneCount);
            for (int i = 0; i < SceneManager.sceneCount; i++)
                Debug.Log(SceneManager.GetSceneAt(i).name);
        }
    }
}
