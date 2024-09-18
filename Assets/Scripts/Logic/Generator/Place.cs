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

    public void CalculateCost(int[] targetPosition,Direction prevDirection)
    {
        //distance from target car
        /*float x = Mathf.Pow(targetPosition[0] - placePosition[0],2);
        float y = Mathf.Pow(targetPosition[1] - placePosition[1],2);
        this.cost = (int)Mathf.Sqrt(x + y);*/

        this.cost += targetPosition[0] - placePosition[0];
        this.cost += targetPosition[1] - placePosition[1];

        //direction variation
        if (prevDirection == direction)
        {
            this.cost += 1;
        }


    }

    public override string ToString()
    {
        return "Vehicle: {Size: " + size + ", placeposition: {" + placePosition[0] + "," + placePosition[1] + "}, Direction: " + direction + ", cost: " + cost + "}";
    }
}
