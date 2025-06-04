using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScrinSetting : MonoBehaviour
{
    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);

        Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, true);
    }
}
