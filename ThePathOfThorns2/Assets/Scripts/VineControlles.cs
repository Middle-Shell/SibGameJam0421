using UnityEngine;

public class VineControlles : MonoBehaviour
{
    public bool flag = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && collision.tag == "Player")
        {
            if (!flag)
            {
                collision.transform.position = new Vector3(transform.position.x, collision.transform.position.y, collision.transform.position.z);

                collision.GetComponent<MovePlayer>().onVine = true;
                collision.GetComponent<Rigidbody2D>().gravityScale = 0;

                flag = true;
            }
            else
            {
                collision.GetComponent<MovePlayer>().onVine = false;
                collision.GetComponent<Rigidbody2D>().gravityScale = 1;

                flag = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<MovePlayer>().onVine = false;
            collision.GetComponent<Rigidbody2D>().gravityScale = 1;

            flag = false;
        }
    }
}