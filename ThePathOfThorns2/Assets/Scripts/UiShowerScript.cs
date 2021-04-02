using UnityEngine;

public class UiShowerScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer UiSprite;
    float transp = 0;
    
    void Update()
    {
        if(UiSprite.color.a != transp)
        {
            UiSprite.color = new Color(UiSprite.color.r, UiSprite.color.g, UiSprite.color.b,
                                    Mathf.Lerp(UiSprite.color.a, transp, Time.deltaTime * 10));

            if (Mathf.Abs(transp - UiSprite.color.a) < .1f)
            {
                UiSprite.color = new Color(UiSprite.color.r, UiSprite.color.g, UiSprite.color.b, transp);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transp = 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transp = 0;
    }
}
