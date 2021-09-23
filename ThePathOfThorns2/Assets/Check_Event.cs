using UnityEngine;
using System.Collections;

public class Check_Event : MonoBehaviour
{
    // public SkeletonAnimation skeletonAnimation;
    public DragonBones.UnityArmatureComponent anim;
    public static Check_Event Instance;
    
    public void CheckStatus(float bonuses)
    {
        Debug.Log("Win");
        //skeletonAnimation = GetComponent<SkeletonAnimation>();
        anim = GetComponent<DragonBones.UnityArmatureComponent>();
        if (bonuses >= 3)
        {

            anim.animation.Play("statick");
            //skeletonAnimation.AnimationName = "statick";
        }
    }
    
    }
