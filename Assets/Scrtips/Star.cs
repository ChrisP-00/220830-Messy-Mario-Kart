using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    bool isGameOver = false;

    private void OnTriggerEnter(Collider other)
    {

        if (isGameOver == false)
        {

            if (other.tag == "Player")
            {

                GameManager.Inst.AddPlayerScore(3);
                ItemPool.Inst.GotStar(this);
            }

            if (other.tag == "AI")
            {
                GameManager.Inst.AddAIScore(3);
                //Ghost 객체를 찾아서 메세지 보내기
                FindObjectOfType<Ghost>().SendMessage("HasItem");
                ItemPool.Inst.GotStar(this);
            }

        }
    }

    public void GG(bool gg)
    {
        isGameOver = gg;
        return;
    }





}
