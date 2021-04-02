using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
	public float HP = 1;//hp противника

    void Update()
    {
		if (HP <= 0)
		{
			Destroy(gameObject);//черновой вариант, чисто что бы было наглядно
		}
	}
}
