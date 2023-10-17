using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMario : MonoBehaviour
{
    Vector3 temp;

    private void Awake()
    {
        temp = transform.position;
    }


    void Update()
    {
        transform.Translate(Vector3.forward * 4 * Time.deltaTime);

        if (transform.position.x > 15)
        { transform.position = temp; }
    
    }
}
