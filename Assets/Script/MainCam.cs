
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float Zoom = 3000f;
    Vector3 offset;
    float smoothTime = 0.3f; // 스무딩에 사용할 시간
    public float scrollSpeed = 5f;
    public float scrollEdge = 0.01f;

    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update

    private void Awake()
    {
        offset = transform.position - player.position;
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if(player != null)
        {
            Vector3 targetPosition = player.position + offset;
            Vector3 Pos = new Vector3(targetPosition.x, 21.5f, targetPosition.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            transform.position = Pos;
            float ScroollWheel = Input.GetAxis("Mouse ScrollWheel");
            Camera.main.fieldOfView -= ScroollWheel * Time.deltaTime * Zoom;
            if (Camera.main.fieldOfView > 50)
            {
                Camera.main.fieldOfView = 50;
            }
            else if (Camera.main.fieldOfView < 25)
            {
                Camera.main.fieldOfView = 25;
            }
        }
    }
}
 