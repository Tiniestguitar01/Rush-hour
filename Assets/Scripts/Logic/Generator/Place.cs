using System;
using UnityEngine;

public class Place : IComparable<Place>
{
    public int size;
    public int[] placePosition;
    public Direction direction;
    public int cost;

    public Place(int size, int[] placePosition, Direction direction) //int[,] board)
    {
        this.size = size;
        this.placePosition = placePosition;
        this.direction = direction;
    }

    public int CompareTo(Place other)
    {
        return this.cost.CompareTo(other.cost);
    }

    public override string ToString()
    {
        return "Vehicle: {Size: " + size + ", placeposition: {" + placePosition[0] + "," + placePosition[1] + "}, Direction: " + direction + ", cost: " + cost + "}";
    }
}
