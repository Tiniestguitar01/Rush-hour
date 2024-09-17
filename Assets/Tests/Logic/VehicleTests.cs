using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class VehicleTests
{
    [Test]
    public void ShouldGetThePositionOfTheVehicle()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        Vehicle vehicleVertical = new Vehicle(1, 2, new int[] { 0, 0 }, Direction.Vertical, Board.Instance.board);
        Vehicle vehicleHorizontal = new Vehicle(1, 2, new int[] { 1, 1 }, Direction.Horizontal, Board.Instance.board);
        Vehicle vehicleSize3 = new Vehicle(1, 3, new int[] { 3, 2 }, Direction.Horizontal, Board.Instance.board);

        List<int[]> positionsVertical = vehicleVertical.GetPosition();
        List<int[]> positionsHorizontal = vehicleHorizontal.GetPosition();
        List<int[]> positionsSize3 = vehicleSize3.GetPosition();

        List<int[]> positionsVerticalTest = new List<int[]>();
        positionsVerticalTest.AddRange(new List<int[]> { new int[] { 0, 0 }, new int[] { 1, 0 } });

        List<int[]> positionsHorizontalTest = new List<int[]>();
        positionsHorizontalTest.AddRange(new List<int[]> { new int[] { 1, 1 }, new int[] { 1, 2 } });


        List<int[]> positionsSize3Test = new List<int[]>();
        positionsSize3Test.AddRange(new List<int[]> { new int[] { 3, 2 }, new int[] { 3, 3 }, new int[] { 3, 4 } });

        Assert.AreEqual(positionsVerticalTest, positionsVertical);
        Assert.AreEqual(positionsHorizontalTest, positionsHorizontal);
        Assert.AreEqual(positionsSize3Test, positionsSize3);
    }

    [Test]
    public void ShouldGetThePositionError()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        Vehicle vehicleVertical = new Vehicle(1, 2, new int[] { 0, 0 }, Direction.Vertical, Board.Instance.board);
        Vehicle vehicleHorizontal = new Vehicle(1, 2, new int[] { 1, 1 }, Direction.Horizontal, Board.Instance.board);
        Vehicle vehicleSize3 = new Vehicle(1, 3, new int[] { 3, 2 }, Direction.Horizontal, Board.Instance.board);

        Vehicle vehicleOverMaximum = new Vehicle(1, 3, new int[] { 5, 5 }, Direction.Horizontal, Board.Instance.board);

        List<int[]> positionsVertical = vehicleVertical.GetPosition();
        List<int[]> positionsHorizontal = vehicleHorizontal.GetPosition();
        List<int[]> positionsSize3 = vehicleSize3.GetPosition();
        List<int[]> positionsOverMaximum = vehicleOverMaximum.GetPosition();

        List<int[]> positionsVerticalTest = new List<int[]>();
        positionsVerticalTest.AddRange(new List<int[]> { new int[] { 0, 0 }, new int[] { 0, 1 } });

        List<int[]> positionsHorizontalTest = new List<int[]>();
        positionsHorizontalTest.AddRange(new List<int[]> { new int[] { 1, 1 }, new int[] { 2, 1 } });


        List<int[]> positionsSize3Test = new List<int[]>();
        positionsSize3Test.AddRange(new List<int[]> { new int[] { 3, 2 }, new int[] { 3, 3 } });

        Assert.AreNotEqual(positionsVerticalTest, positionsVertical);
        Assert.AreNotEqual(positionsHorizontalTest, positionsHorizontal);
        Assert.AreNotEqual(positionsSize3Test, positionsSize3);
        Assert.AreEqual(6, positionsOverMaximum[1][1]);
    }

    [Test]
    public void ShouldGetMoveablePositions()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        Vehicle vehicle = new Vehicle(1, 2, new int[] { 0, 0 }, Direction.Vertical, Board.Instance.board);

        List<int[]> possibleMovesTest = new List<int[]>();
        possibleMovesTest.AddRange(new List<int[]> {
            new int[] { 0, 0 },
            new int[] { 1, 0 },
            new int[] { 2, 0 },
            new int[] { 3, 0 },
            new int[] { 4, 0 },
        });
        Assert.AreEqual(possibleMovesTest, vehicle.possibleMoves);
    }

    [Test]
    public void ShouldMoveStartPosition()
    {
        Vehicle vehicle = new Vehicle(1, 2, new int[] { 0, 0 }, Direction.Vertical, Board.Instance.board);

        vehicle.Move(new int[] { 1, 0 });

        Assert.AreEqual(new int[] { 1, 0 }, vehicle.startPosition);
        Assert.Contains(new int[] { 1, 0 }, vehicle.possibleMoves);
    }
}
