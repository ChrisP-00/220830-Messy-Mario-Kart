using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Item Pooling은 생성 되있는 item의 활성화 비활성화를 관리
// Coint & Item을 미리 깔아두고 플레이이어가 획득 시 비활성화한 후
// pooling에 넣은 뒤 일정 시간마다 다시 활성화 한다. 

public class ItemPool : MonoBehaviour
{
    //-------------------------------------------------------
    #region Singleton

    static ItemPool inst = null;

    public static ItemPool Inst
    {
        get
        {
            if (inst == null)
            { inst = FindObjectOfType<ItemPool>(); }    // item pool이 null이면 itempool을 찾는다. 


            if (inst == null)          // 찾아도 없으면 ItemPool이라는 이름으로 생성 후 Component추가 
            { inst = new GameObject("ItemPool").AddComponent<ItemPool>(); }

            return inst;
        }
    }

    #endregion
    //-------------------------------------------------------


    Queue<Coin> itemPool = new Queue<Coin>();
    Queue<Star> StarPool = new Queue<Star>();

    //Object Pooling 

    public void Gotcha(Coin item)
    {
        StartCoroutine(Got(item));
    }


    IEnumerator Got(Coin get)
    {

        get.transform.parent = this.transform;
        get.gameObject.SetActive(false);

        itemPool.Enqueue(get);

        // 30초 후에 재 생성 
        yield return new WaitForSeconds(30f);

        Coin coin = itemPool.Dequeue();
        coin.transform.parent = null;
        coin.gameObject.SetActive(true);

        yield return null;
    
    }


    public void GotStar(Star item)
    {
        StartCoroutine(Star(item));
    }

    IEnumerator Star(Star get)
    {

        get.transform.parent = this.transform;
        get.gameObject.SetActive(false);

        StarPool.Enqueue(get);

        // 30초 후에 재 생성 
        yield return new WaitForSeconds(60f);

        Star star = StarPool.Dequeue();
        star.transform.parent = null;
        star.gameObject.SetActive(true);

        yield return null;

    }

}
