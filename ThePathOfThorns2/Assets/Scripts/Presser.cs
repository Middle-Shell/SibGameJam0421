using UnityEngine;

public class Presser : MonoBehaviour
{
    DragonBones.UnityArmatureComponent anim;
    [SerializeField] GameObject doorObject;
    doorOpener door;

    void Start()
    {
        door = doorObject.GetComponent<doorOpener>();
        anim = GetComponent<DragonBones.UnityArmatureComponent>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "box")
        {
            anim.animation.Play("on", 1);

            door.Open();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "box")
        {
            anim.animation.Play("off", 1);

            door.Close();
        }
    }
}
