

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour, IInteractable
{
    Rigidbody2D rb;
    RaycastHit hit;

    AudioSource ac;
    DragonBones.UnityArmatureComponent anim;

    [SerializeField] LayerMask lMask;
    [SerializeField] LayerMask FlowLayer;
    [SerializeField] LayerMask Lava;
    private Coroutine currentCoroutine;

    [SerializeField] float speed = 5;
    [SerializeField] float speedMode = 0f;
    [SerializeField] float jumpForce = .6f;

    public float health = 3f;
    [SerializeField] bool lava = false;

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

    public Collider2D myCollider = null;
    public float floorDistance = 0.1f;
    public bool isGroundedvar => myCollider != null ? Physics2D.Raycast(myCollider.bounds.min, Vector2.down, floorDistance) : false;

    public Image img;
    public AnimationCurve curve;
    float invisible;
    public Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        myCollider = GetComponent<Collider2D>();

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
        {
            transform.localScale = new Vector3(inputX > 0 ? defaultScaleX : -defaultScaleX, transform.localScale.y, transform.localScale.y);
            //1 - right  -1 - left
        }

        if (!canMove)
            return;

        jumpRemember -= Time.deltaTime;

        if (!onVine)
        {
            transform.Translate(new Vector2(inputX * Time.deltaTime * (speed + (inputX==1? speedMode : -0.5f * speedMode)) * (running ? 1.5f : 1), 0)); //течения надо придумать

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
            {
                rb.velocity = new Vector2(0, inputY * 5);
            }
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
                if (anim.animation.lastAnimationName != "die_2_down")
                {
                    anim.animation.Play("die_2_down", 1);
                    if (transform.position.x >= 52f)
                    {
                        AudioSystem("jump_down");
                    }
                    else
                    {
                        AudioSystem("jump_down");
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
                if (anim.animation.lastAnimationName != "die_2_up")
                    anim.animation.Play("die_2_up", 1);
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
                if (anim.animation.lastAnimationName != "idle")
                    anim.animation.Play("idle");
            }
            // Идём
            else if (!running)
            {
                if (isGroundedvar && anim.animation.lastAnimationName != "goes")
                {
                    anim.animation.Play("goes");
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
                if (isGroundedvar && anim.animation.lastAnimationName != "beg")
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
                if (anim.animation.lastAnimationName == "die_2_up" && anim.animation.isPlaying)
                {
                    // Ждём окончания анимации подпрыгивания
                }
                else if (rb.velocity.y < 0)
                {
                    if (anim.animation.lastAnimationName != "down")
                        anim.animation.Play("down");
                }
                else if (rb.velocity.y > 0)
                {
                    if (anim.animation.lastAnimationName != "up")
                        anim.animation.Play("up");
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
        bool onTheGround = isGroundedvar;
        bool attack = Input.GetKeyDown(KeyCode.Mouse0);
        bool jump = Input.GetButtonDown("Jump");
        RaycastHit2D hit;


        UpdateMoving(inputX, inputY, running, debugCheat, onTheGround, attack, jump);
        UpdateAnimation(inputX, inputY, running, debugCheat, onTheGround, attack, jump);
        if (health <= 0)
        {
            //anim.animation.Play("die");
            Invoke("toSpawn", 1f);
        }
        if (lava)
        {
            currentCoroutine = StartCoroutine(HitCoroutine(0.01f, 1f, lava));
            StartCoroutine(BloodScreen(lava, 0.5f, 0f));
        }
        else
        {
            //StartCoroutine(BloodScreen(lava, 0.5f));
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

    public IEnumerator HitCoroutine(float damage, float delayTime, bool lava)
    {
        if (lava)
        {
            yield return new WaitForSeconds(delayTime);
            health -= damage;
        }
    }
    public IEnumerator BloodScreen(bool lava, float delayTime, float invisible = 100f)
    {
        yield return new WaitForSeconds(delayTime);
        if (!lava)
        {
            while (invisible > 0f)
            {
                invisible += Time.deltaTime* 0.1f;
                float a = curve.Evaluate(invisible);
                img.color = new Color(0.97f, 0.12f, 0.12f, a);
                if (cam.orthographicSize < 7f)
                {
                    cam.orthographicSize += 0.5f;
                }
                //cam.orthographicSize = 7.0f;
                yield return 0;
            }
        }
        else
        {

            while (invisible < 100f)
            {
                invisible -= Time.deltaTime * 0.2f;
                float a = curve.Evaluate(invisible);
                img.color = new Color(0.97f, 0.12f, 0.12f, a);
                if (cam.orthographicSize > 5f)
                {
                    cam.orthographicSize -= 0.5f;
                }
                //cam.orthographicSize = 5.0f;
                yield return 0;
            }
        }
        
    }
    public void Hit(float damage = 0.01f)
    {
        Debug.Log($"Damaged for {damage} damage");
        health -= damage;
    }
    void toSpawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == FlowLayer)//чек на вход в течение
        {
            speedMode = 5f;
        }
        if (1 << other.gameObject.layer == Lava)//чек на вход в лаву
        {
            lava = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == FlowLayer)//чек на выход из течения
        {
            speedMode = 0f;
        }
        if (1 << other.gameObject.layer == Lava)//чек на вход в лаву
        {
            StopCoroutine("HitCoroutine");
            lava = false;
        }
    }
}
