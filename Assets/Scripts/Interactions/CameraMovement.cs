using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Transform rotation;

    public float speed = 5f;

    public float cameraYMax = 90;
    public float cameraYMin = 0;

    float rotationY = 0f;

    void LateUpdate()
    {
        transform.LookAt(target);
        if(UIManager.Instance.state == (int)Menu.Game)
        {
            if (Input.GetMouseButton(1))
            {
                float VerticalAxis = -Input.GetAxis("Mouse Y") * speed;

                float rotationX = rotation.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * speed;

                rotationY += -Input.GetAxis("Mouse Y") * speed;
                rotationY = Mathf.Clamp(rotationY, cameraYMin, cameraYMax);

                rotation.transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }
        }
        else
        {
            rotation.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
        }
    }
}
