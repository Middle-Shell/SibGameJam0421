using UnityEngine;

public class CristalScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Camera.main.GetComponent<CameraFollow2D>().addCreistals();

            Destroy(gameObject);
        }
    }
}
