using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingDoor : MonoBehaviour
{

    void FixedUpdate()
    {
        transform.Rotate(0, 1, 0, Space.Self);
    }
}
