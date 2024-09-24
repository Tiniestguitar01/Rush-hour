using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopVehicle : MonoBehaviour
{
    public float wait = 30f;

    public GameObject redPrefab;

    public GameObject red;

    public bool stopped = false;

    void Start()
    {
        InstantiateRed(redPrefab);
        StartCoroutine(Stop());
    }

    public void InstantiateRed(GameObject redPrefab)
    {
        red = Instantiate(redPrefab, transform.position, Quaternion.identity);
        red.SetActive(false);
    }

    public IEnumerator Stop()
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
