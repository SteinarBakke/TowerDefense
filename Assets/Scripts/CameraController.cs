using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float scrollSpeed = 2f;
    public float panSpeed = 30f;
    public float panBorderThickness = 20f;
    public GameObject freeCamText;
    private bool freeCam = false;

    public float minY = 10f;
    public float maxY = 80f;


    private Vector3 defaultPosition;
    private Quaternion defaultRotation;


    void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameEnded)
        {
            this.enabled = false;
            return;
        }
        if (Input.GetKeyDown("c"))
        {
            freeCamText.SetActive(true);
            freeCam = !freeCam;
            if (!freeCam)
            {
                freeCamText.SetActive(false);
                transform.position = defaultPosition;
                transform.rotation = defaultRotation;
            }
        }
        if (!freeCam)
            return;

        //Since my Global is a little wrong, we're moving it like this
        //OR PHONE
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 pos = transform.position;
        pos.y -= scroll * 100 * scrollSpeed * Time.deltaTime;
        
        //restrict scro
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;

    }
}
