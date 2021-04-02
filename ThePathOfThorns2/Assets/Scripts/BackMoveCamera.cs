using UnityEngine;

public class BackMoveCamera : MonoBehaviour
{
    CameraFollow2D mc;
    float defaultSize;

    private void Start()
    {
        mc = Camera.main.GetComponent<CameraFollow2D>();
        defaultSize = Camera.main.orthographicSize;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            mc.ChangeSize(8);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            mc.ChangeSize(defaultSize);
        }
    }
}
