using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopVehicle : MonoBehaviour
{
    public float wait = 30f;

    public GameObject redPrefab;

    GameObject red;

    public bool stopped = false;

    void Start()
    {
        red = Instantiate(redPrefab, transform.position, Quaternion.identity);
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        if(stopped)
        {
            red.SetActive(false);
            stopped = false;
        }
        else
        {
            red.SetActive(true);
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
