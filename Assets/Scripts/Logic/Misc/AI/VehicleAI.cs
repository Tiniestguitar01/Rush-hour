using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class VehicleAI : MonoBehaviour
{
    public float step = 10f;
    public float speed = 1f;

    public bool stopped = false;
    Vector3 startPosition;

    public float distanceToDestroy = 200f;

    private void Start()
    {
        startPosition = transform.position;

        speed = Random.Range(1f,1.5f);
    }

    void Update()
    {
        if (stopped == false)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + transform.right * step, Time.deltaTime * speed);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 5f))
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
    }
}
