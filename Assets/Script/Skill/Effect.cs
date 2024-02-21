using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    FireBall fireball;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = fireball.transform.position;
    }
}
