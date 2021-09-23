using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour, IInteractable
{
    Rigidbody2D rb;
    RaycastHit hit;

    AudioSource ac;
    DragonBones.UnityArmatureComponent anim;
    [SerializeField] List<LayerMask> LMask = new List<LayerMask>();

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
    bool die = false;
    public byte Bonuses = 0;
    //float rememberedInputYForVine;

    public bool onVine;

    public Collider2D myCollider = null;
    public float floorDistance = 0.1f;
    public bool isGroundedvar => myCollider != null ? Physics2D.Raycast(myCollider.bounds.min, Vector2.down, floorDistance) : false;

    public AnimationCurve curve;
    float invisible;
    public Camera cam;
    AudioSource audioSrc;

    public GameObject Text_of_Dead = GameObject.FindGameObjectWithTag("Dead");
    public Vector3 StartPosition;
    public GameObject Lamp;
    private GameObject[] players;
    public SoundEffectsHelper SEH = GameObject.FindObjectOfType<SoundEffectsHelper>();
    public GameObject Win_text;


    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
        {
            Destroy(this.gameObject);
        }
        rb = GetComponent<Rigidbody2D>();

        myCollider = GetComponent<Collider2D>();
        StartPosition = transform.position;

        ac = GetComponent<AudioSource>();
        anim = GetComponent<DragonBones.UnityArmatureComponent>();
        audioSrc = GetComponent<AudioSource>();

        defaultScaleX = transform.localScale.x;

        DontDestroyOnLoad(transform.gameObject);
        SEH.On_Beach_Music();
    }

    void UpdateMoving(float inputX, float inputY, bool running, bool debugCheat, bool onTheGround, bool attack, bool jump)
    {
        if (canMove)
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

            jumpRemember -= Time.deltaTime;

            if (!onVine)
            {
                transform.Translate(new Vector2(inputX * Time.deltaTime * (speed + (inputX == 1 ? speedMode : -0.5f * speedMode)) * (running ? 1.5f : 1), 0)); //течения надо придумать

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
    }

    void UpdateAnimation(float inputX, float inputY, bool running, bool debugCheat, bool onTheGround, bool attack, bool jump, float health)
    {
        if (health > 0)
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
                    audioSrc.Stop();
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
                    audioSrc.Stop();
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
                    audioSrc.Stop();
                    if (anim.animation.lastAnimationName != "idle")
                        anim.animation.Play("idle");
                }
                // Идём
                else if (!running)
                {
                    if (isGroundedvar && anim.animation.lastAnimationName != "goes")
                    {
                        anim.animation.Play("goes");
                        if (!audioSrc.isPlaying)
                            audioSrc.Play();
                        /*if (transform.position.x >= 52f)
                        {
                            AudioSystem("Loli_step_snow");
                        }
                        else
                        {
                            AudioSystem("Loli_step");
                        }*/
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
                audioSrc.Stop();
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
        else
        {
            audioSrc.Stop();
            if (die && health <= 0)
            {
                anim.animation.Play("die");
                die = false;
                StartCoroutine(toSpawn(2f));
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
        UpdateAnimation(inputX, inputY, running, debugCheat, onTheGround, attack, jump, health);
        if (lava)
        {
            currentCoroutine = StartCoroutine(HitCoroutine(0.01f, 1f, lava));
        }

    }

    void AudioSystem(string nameOfClip)
    {
        GetComponents<AudioSource>().FirstOrDefault(s => s.clip.name == nameOfClip)?.Play();
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

    [System.Obsolete]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == LMask[1])//чек на вход в течение
        {
            speedMode = 5f;
        }
        if (1 << other.gameObject.layer == LMask[2])//чек на вход в лаву
        {
            lava = true;
        }
        if (other.tag == "Enemy" || other.tag == "Killer")
        {
            health = 0;
            die = true;
        }
        if (other.tag == "Bonus")
        {
            Bonuses++;
        }
        if (other.tag == "SavePoint")
        {
            StartPosition = other.transform.position;
        }
        if (other.tag == "Portal")
        {
            if(Bonuses > 2)
            {
                Win_text.SetActive(true);
                SEH.AS.PlayOneShot(SEH.audioClips[4]);
                StartCoroutine(toMenu(2f));
            }
            
        }
        if (1 << other.gameObject.layer == LMask[3])
        {
            transform.position = new Vector3(403, -141, 0); //можно отредактировать  для расширения проекта (но никто этим заниматься не будет)
            checkLamp();
            SEH.On_Underground_Music();
        }
        if (1 << other.gameObject.layer == LMask[4])
        {
            if (Bonuses >= 2)
            {
                transform.position = new Vector3(702, -6, 0);
                checkLamp();
                SEH.On_Deep_Music();
            }
        }
        if (1 << other.gameObject.layer == LMask[5])//win
        {
            SEH.AS.PlayOneShot(SEH.audioClips[4]);
            //Application.LoadLevel("lvl0");
            this.transform.position = new Vector3(-6f, 179f, 0f);
            cam.transform.position = new Vector3(-6f, 179f, 0f);
            SEH.On_Beach_Music();
            GameObject.FindGameObjectWithTag("Portal").GetComponent<Check_Event>().CheckStatus(Bonuses);
            //SEH.On_Win_Music();
        }
        if (1 << other.gameObject.layer == LMask[6])//start game
        {
            //Application.LoadLevel("new_lvl1");
            this.transform.position = new Vector3(16f, -20f, 0f);
            cam.transform.position = new Vector3(16f, -20f, 0f);
            SEH.On_Deep_Music();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == LMask[1])//чек на выход из течения
        {
            speedMode = 0f;
        }
        if (1 << other.gameObject.layer == LMask[2])//чек на выход  лаву
        {
            StopCoroutine("HitCoroutine");
            lava = false;
        }
    }
    public IEnumerator toSpawn(float delayTime)
    {
        canMove = false;
        Text_of_Dead.SetActive(true);
        SEH.AS.PlayOneShot(SEH.audioClips[3]);
        //SoundEffectsHelper.Instance.On_Loose_Music();
        yield return new WaitForSeconds(delayTime);
        transform.position = StartPosition;
        canMove = true;
        Text_of_Dead.SetActive(false);
        health = 3f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameObject.FindGameObjectWithTag("Bubble").transform.localScale = new Vector3(1, 1, 1);
        GameObject.FindGameObjectsWithTag("Killer")[0].GetComponent<EnemyAI>().ReloadScene();
        //Text_of_Dead = GameObject.FindGameObjectsWithTag("Dead")[0];
        checkLamp();
    }
    void checkLamp()
    {
        if (transform.position.y <= -50)
        {
            Lamp.SetActive(true);
            SEH.On_Underground_Music();
            
            Debug.Log("Under");
        }
        else
        {
            Lamp.SetActive(false);
            SEH.On_Deep_Music();
            Debug.Log("Upper");
        }
    }
    public IEnumerator toMenu(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Application.LoadLevel("Menu");
        Destroy(GameObject.FindGameObjectWithTag("Killer"));
        Win_text.SetActive(false);
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        
    }
}
