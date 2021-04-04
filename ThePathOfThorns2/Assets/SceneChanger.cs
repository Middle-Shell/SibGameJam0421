using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private int sceneIndex = 0;

    public void ChangeScene()
    {
        if (sceneIndex < SceneManager.sceneCount)
            SceneManager.LoadScene(sceneIndex);
    }
}
