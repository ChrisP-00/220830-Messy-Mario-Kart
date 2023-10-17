using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{

    public LayerMask Gotcha;   // 타겟 레이어 - items or coins

    public float range = 10000; // searching range 

    GameObject targetObject; // 추격 object
    Transform target;  // 추격 대상 transform

    Transform[] targets;  // 추격 대상들의 transform

    NavMeshAgent navMeshAgent;  // 경로 계산 AI Agent


    bool IsGameOver = false;     // 게임 진행 중인가? 
    bool hasItem = false;       // 아이템 가지고 있나? 



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
        //플레이어 gameObject를 가져옴 
        targetObject = GameObject.FindWithTag("Player");

        StartCoroutine(UpdatePath());
    }


    private void Update()
    {
        //고스트가 offMeshLink에 도착했을 경우 
        if (navMeshAgent.isOnOffMeshLink)
        {
            // data변수에 현재 offmeshdata 할당 
            //offMeshLinkData는 offmeshlink에 대한 데이터를 가져올수있다. 
            // activated, endPos, startPos, 등등 

            OffMeshLinkData data = navMeshAgent.currentOffMeshLinkData;

            // 착지 위치는 offmeshlink의 endPos에 navmeshagent의 수직축 offset을 합해야 한다. 
            Vector3 landingPos = data.endPos + Vector3.up * navMeshAgent.baseOffset;


            //vector3.MoveTowards 내 위치에서 target위치까지 매 프레임마다 속도만큼 움직임 
            navMeshAgent.transform.position = Vector3.MoveTowards(transform.position, landingPos, navMeshAgent.speed * Time.deltaTime);

            float jumpdistance = Vector3.Distance(data.startPos, data.endPos);
            float whereMi = Vector3.Distance(transform.position, data.endPos);


            // 내 위치가 점프 거리의 반보다 큰 경우 
            if (whereMi > jumpdistance / 2)
            {
                navMeshAgent.transform.Translate(Vector3.up * 5f * Time.deltaTime);
            }


            // 내 위치가 점프 거리의 반보다 작을 경우 
            if (whereMi < jumpdistance / 2 && whereMi >= 0)
            {
                navMeshAgent.transform.Translate(Vector3.up * -2f * Time.deltaTime);
            }

            //NavMeshAgent가 landingpos에 닿았을 경우
            if (Vector3.Distance(navMeshAgent.transform.position, landingPos) <= 1)
            {
                // 현재 offmeshlink에서의 움직임을 완료하고 다음 navmesh point로 이동한다
                navMeshAgent.CompleteOffMeshLink();
            }

        }

    }

    // 고스트가 아이템이 있으면 true;
    public void HasItem()
    {
        hasItem = true;
    }

    // 고스트가 플레이어와 접촉하면 hasitem을 false로 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        { hasItem = false; }
    }


    // 가까운 코인 혹은 아이템을 찾는다 -> 
    // 아이템 보유 시 -> 플레이어를 찾는다. 
    private IEnumerator UpdatePath()
    {
        while (IsGameOver != true)
        {


            // 필요한 변수들 
            // 가장 가까운 object의 위치 정보
            Transform closestTarget = null;

            // path의 값이 크기 때문에 표현할 수 있는 최대 수로 설정한 후 target과 비교 하기 위함
            float closestTargetDistance = float.MaxValue;


            //닿은 Collider 중 Gotcha layer를 가진 것만 가져온다. 
            Collider[] colliders = Physics.OverlapSphere(transform.position, range, Gotcha);


            //// 찾은 colliders
            //for (int i = 0; i < colliders.Length; i++)
            //{
            //    Debug.Log("## 내가 찾은 colliders " + colliders[i].name);
            //}


            //가져온 Gotcha 중 제일 가까운 Gotcha path 계산 

            // navigation system으로 계산되는 path 
            // object까지의 waypoint가 corner 배열로 저장 됨 

            NavMeshPath path = new NavMeshPath();

            //찾아온 collider의 transform을 저장 
            for (int i = 0; i < colliders.Length; i++)
            {

                //Debug.Log("## Collider 이름 위치! " + colliders[i].name + colliders[i].transform.position);

                // 현재 내 위치부터 배열 첫번째 collider의 위치까지의 path 
                // true => path, false => no path 
                if (NavMesh.CalculatePath(transform.position, colliders[i].transform.position, navMeshAgent.areaMask, path))
                {
                    // 첫번째 collider의 path의 첫번쨰 corner까지의 거리 계산 
                    // path는 target까지의 waypoint가 corner 배열에 저장됨 
                    float distance = Vector3.Distance(transform.position, path.corners[0]);

                    //Debug.Log("첫번째 코너까지 거리! " + colliders[i].name + distance);


                    // 다음 corner가 있다면 for문 입장, 없으면 첫코너에서 끝
                    for (int j = 1; j < path.corners.Length; j++)
                    {
                        // 앞 코너와 뒷 코너와의 거리를 계산하여 distance에 저장
                        distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                    }


                    //collier와의 거리가 closest target 거리보다 짧으면 closest target을 현재 collider로 변경 
                    if (distance < closestTargetDistance)
                    {
                        closestTargetDistance = distance;
                        closestTarget = colliders[i].transform;

                        //Debug.Log("### 제일 짧은 거리!!! " + closestTargetDistance);

                    }
                }

                // corner가 없는 path의 거리와 최단거리 비교
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

            // 아이템을 소지 하지 않았으면 가까운 코인이나 아이템으로 이동 
            if (hasItem == false)
            { target = closestTarget; }


            //아이템을 가지고 있으면 플레이어에게 이동 
            if (hasItem)
            { target = targetObject.transform; }

            navMeshAgent.SetDestination(target.position); // 타겟의 pos로 이동 
            yield return new WaitForSeconds(0.25f);

        }

        yield return null;

    }
}
