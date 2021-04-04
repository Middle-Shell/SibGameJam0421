using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class final : MonoBehaviour
{
    public TextMeshProUGUI self;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //self.text = "Congratulations! You WIN!";
            StartCoroutine(toSpawn(5f));
        }

    }
    public IEnumerator toSpawn(float delayTime)
    {

        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("Underground");
    }
}
