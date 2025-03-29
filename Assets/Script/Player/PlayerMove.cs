using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 2f;
    public float rotationSpeed = 10f; // 회전 보간 속도

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 입력값 처리
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(x, 0, y);

        // 대각선 입력 시 속도가 증가하는 것을 방지
        if (inputDir.magnitude > 1)
            inputDir.Normalize();

        // 속도 업데이트
        rb.velocity = inputDir * speed;

        // 입력이 있을 때만 회전 업데이트 (입력이 없으면 기존 회전값 유지)
        if (inputDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
