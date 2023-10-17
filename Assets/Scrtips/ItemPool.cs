using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Item Pooling�� ���� ���ִ� item�� Ȱ��ȭ ��Ȱ��ȭ�� ����
// Coint & Item�� �̸� ��Ƶΰ� �÷����̾ ȹ�� �� ��Ȱ��ȭ�� ��
// pooling�� ���� �� ���� �ð����� �ٽ� Ȱ��ȭ �Ѵ�. 

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
            { inst = FindObjectOfType<ItemPool>(); }    // item pool�� null�̸� itempool�� ã�´�. 


            if (inst == null)          // ã�Ƶ� ������ ItemPool�̶�� �̸����� ���� �� Component�߰� 
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

        // 30�� �Ŀ� �� ���� 
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

        // 30�� �Ŀ� �� ���� 
        yield return new WaitForSeconds(60f);

        Star star = StarPool.Dequeue();
        star.transform.parent = null;
        star.gameObject.SetActive(true);

        yield return null;

    }

}
