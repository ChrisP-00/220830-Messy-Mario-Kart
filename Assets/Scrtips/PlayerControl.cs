using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerControl : MonoBehaviour
{
    [Header("Kart Info")]
    public float acceleration = 50;
    public float brakingForce = 1000f;
    public float maxTurnAngle = 45;

    private float curAcc;
    private float curTA;

    [Header("Wheel Collider Info")]
    [SerializeField] WheelCollider[] wheelCollider = new WheelCollider[4];

    [Header("Wheel Info")]
    [SerializeField] GameObject[] wheelMesh = new GameObject[4];


    [SerializeField] TrailRenderer[] trailRenderers;

    // Cinemachine의 transposer 접근할때 
    //[Header("Camera Info")]
    //[SerializeField] CinemachineVirtualCamera MyFollowCam;
    //CinemachineTransposer MyCam;

    Rigidbody playerRigid;

    private void Awake()
    {
        playerRigid = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {

        //wheel mesh와 wheel collider의 위치를 맞춰줌 
        for (int i = 0; i < wheelMesh.Length; i++)
        {
            wheelCollider[i].transform.position = wheelMesh[i].transform.position;
        }

        //무게 중심을 뒤쪽으로 이동 
        playerRigid.centerOfMass += new Vector3(0, -1, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
        //플레이어 입력 키 
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        //플레이어 움직임이 있나? 
        bool isSteer = (xAxis != 0f);
        bool isMoveForward = (zAxis != 0f);


        if (isSteer)     // 좌우 방향키 입력? 
        {
            curTA = maxTurnAngle * xAxis;

            // Front wheel steer
            wheelCollider[0].steerAngle = curTA;
            wheelCollider[1].steerAngle = curTA;

        }


        if (isMoveForward)      // 앞뒤 방향키 입력? 
        {
            curAcc = acceleration * zAxis;      // 방향 * 속도 


            // Front 2WD 
            wheelCollider[0].motorTorque = curAcc;
            wheelCollider[1].motorTorque = curAcc;

            // Rear 2WD 
            wheelCollider[2].motorTorque = curAcc;
            wheelCollider[3].motorTorque = curAcc;
        }


        //Wheel collider가 움직임에 따라 wheel의 움직임
        for (int i = 0; i < wheelCollider.Length; i++)
        {
            UpdateWheel(wheelCollider[i], wheelMesh[i].transform);
        }

        // 스페이스키를 누를 때 브레이크
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스 입력 =>  브레이크 
        {
            for (int i = 2; i < wheelMesh.Length; i++)     // Rear wheel  브레이크 
            {
                wheelCollider[i].brakeTorque = brakingForce;
            }

            // skid mark 생성
            StartDrift();
        }


        // 스페이스 키를 땔 때 브레이크 해제 
        if (Input.GetKeyUp(KeyCode.Space))
        {

            for (int i = 0; i < wheelMesh.Length; i++)     // 브레이크 해제 
            {
                wheelCollider[i].brakeTorque = 0f;
            }

            // skid mark 없애기 
            EndDrift();
        }

        // 땅에 땋지 않은 타이어의 skid mark 없애기 
        Goway();
    }

    //Wheel Collider가 움직임에 따라 Wheel도 같이 움직이게 하기 위한 함수
    void UpdateWheel(WheelCollider colli, Transform trans)
    {
        //Wheel collider의 정보를 가져옴 
        Vector3 position;
        Quaternion rotation;
        colli.GetWorldPose(out position, out rotation);

        //wheel 움직임 
        trans.position = position;
        trans.rotation = rotation;
    }


    // 드리프트를 하게 되면 타이어에 있는 trailrenderer 표시 
    public void StartDrift()
    {

        for (int i = 0; i < wheelCollider.Length; i++)
        {
            // 해당 wheel이 grounded인지 ? 
            if (wheelCollider[i].isGrounded)
            { trailRenderers[i].emitting = true; }  // ground만 skid mark 생성
        }
    }


    // skid mark 없애기 
    public void EndDrift()
    {

        foreach (TrailRenderer T in trailRenderers)
        {
            T.emitting = false;
        }

    }


    // 땅에 땋지 않은 타이어의 skid mark 없애기 
    public void Goway()
    {

        for (int i = 0; i < wheelCollider.Length; i++)
        {
            // 해당 wheel이 grounded인지 ? 
            if (wheelCollider[i].isGrounded == false)
            { trailRenderers[i].emitting = false; }  // ground만 skid mark 생성
        }
    }


}