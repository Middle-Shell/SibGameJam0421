using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    private Collider2D myCollider = null;
    private float floorDistance = 0.1f;

    private bool IsGroundeds => myCollider != null ? Physics2D.Raycast(myCollider.bounds.min, Vector2.down, floorDistance) : false;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }
}
