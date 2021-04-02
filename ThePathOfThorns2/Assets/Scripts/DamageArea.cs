using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField]
    int Damage;
    [SerializeField]
    float upForce;

    Collider2D collision;

    void OnTriggerEnter2D(Collider2D _collision)
    {
        collision = _collision;

        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<MovePlayer>().Hit(Damage);
            Invoke("addFrs", .2f);
        }
    }
    
    void addFrs()
    {
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, upForce), ForceMode2D.Impulse);
    }
}
