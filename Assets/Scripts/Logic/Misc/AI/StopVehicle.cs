using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopVehicle : MonoBehaviour
{
    public float wait = 30f;
    BoxCollider boxCollider;

    public GameObject redLight;
    public GameObject greenLight;

    public bool stopped = false;

    void Start()
    {
        redLight.SetActive(false);
        greenLight.SetActive(false);
        boxCollider = GetComponent<BoxCollider>();
        StartCoroutine(Stop());
    }

    public IEnumerator Stop()
    {
        if(stopped)
        {
            boxCollider.enabled = false;
            redLight.SetActive(false);
            greenLight.SetActive(true);
            stopped = false;
        }
        else
        {
            boxCollider.enabled = true;
            redLight.SetActive(true);
            greenLight.SetActive(false);
            stopped = true;
        }
        yield return new WaitForSeconds(wait);
        StartCoroutine(Stop());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(5f,5f,5f));
    }
}
