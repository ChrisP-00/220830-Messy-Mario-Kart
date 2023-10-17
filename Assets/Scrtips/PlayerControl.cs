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

    // Cinemachine�� transposer �����Ҷ� 
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

        //wheel mesh�� wheel collider�� ��ġ�� ������ 
        for (int i = 0; i < wheelMesh.Length; i++)
        {
            wheelCollider[i].transform.position = wheelMesh[i].transform.position;
        }

        //���� �߽��� �������� �̵� 
        playerRigid.centerOfMass += new Vector3(0, -1, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        
        //�÷��̾� �Է� Ű 
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        //�÷��̾� �������� �ֳ�? 
        bool isSteer = (xAxis != 0f);
        bool isMoveForward = (zAxis != 0f);


        if (isSteer)     // �¿� ����Ű �Է�? 
        {
            curTA = maxTurnAngle * xAxis;

            // Front wheel steer
            wheelCollider[0].steerAngle = curTA;
            wheelCollider[1].steerAngle = curTA;

        }


        if (isMoveForward)      // �յ� ����Ű �Է�? 
        {
            curAcc = acceleration * zAxis;      // ���� * �ӵ� 


            // Front 2WD 
            wheelCollider[0].motorTorque = curAcc;
            wheelCollider[1].motorTorque = curAcc;

            // Rear 2WD 
            wheelCollider[2].motorTorque = curAcc;
            wheelCollider[3].motorTorque = curAcc;
        }


        //Wheel collider�� �����ӿ� ���� wheel�� ������
        for (int i = 0; i < wheelCollider.Length; i++)
        {
            UpdateWheel(wheelCollider[i], wheelMesh[i].transform);
        }

        // �����̽�Ű�� ���� �� �극��ũ
        if (Input.GetKeyDown(KeyCode.Space)) // �����̽� �Է� =>  �극��ũ 
        {
            for (int i = 2; i < wheelMesh.Length; i++)     // Rear wheel  �극��ũ 
            {
                wheelCollider[i].brakeTorque = brakingForce;
            }

            // skid mark ����
            StartDrift();
        }


        // �����̽� Ű�� �� �� �극��ũ ���� 
        if (Input.GetKeyUp(KeyCode.Space))
        {

            for (int i = 0; i < wheelMesh.Length; i++)     // �극��ũ ���� 
            {
                wheelCollider[i].brakeTorque = 0f;
            }

            // skid mark ���ֱ� 
            EndDrift();
        }

        // ���� ���� ���� Ÿ�̾��� skid mark ���ֱ� 
        Goway();
    }

    //Wheel Collider�� �����ӿ� ���� Wheel�� ���� �����̰� �ϱ� ���� �Լ�
    void UpdateWheel(WheelCollider colli, Transform trans)
    {
        //Wheel collider�� ������ ������ 
        Vector3 position;
        Quaternion rotation;
        colli.GetWorldPose(out position, out rotation);

        //wheel ������ 
        trans.position = position;
        trans.rotation = rotation;
    }


    // �帮��Ʈ�� �ϰ� �Ǹ� Ÿ�̾ �ִ� trailrenderer ǥ�� 
    public void StartDrift()
    {

        for (int i = 0; i < wheelCollider.Length; i++)
        {
            // �ش� wheel�� grounded���� ? 
            if (wheelCollider[i].isGrounded)
            { trailRenderers[i].emitting = true; }  // ground�� skid mark ����
        }
    }


    // skid mark ���ֱ� 
    public void EndDrift()
    {

        foreach (TrailRenderer T in trailRenderers)
        {
            T.emitting = false;
        }

    }


    // ���� ���� ���� Ÿ�̾��� skid mark ���ֱ� 
    public void Goway()
    {

        for (int i = 0; i < wheelCollider.Length; i++)
        {
            // �ش� wheel�� grounded���� ? 
            if (wheelCollider[i].isGrounded == false)
            { trailRenderers[i].emitting = false; }  // ground�� skid mark ����
        }
    }


}