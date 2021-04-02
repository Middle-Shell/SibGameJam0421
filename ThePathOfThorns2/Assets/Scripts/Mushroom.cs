using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public enum MushroomState
    {
        Start,
        Reloading,
        Ready,
        Jump,
        Attack,
        Hiding
    }

    private GameObject player;
    private float startY;
    private float vspeed;
    private float timer;
    private float Damage = 1f;
    MushroomState state = MushroomState.Start;

    Renderer m_Renderer;
    public ParticleSystem particleObject;

    [SerializeField]
    float SeeDistance = 3f;
    [SerializeField]
    float DamageDistance = 4f;
    [SerializeField]
    float HidingSpeed = 0.6f;
    [SerializeField]
    float HidingDepth = 0.8f;
    [SerializeField]
    float ReadyDepth = 0.5f;
    [SerializeField]
    float ReloadingTimer = 5f;
    [SerializeField]
    float AttackTimer = 3f;
    [SerializeField]
    float JumpSpeed = 15f;


    // Start is called before the first frame update
    void Start()
    {
        // Ставим себе тег
        this.tag = "Enemy";
        // Вытягиваем игрока по тегу
        player = GameObject.FindGameObjectWithTag("Player");
        // Запоминаем изначальную позицию
        startY = transform.position.y;
        // Прячемся под землю и говорим что мы готовы к прыжку
        transform.Translate(Vector3.down * ReadyDepth, Space.World);
        state = MushroomState.Ready;
        //particleObject.Stop();

        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit(float damage)
    {
        AudioSystem("Mushroom_damage");
    }

    void AudioSystem(string nameOfClip)
    {
        GetComponents<AudioSource>().FirstOrDefault(s => s.clip.name == nameOfClip)?.Play();
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case MushroomState.Ready:
                // Если видим игрока - прыгаем
                if ((player.transform.position - this.transform.position).magnitude < SeeDistance)
                {
                    state = MushroomState.Jump;
                    vspeed = JumpSpeed;
                }
                break;
            case MushroomState.Jump:
                // Летим
                transform.Translate(Vector3.up * vspeed * Time.fixedDeltaTime, Space.World);
                // Если не под землёй - гравитация
                if (transform.position.y > startY)
                {
                    vspeed -= 0.5f;
                }
                // Иначе если уже упали под землю - переходим в атаку
                else if (vspeed < 0)
                {
                    particleObject.Play();
                    // Встаём куда надо
                    transform.position.Set(transform.position.x, startY, transform.position.z);
                    // Атакуем
                    m_Renderer.material.color = Color.red;
                    state = MushroomState.Attack;
                    AudioSystem("Mushroom_atack");

                    timer = AttackTimer;
                }
                break;
            case MushroomState.Attack:
                // Ждём время атаки
                if ((player.transform.position - this.transform.position).magnitude < DamageDistance && ((((int)(timer * 100)) / 100f) % 1) == 0)
                {
                    player.GetComponent<MovePlayer>().health -= 1f;
                }
                timer -= Time.fixedDeltaTime;
                // По истечению таймера - прячемся
                if (timer <= 0)
                {
                    state = MushroomState.Hiding;
                    m_Renderer.material.color = Color.white;
                }
                break;
            case MushroomState.Hiding:
                // Уползаем под землю
                transform.Translate(Vector3.down * HidingSpeed * Time.fixedDeltaTime, Space.World);
                // Когда уползли на нужную глубину - перезарядка
                if (transform.position.y < (startY - HidingDepth))
                {
                    state = MushroomState.Reloading;
                    timer = ReloadingTimer;
                }
                break;
            case MushroomState.Reloading:
                // Выжидаем таймер перезарядки
                if (timer > 0)
                {
                    timer -= Time.fixedDeltaTime;
                    if (timer > 0)
                        AudioSystem("Mushroom_grow");
                }
                else // Когда истёк - поднимаемся до высоты Ready
                {
                    transform.Translate(Vector3.up * HidingSpeed * Time.fixedDeltaTime, Space.World);
                    if (transform.position.y > (startY - ReadyDepth))
                        state = MushroomState.Ready;
                }
                break;
            default:
                break;
        }
    }
}
