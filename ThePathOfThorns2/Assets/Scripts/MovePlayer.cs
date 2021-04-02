#define FALLING_VERSION_1

using UnityEngine;
using System.Linq;

public class MovePlayer : MonoBehaviour
{
    Rigidbody2D rb;
    //BoxCollider2D col;

    CapsuleCollider2D col;

    RaycastHit hit;

    AudioSource ac;
    DragonBones.UnityArmatureComponent anim;

    [SerializeField] LayerMask lMask;

    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = .6f;
    public float health = 3f;

    [SerializeField] Transform punch;
    [SerializeField] float punchRadius;
    bool canMove = true;
    float jumpRememberTime = 2f;
    float jumpRemember = 0;
    float defaultScaleX;
    float timeWithoutGround, fallingTime = 0;
    bool flyAnim = false;
    float wJump;
    //float rememberedInputYForVine;

    public bool onVine;
    private bool IsGrounded = true;

    public bool isGrounded // Проверка на приземлённость
    {
        get
        {
            return IsGrounded;
        }
        set
        {
            IsGrounded = value;
        }//Это же жесть, я уже начал сомневаться во всём в своих навыках, в своих знаниях, в цивилизации, а оказывается я просто не нажал кнопку istrigger, 3-4 часа, куча прочитанных форумов и штук 6 способов написать этот код АААААААААААААААААААААААААААА
        /*Physics.CapsuleCast(p1, p2, charContr.radius, transform.forward, out hit, 10);
        //Physics2D.CapsuleCast(col.bounds.center, col.bounds.size, Vertical, 0.1f, Vector2.down);// .1f);
        RaycastHit2D raycast = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0f, Vector2.down, .1f, lMask);
        return raycast.collider != null;*/

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //col = GetComponent<BoxCollider2D>();

        col = GetComponent<CapsuleCollider2D>();

        ac = GetComponent<AudioSource>();
        anim = GetComponent<DragonBones.UnityArmatureComponent>();

        defaultScaleX = transform.localScale.x;
    }

    void UpdateMoving(float inputX, float inputY, bool running, bool debugCheat, bool onTheGround, bool attack, bool jump)
    {
        // Ожидание прыжка (для анимации)
        if (!running)
        {
            jump |= wJump > 0;
            if (!jump)
                wJump = 0;
            else
                wJump += Time.deltaTime;
            jump = wJump > 0.12f;
            if (jump)
                wJump = 0;
        }

        if (inputX != 0)
            transform.localScale = new Vector3(inputX > 0 ? defaultScaleX : -defaultScaleX, transform.localScale.y, transform.localScale.y);

        if (!canMove)
            return;

        jumpRemember -= Time.deltaTime;

        if (!onVine)
        {
            transform.Translate(new Vector2(inputX * Time.deltaTime * speed * (running ? 1.5f : 1), 0));

            if (jump && onTheGround)
                jumpRemember = jumpRememberTime;

            if ((jumpRemember > 0) && onTheGround)
            {
                jumpRemember = 0;
                rb.AddForce(new Vector2(0, jumpForce * (debugCheat ? 25 : 10)), ForceMode2D.Impulse);
            }

            if (attack)
                //точкa контакта, радиус, номер слоя юнита, урон по цели, только один враг получает урон
                Fight2D.Action(punch.position, punchRadius, 9, 1, false);
        }
        else
        {
            if (inputY != 0)
                rb.velocity = new Vector2(0, inputY * 5);
            else
                rb.velocity = new Vector2(0, 0);

            if (jump)
            {
                onVine = false;
                rb.gravityScale = 1;

                if (transform.localScale.x < 0)
                {
                    rb.velocity = new Vector2(-5f, 3f);
                }
                else if (transform.localScale.x > 0)
                {
                    rb.velocity = new Vector2(5f, 3f);
                }
            }
        }
    }

