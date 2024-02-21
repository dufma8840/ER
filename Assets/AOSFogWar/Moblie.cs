using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moblie : MonoBehaviour
{
    public Material mat;
    void Start()
    {
        mat = new Material(Shader.Find("Assets/AOSFogWar/Shaders"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
