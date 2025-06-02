using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCam : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam == null) return;

        // 카메라의 방향을 바라보도록 회전
        transform.rotation = Quaternion.LookRotation(mainCam.transform.forward, mainCam.transform.up);
    }
}
