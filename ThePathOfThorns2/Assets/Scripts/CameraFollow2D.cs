using UnityEngine;
using UnityEngine.UI;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] float damping = 1.5f;
    [SerializeField] Vector2 offset = new Vector2(2f, 1f);
    [SerializeField] bool faceLeft;
    private Transform player;
    private int lastX;

    [SerializeField] float newSize;
    bool sizeIsChanged;

    [Space]
    [SerializeField] Vector2 minCord;
    [SerializeField] Vector2 maxCord;

    public int numCristals;
    AudioSource audioSrc;
    [SerializeField] Text txt;

    

    void Start()
    {
        numCristals = 0;
       // txt.text = "";
        
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(faceLeft);
        
    }

    public void FindPlayer(bool playerFaceLeft)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastX = Mathf.RoundToInt(player.position.x);

        Vector2 pp = player.position;
    }

    [Header("Parameters")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private string playerTag;
    [SerializeField] [Range(0.5f, 7.5f)] private float movingSpeed = 1.5f;

    private void Awake()
    {
        if (this.playerTransform == null)
        {
            if (this.playerTag == "")
            {
                this.playerTag = "Player";
            }

            this.playerTransform = GameObject.FindGameObjectWithTag(this.playerTag).transform;
        }

        this.transform.position = new Vector3()
        {
            x = this.playerTransform.position.x,
            y = this.playerTransform.position.y,
            z = this.playerTransform.position.z - 500,
        };
        

    }

private void Update()
    {
        if (this.playerTransform)
        {
            Vector3 target = new Vector3()
            {
                x = this.playerTransform.position.x,
                y = this.playerTransform.position.y + 3f,
                z = this.playerTransform.position.z - 500,
            };

            Vector3 pos = Vector3.Lerp(this.transform.position, target, this.movingSpeed * Time.deltaTime);

            this.transform.position = pos;
        }
    }


    public void ChangeSize(float _newSize)
    {
        sizeIsChanged = true;
        newSize = _newSize;
    }

    public void addCreistals()
    {
        numCristals += 1;

        txt.text = numCristals.ToString();
    }
}