using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOnGND : MonoBehaviour
{

    GameObject myCar;

    private void OnCollisionEnter(Collision colli)
    {
        if (colli.collider.tag == "Ground")
        {
            Debug.Log("## I'm on the ground~! ");

            myCar = GameObject.FindWithTag("Player");
            myCar.SendMessage("OnGround");

        }

    }    

    private void OnCollisionExit(Collision colli)
    {

        if (colli.collider.tag == "Ground")
        {

            Debug.Log("## I'm flying~! ");

            myCar = GameObject.FindWithTag("Player");
            myCar.SendMessage("OnAir");

        }
    }



}
