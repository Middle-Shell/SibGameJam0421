using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class hit : MonoBehaviour
{
    public TextMeshProUGUI self;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            self.text = "YOU DEAD";
            StartCoroutine(toSpawn(2f));
        }
        
    }
    public IEnumerator toSpawn(float delayTime)
    {
        
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
