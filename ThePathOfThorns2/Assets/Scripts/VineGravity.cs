using UnityEngine;

public class VineGravity : MonoBehaviour
{
    VineControlles pFlag;

    private void Start()
    {
        pFlag = transform.GetComponentInParent<VineControlles>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && collision.tag == "Player")
        {
            if (!pFlag.flag)
            {
                collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                collision.GetComponent<MovePlayer>().onVine = true;
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().gravityScale = 0;
                collision.GetComponent<MovePlayer>().onVine = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pFlag.flag = false;

            collision.GetComponent<MovePlayer>().onVine = false;
            collision.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
