using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    public float scrollSpeed = 5f;
    public float scrollEdge = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.x <= Screen.width * scrollEdge)
        {
            pos.x -= scrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width * (1 - scrollEdge))
        {
            pos.x += scrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= Screen.height * scrollEdge)
        {
            pos.z -= scrollSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y >= Screen.height * (1 - scrollEdge))
        {
            pos.z += scrollSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
