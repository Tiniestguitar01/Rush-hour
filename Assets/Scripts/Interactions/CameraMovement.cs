using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Transform rotation;

    public float speed = 5f;
    public float scrollScale = 10f;

    public float cameraYMax = 90;
    public float cameraYMin = 0;

    public float cameraZMax = -15;
    public float cameraZMin = -100;

    float rotationY = 0f;

    void LateUpdate()
    {
        transform.LookAt(target);
        if (InstanceCreator.GetGameData().state == (int)Menu.Game)
        {
            if (Input.GetMouseButton(1))
            {
                float VerticalAxis = -Input.GetAxis("Mouse Y") * speed;

                float rotationX = rotation.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * speed;

                rotationY += -Input.GetAxis("Mouse Y") * speed;
                rotationY = Mathf.Clamp(rotationY, cameraYMin, cameraYMax);

                rotation.transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }

            if (Input.mouseScrollDelta.y != 0) 
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Mathf.Clamp(transform.localPosition.z + Input.mouseScrollDelta.y * scrollScale, cameraZMin, cameraZMax));
            }
        }
        else
        {
            rotation.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        }
    }   
}
