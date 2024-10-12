using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class VehicleAI : MonoBehaviour
{
    public float step = 10f;
    public float speed = 1f;
    float checkDistance = 5.5f;

    public bool stopped = false;
    Vector3 startPosition;

    float distanceToDestroy = 400f;

    float timeSinceStop = 0;

    public float timeUntilDestroy = 30f;

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (stopped == false)
        {
            timeSinceStop = 0;
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * step, Time.deltaTime * speed);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, checkDistance))
        {
            if(hit.transform.tag == "car" || hit.transform.tag == "red")
            {
                stopped = true;
            }
            else
            {
                stopped = false;
            }
        }
        else
        {
            stopped = false;
        }

        if (Vector3.Distance(transform.position, startPosition) > distanceToDestroy)
        {
            Destroy(gameObject);
        }

        if(stopped == true)
        {
            timeSinceStop += Time.deltaTime;
            if (timeSinceStop > timeUntilDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
