using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2f;
    public float rotationSpeed = 10f; // ȸ�� ���� �ӵ�

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �Է°� ó��
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(x, 0, y);

        // �밢�� �Է� �� �ӵ��� �����ϴ� ���� ����
        if (inputDir.magnitude > 1)
            inputDir.Normalize();

        // �ӵ� ������Ʈ
        rb.velocity = inputDir * speed;

        // �Է��� ���� ���� ȸ�� ������Ʈ (�Է��� ������ ���� ȸ���� ����)
        if (inputDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
