using UnityEngine;

public class VehicleComponent : MonoBehaviour
{
    public int id;
    public int size;
    public int[] startPosition;
    public Direction direction;

    public void Init(int id, int size, int[] startPosition, Direction direction)
    {
        this.id = id;
        this.size = size;
        this.startPosition = startPosition;
        this.direction = direction;
    }
}
