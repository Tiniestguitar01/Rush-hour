using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public float time;
    public float scale = 2f;
    public float speed = 1;
    public GameObject Light;


    void Update()
    {
        if (time < 360 * scale)
        {
            time += Time.deltaTime * speed;
        }
        else
        {
            time = 0;
        }
        Light.transform.eulerAngles = new Vector3(time / scale, 0, 0);
    }
}
