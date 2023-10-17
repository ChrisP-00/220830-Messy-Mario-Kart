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
                //Ghost ��ü�� ã�Ƽ� �޼��� ������
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
