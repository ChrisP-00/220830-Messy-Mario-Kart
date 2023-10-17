using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{

    public LayerMask Gotcha;   // Ÿ�� ���̾� - items or coins

    public float range = 10000; // searching range 

    GameObject targetObject; // �߰� object
    Transform target;  // �߰� ��� transform

    Transform[] targets;  // �߰� ������ transform

    NavMeshAgent navMeshAgent;  // ��� ��� AI Agent


    bool IsGameOver = false;     // ���� ���� ���ΰ�? 
    bool hasItem = false;       // ������ ������ �ֳ�? 



    public void GameOver()
    {
        IsGameOver = true;
        StopCoroutine(UpdatePath());
        return;
    }


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        //�÷��̾� gameObject�� ������ 
        targetObject = GameObject.FindWithTag("Player");

        StartCoroutine(UpdatePath());
    }


    private void Update()
    {
        //��Ʈ�� offMeshLink�� �������� ��� 
        if (navMeshAgent.isOnOffMeshLink)
        {
            // data������ ���� offmeshdata �Ҵ� 
            //offMeshLinkData�� offmeshlink�� ���� �����͸� �����ü��ִ�. 
            // activated, endPos, startPos, ��� 

            OffMeshLinkData data = navMeshAgent.currentOffMeshLinkData;

            // ���� ��ġ�� offmeshlink�� endPos�� navmeshagent�� ������ offset�� ���ؾ� �Ѵ�. 
            Vector3 landingPos = data.endPos + Vector3.up * navMeshAgent.baseOffset;


            //vector3.MoveTowards �� ��ġ���� target��ġ���� �� �����Ӹ��� �ӵ���ŭ ������ 
            navMeshAgent.transform.position = Vector3.MoveTowards(transform.position, landingPos, navMeshAgent.speed * Time.deltaTime);

            float jumpdistance = Vector3.Distance(data.startPos, data.endPos);
            float whereMi = Vector3.Distance(transform.position, data.endPos);


            // �� ��ġ�� ���� �Ÿ��� �ݺ��� ū ��� 
            if (whereMi > jumpdistance / 2)
            {
                navMeshAgent.transform.Translate(Vector3.up * 5f * Time.deltaTime);
            }


            // �� ��ġ�� ���� �Ÿ��� �ݺ��� ���� ��� 
            if (whereMi < jumpdistance / 2 && whereMi >= 0)
            {
                navMeshAgent.transform.Translate(Vector3.up * -2f * Time.deltaTime);
            }

            //NavMeshAgent�� landingpos�� ����� ���
            if (Vector3.Distance(navMeshAgent.transform.position, landingPos) <= 1)
            {
                // ���� offmeshlink������ �������� �Ϸ��ϰ� ���� navmesh point�� �̵��Ѵ�
                navMeshAgent.CompleteOffMeshLink();
            }

        }

    }

    // ��Ʈ�� �������� ������ true;
    public void HasItem()
    {
        hasItem = true;
    }

    // ��Ʈ�� �÷��̾�� �����ϸ� hasitem�� false�� 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        { hasItem = false; }
    }


    // ����� ���� Ȥ�� �������� ã�´� -> 
    // ������ ���� �� -> �÷��̾ ã�´�. 
    private IEnumerator UpdatePath()
    {
        while (IsGameOver != true)
        {


            // �ʿ��� ������ 
            // ���� ����� object�� ��ġ ����
            Transform closestTarget = null;

            // path�� ���� ũ�� ������ ǥ���� �� �ִ� �ִ� ���� ������ �� target�� �� �ϱ� ����
            float closestTargetDistance = float.MaxValue;


            //���� Collider �� Gotcha layer�� ���� �͸� �����´�. 
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, Gotcha);


            //// ã�� colliders
            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    Debug.Log("## ���� ã�� colliders " + colliders[i].name);
            //}


            //������ Gotcha �� ���� ����� Gotcha path ��� 

            // navigation system���� ���Ǵ� path 
            // object������ waypoint�� corner �迭�� ���� �� 

            NavMeshPath path = new NavMeshPath();

            //ã�ƿ� collider�� transform�� ���� 
            for (int i = 0; i < colliders.Length; i++)
            {

                //Debug.Log("## Collider �̸� ��ġ! " + colliders[i].name + colliders[i].transform.position);

                // ���� �� ��ġ���� �迭 ù��° collider�� ��ġ������ path 
                // true => path, false => no path 
                if (NavMesh.CalculatePath(transform.position, colliders[i].transform.position, navMeshAgent.areaMask, path))
                {
                    // ù��° collider�� path�� ù���� corner������ �Ÿ� ��� 
                    // path�� target������ waypoint�� corner �迭�� ����� 
                    float distance = Vector3.Distance(transform.position, path.corners[0]);

                    //Debug.Log("ù��° �ڳʱ��� �Ÿ�! " + colliders[i].name + distance);


                    // ���� corner�� �ִٸ� for�� ����, ������ ù�ڳʿ��� ��
                    for (int j = 1; j < path.corners.Length; j++)
                    {
                        // �� �ڳʿ� �� �ڳʿ��� �Ÿ��� ����Ͽ� distance�� ����
                        distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                    }


                    //collier���� �Ÿ��� closest target �Ÿ����� ª���� closest target�� ���� collider�� ���� 
                    if (distance < closestTargetDistance)
                    {
                        closestTargetDistance = distance;
                        closestTarget = colliders[i].transform;

                        //Debug.Log("### ���� ª�� �Ÿ�!!! " + closestTargetDistance);

                    }
                }

                // corner�� ���� path�� �Ÿ��� �ִܰŸ� ��
                else
                {
                    float distance = float.MaxValue;
                    distance = Vector3.Distance(transform.position, colliders[i].transform.position);

                    if (distance < closestTargetDistance)
                    {
                        closestTargetDistance = distance;
                        closestTarget = colliders[i].transform;
                    }
                }
            }

            // �������� ���� ���� �ʾ����� ����� �����̳� ���������� �̵� 
            if (hasItem == false)
            { target = closestTarget; }


            //�������� ������ ������ �÷��̾�� �̵� 
            if (hasItem)
            { target = targetObject.transform; }

            navMeshAgent.SetDestination(target.position); // Ÿ���� pos�� �̵� 
            yield return new WaitForSeconds(0.25f);

        }

        yield return null;

    }
}
