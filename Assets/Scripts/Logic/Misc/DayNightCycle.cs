using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float time;
    public float scale = 2f;
    public GameObject Light;


    void Update()
    {
        if(time < 360 * scale)
        {
            time += Time.deltaTime;
        }
        else
        {
            time = 0;
        }
        Light.transform.eulerAngles = new Vector3(time / scale, 0, 0);
    }
}
