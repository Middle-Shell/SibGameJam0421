using UnityEngine;

public class doorOpener : MonoBehaviour
{
    [SerializeField] float posX;

    public float op;
    float defaultX;

    private void Start()
    {
        defaultX = transform.position.x;
        op = defaultX;
    }

    void Update()
    {
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, op, Time.deltaTime * 10), transform.position.y,
            transform.position.z);
    }

    public void Open()
    {
        op = defaultX + posX;
    }

    public void Close()
    {
        op = defaultX;
    }
}
