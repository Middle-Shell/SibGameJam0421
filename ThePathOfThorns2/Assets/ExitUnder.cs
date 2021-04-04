using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitUnder : MonoBehaviour
{
    public string namelvl;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("exit");
            SceneManager.LoadScene(namelvl);
        }
    }
}