    void UpdateAnimation(float inputX, float inputY, bool running, bool debugCheat, bool onTheGround, bool attack, bool jump)
    {
        // Вычисляем время в воздухе
        if (onTheGround)
        {
            timeWithoutGround = 0;
            fallingTime = 0;
        }
        else
        {
            timeWithoutGround += Time.deltaTime;
            if (rb.velocity.y < 0)
                fallingTime += Time.deltaTime;
            else
                fallingTime = 0;
        }

        if (onVine)
        {
            if (anim.animation.lastAnimationName != "lezet_po_lianye")
                anim.animation.Play("lezet_po_lianye");
            anim.animation.timeScale = inputY;
            /*
            if (inputY != rememberedInputYForVine)
            {
                anim.animation.Play("lezet_po_lianye");
                if (inputY > 0)
                    
                else if (inputY < 0)

            }
            rememberedInputYForVine = inputY;*/
        }

        // На земле
        else if (onTheGround)
        {
            anim.animation.timeScale = 1;
            // После прыжка
            if (flyAnim)
            {
                if (anim.animation.lastAnimationName != "polyot_to_stop")
                {
                    anim.animation.Play("polyot_to_stop", 1);
                    if (transform.position.x >= 52f)
                    {
                        AudioSystem("Snow_jump_end");
                    }
                    else
                    {
                        AudioSystem("Jump_end");
                    }

                }
                else if (!anim.animation.isPlaying)
                    flyAnim = false;
            }
            // Атакуем
            else if (attack)
            {
                if (anim.animation.lastAnimationName != "ydar")
                    anim.animation.Play("ydar", 1);
                AudioSystem($"loli_atack_{Random.Range(1, 5)}");
            }
            // Перед прыжком
            else if (jump || wJump > 0)
            {
                if (anim.animation.lastAnimationName != "podprig")
                    anim.animation.Play("podprig", 1);
                if (transform.position.x >= 52f)
                {
                    AudioSystem("Snow_jump_begin");
                }
                else
                {
                    AudioSystem("Jump_begin");
                }
            }
            // Ждём сперва окончания анимации
            else if ((anim.animation.lastAnimationName == "ydar" || anim.animation.lastAnimationName == "podprig") && anim.animation.isPlaying)
            {
                //rb.AddForce(new Vector2(4, 7), ForceMode2D.Impulse);
            }
            // Стоим на месте
            else if (inputX == 0)
            {
                if (anim.animation.lastAnimationName != "stoit")
                    anim.animation.Play("stoit");
            }
            // Идём
            else if (!running)
            {
                if (IsGrounded && anim.animation.lastAnimationName != "hodba")
                {
                    anim.animation.Play("hodba");
                    if (transform.position.x >= 52f)
                    {
                        AudioSystem("Loli_step_snow");
                    }
                    else
                    {
                        AudioSystem("Loli_step");
                    }
                }

            }
            // Бежим
            else
            {
                if (IsGrounded && anim.animation.lastAnimationName != "beg")
                    anim.animation.Play("beg");
            }
        }

        // В воздухе
        else
        {
            anim.animation.timeScale = 1;
            // Вычисляем коэффициент анимации в воздухе
            float flyAnimCoef = Mathf.Abs(rb.velocity.y / 3) - Mathf.Abs(rb.velocity.x / 5) + timeWithoutGround / 2 + fallingTime / 3;
            //Debug.Log(flyAnimCoef);   Я ХОТЕЛ СДЕЛАТЬ КРАСИВО НО НИХУЯ НЕ ПОЛУЧИЛОСЬ ХД

            // Включаем
            if (flyAnimCoef > 1f)
            {
                flyAnim = true;
                if (anim.animation.lastAnimationName == "podprig" && anim.animation.isPlaying)
                {
                    // Ждём окончания анимации подпрыгивания
                }
                else if (rb.velocity.y < 0)
                {
                    if (anim.animation.lastAnimationName != "polyot_v_niz")
                        anim.animation.Play("polyot_v_niz");
                }
                else if (rb.velocity.y > 0)
                {
                    if (anim.animation.lastAnimationName != "polyot_v_verh")
                        anim.animation.Play("polyot_v_verh");
                }
            }
        }
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        bool running = Input.GetKey(KeyCode.LeftShift);
        bool debugCheat = Input.GetKey(KeyCode.LeftControl);
        bool onTheGround = IsGrounded;
        bool attack = Input.GetKeyDown(KeyCode.Mouse0);
        bool jump = Input.GetButtonDown("Jump");
        RaycastHit2D hit;

        UpdateMoving(inputX, inputY, running, debugCheat, onTheGround, attack, jump);
        UpdateAnimation(inputX, inputY, running, debugCheat, onTheGround, attack, jump);

        /*if(Physics2D.Raycast(transform.position, Vector2.down, hit, 2))
        {
            if(hit.transform.tag == "grass")
            {

            }
        }*/
        if (health <= 0)
        {
            Invoke("toSpawn", .5f);
        }
    }

    void AudioSystem(string nameOfClip)
    {
        GetComponents<AudioSource>().FirstOrDefault(s => s.clip.name == nameOfClip)?.Play();
    }

    // смерть (изменится код)
    public void kill()
    {
        canMove = false;
        Invoke("destr", .3f);
    }

    public void Hit(int damage)
    {
        Debug.Log($"Damaged for {damage} damage");
        health -= damage;
    }

    void toSpawn()
    {
        transform.position = new Vector2(-13f, -2.66f);
        health = 3f;
    }
}
