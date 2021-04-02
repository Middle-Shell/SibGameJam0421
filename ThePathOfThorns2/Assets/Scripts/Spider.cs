using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spider : MonoBehaviour
{
    enum SpiderBehavour
    {
        FallAndBack,
        FallAndAttack,
        FallAndRunAway,
        Attack,
    }

    enum SpiderState
    {
        WaitForPlayer,
        Jump,
        Attack,
        RunAway,
        Returning
    }


    [SerializeField]
    float SeeDistance = 5;
    [SerializeField]
    float JumpForce = 7;
    [SerializeField]
    float MaxSpeed = 3;
    [SerializeField]
    float ReturningSpeed = 0.5f;
    [SerializeField]
    SpiderBehavour Behavour = SpiderBehavour.FallAndRunAway;
    [SerializeField]
    SpiderState State = SpiderState.WaitForPlayer;
    [SerializeField]
    private GameObject webPrefab;
    [SerializeField]
    LayerMask lMask;

    private GameObject web;
    private Transform player;
    float timer;
    private Vector2 startPosition;

    Rigidbody2D rb;

    void Start()
    {
        if (Behavour == SpiderBehavour.FallAndAttack || Behavour == SpiderBehavour.FallAndRunAway)
        {
            // Создаём паутинку
            RaycastHit2D raycast = Physics2D.Linecast((Vector2)transform.position, (Vector2)transform.position + Vector2.up * 20, lMask);
            //Debug.Log(raycast.point);
            //web = Instantiate(webPrefab, transform);
            //web.transform.position.Set(0, raycast.point.y / 2, 0);
            //web.transform.localScale.Set(0.05f, raycast.point.y, 1);
            //Debug.Log(web.transform.position);
            // НЕ РАБОТАЕТ! Пофиксить если будет время

            // принцип - кидаем рейкаст вверх, находим ближайший потолок, берём префаб паутинки, вставляем посередине и вытягиваем до нужного размера
            // при прыжке паука - паутинка должна удаляться
        }
        State = SpiderState.WaitForPlayer;

        startPosition = transform.position;

        // Ставим себе тег
        this.tag = "Enemy";
        // Вытягиваем игрока по тегу
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    void AudioSystem(string nameOfClip)
    {
        GetComponents<AudioSource>().FirstOrDefault(s => s.clip.name == nameOfClip)?.Play();
    }

    public void Hit(float damage)
    {
        AudioSystem("spider_damage");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 jumpVector;
        switch (State)
        {
            case SpiderState.WaitForPlayer:
                if ((player.position - this.transform.position).magnitude < SeeDistance)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    State = SpiderState.Jump;
                    timer = 1000;
                    if ((player.position.y < transform.position.y))
                    {
                        jumpVector = (player.position - transform.position);
                        jumpVector.Scale(new Vector3(2, Behavour == SpiderBehavour.FallAndBack ? 20 : 1, 0));
                        jumpVector.Normalize();
                        rb.AddForce((Vector2)(jumpVector * JumpForce), ForceMode2D.Impulse);
                        AudioSystem("spider_atack");
                    }
                }
                // TODO - если паук висит на потолке - то чекать дополнительно что игрок внизу в зоне досягаемости прыжка
                break;
            case SpiderState.Jump:
                timer -= Time.fixedDeltaTime;
                if (rb.velocity.y >= -0.1f)
                {
                    timer = Mathf.Min(timer, 0.3f);
                }
                if (timer <= 0)
                {
                    if (Behavour == SpiderBehavour.FallAndRunAway)
                    {
                        if (transform.position.x > player.position.x)
                            jumpVector = new Vector3(5, 3, 0);
                        else
                            jumpVector = new Vector3(-5, 3, 0);
                        jumpVector.Normalize();
                        jumpVector = jumpVector * JumpForce * 1.5f;
                        Debug.Log(jumpVector);
                        rb.AddForce(jumpVector, ForceMode2D.Impulse);
                        State = SpiderState.RunAway;
                    }
                    else if (Behavour == SpiderBehavour.FallAndBack)
                    {
                        rb.gravityScale = 0;
                        AudioSystem("spider_up");
                        State = SpiderState.Returning;
                    }
                    else
                    {
                        State = SpiderState.Attack;
                    }
                }
                // Ждём конца падения
                break;
            case SpiderState.Attack:
                /*if (transform.position.x < player.position.x)
                    //transform.Translate(new Vector2(1 * Time.deltaTime * MaxSpeed, 0), Space.World);
                    rb.velocity.Set(10 * Time.deltaTime * MaxSpeed, 0);
                else
                    rb.velocity.Set(-10 * Time.deltaTime * MaxSpeed, 0);
                //transform.Translate(new Vector2(1 * Time.deltaTime * MaxSpeed, 0), Space.World);
                //jumpVector.Normalize();
                //jumpVector = jumpVector * JumpForce * 1.5f;
                //rb.AddForce(jumpVector, ForceMode2D.Force);
                //rb.velocity.Set(Mathf.Min(MaxSpeed, rb.velocity.x), rb.velocity.y);*/
                if (transform.position.x < player.position.x)
                    jumpVector = new Vector3(20, 1, 0);
                else
                    jumpVector = new Vector3(-20, 1, 0);
                jumpVector.Normalize();
                jumpVector = jumpVector * MaxSpeed;
                //rb.AddForce(jumpVector, ForceMode2D.Force);
                rb.velocity = new Vector2(jumpVector.x, rb.velocity.y + jumpVector.y);
                //rb.velocity.Set(Mathf.Min(MaxSpeed, rb.velocity.x), rb.velocity.y);
                break;
            case SpiderState.RunAway:
                // Уменьшаемся
                transform.localScale -= new Vector3(0.15f, 0.15f) * Time.fixedDeltaTime * Mathf.Pow((player.position - this.transform.position).magnitude / 7, 2.5f);
                // И по съёбам
                if (transform.position.x > player.position.x)
                    jumpVector = new Vector3(5, 1, 0);
                else
                    jumpVector = new Vector3(-5, 1, 0);
                jumpVector.Normalize();
                jumpVector = jumpVector * JumpForce * 1.5f;
                rb.AddForce(jumpVector, ForceMode2D.Force);
                rb.velocity.Set(Mathf.Min(MaxSpeed, rb.velocity.x), rb.velocity.y);
                // Если совсем мелкие - удаляемся
                if (this.transform.localScale.x < 0.05)
                    Destroy(gameObject);
                break;
            case SpiderState.Returning:
                var backVector = (startPosition - (Vector2)transform.position);
                if (backVector.magnitude > ReturningSpeed * Time.fixedDeltaTime)
                {
                    transform.Translate(backVector.normalized * ReturningSpeed * Time.fixedDeltaTime, Space.World);
                }
                else
                {
                    transform.position = startPosition;
                    State = SpiderState.WaitForPlayer;
                    rb.gravityScale = 1;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;
                }
                break;
            default:
                break;
        }
    }
}
