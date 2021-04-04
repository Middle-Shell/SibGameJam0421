using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayZer : MonoBehaviour, IBoolianState
{
    public bool State => IsGayZering;
    public bool IsGayZering = false;

    [SerializeField] private float idleTickTime = 1;
    private float idleTimeRemaining = 0;

    [SerializeField] private float GayZeringTickTime = 1;
    private float GayZeringTimeRemaining = 0;

    private GameObject gameObjectInTrigger = null;
    private bool alreadyThrowed = false;

    private Collider2D myCollider = null;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        idleTimeRemaining += Time.deltaTime;

        if (IsGayZering)
        {
            GayZeringTimeRemaining += Time.deltaTime;
            DoGayZering();
        }

        if (idleTimeRemaining >= idleTickTime)
        {
            ResetGayZering();

            idleTimeRemaining = 0;
            IsGayZering = true;
        }
        if (GayZeringTimeRemaining >= GayZeringTickTime)
        {
            GayZeringTimeRemaining = 0;
            IsGayZering = false;
        }
    }

    private void DoGayZering()
    {
        if (gameObjectInTrigger != null && !alreadyThrowed)
        {
            float height = myCollider == null ? 10 : myCollider.bounds.max.y - gameObjectInTrigger.GetComponent<Collider2D>().bounds.min.y * 1.3f;

            if (height > 1)
            {
                alreadyThrowed = true;
                gameObjectInTrigger.GetComponent<Rigidbody2D>().velocity = Vector2.up * Mathf.Sqrt(2 * -Physics2D.gravity.y * height);
            }
        }
    }

    private void ResetGayZering()
    {
        alreadyThrowed = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            gameObjectInTrigger = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            gameObjectInTrigger = null;
            ResetGayZering();
        }
    }
}

public interface IInteractable
{

}

public interface IBoolianState
{
    bool State { get; }
}