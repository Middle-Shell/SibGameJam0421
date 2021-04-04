using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build.Player;

public class GroundTrigger : MonoBehaviour
{
    [SerializeField]
    private MovePlayer player;
    [SerializeField]
    private LayerMask groundLayers;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(1 << other.gameObject.layer == groundLayers)
        {
            //player.isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (1 << other.gameObject.layer == groundLayers)
        {
            //player.isGrounded = false;
        }
    }
}
