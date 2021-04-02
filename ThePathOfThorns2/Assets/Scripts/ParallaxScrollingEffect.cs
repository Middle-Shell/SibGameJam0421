using UnityEngine;

public class ParallaxScrollingEffect : MonoBehaviour
{
    [SerializeField] Transform cam = null;
    [SerializeField] float xAmount = 1f, yAmount = 0.2f;

    Vector2 newPos;
    float x0, y0, xDiff, yDiff;

    void Start()
    {
        x0 = cam.position.x;
        y0 = cam.position.y;
    }

    void Update()
    {
        xDiff = x0 + (cam.position.x * xAmount);
        yDiff = y0 + (cam.position.y * yAmount);

        Vector3 newPos = new Vector3(cam.position.x - xDiff, cam.position.y - yDiff , transform.position.z);
        transform.position = newPos;
    }
}
