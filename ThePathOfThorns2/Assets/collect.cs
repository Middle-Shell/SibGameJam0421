using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class collect : MonoBehaviour
{

    string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            StreamWriter sw = new StreamWriter(path);
            //Write a line of text
            sw.WriteLine(1);
            //Close the file
            sw.Close();
            Destroy(gameObject, 0.3f);
        }
    }
}
