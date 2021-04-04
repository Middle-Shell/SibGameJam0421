using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GayZerVisual : MonoBehaviour
{
    [SerializeField] private GameObject SelectedObject = null;
    private IBoolianState stateble = null;


    //
    private SpriteRenderer mySpriteRenderer = null;
    //

    private void Start()
    {
        stateble = SelectedObject.GetComponent<IBoolianState>();

        //
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        //
    }
    

    void Update()
    {
        if (stateble != null)
        {
            if (stateble.State)
            {
                //
                mySpriteRenderer.enabled = true;
                //
            }
            else
            {
                //
                mySpriteRenderer.enabled = false;
                //
            }
        }
    }
}
