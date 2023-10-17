using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    bool isGameOver = false;

    private void OnTriggerEnter(Collider other)
    {

        if (isGameOver == false)
        {

            if (other.tag == "Player")
            {
                GameManager.Inst.AddPlayerScore(1);
                ItemPool.Inst.Gotcha(this);
            }

            if (other.tag == "AI")
            {
                GameManager.Inst.AddAIScore(1);
                ItemPool.Inst.Gotcha(this);
            }

        }
    }

    public void GG(bool gg)
    {
        isGameOver = gg;
        return;
    }





}
