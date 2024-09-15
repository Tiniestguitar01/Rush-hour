using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SolverTests
{

    [Test]
    public void ShouldGetTrueForOneCar()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        ModifyBoard.Instance = new ModifyBoard();
        Solver.Instance = new Solver();

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(true,Solver.Instance.BestFirstSearch(testBoard));
    }

    [Test]
    public void ShouldGetTrueForBlockingCar()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        ModifyBoard.Instance = new ModifyBoard();
        Solver.Instance = new Solver();

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,2,2,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(true, Solver.Instance.BestFirstSearch(testBoard));
    }

    [Test]
    public void ShouldGetFalseForCarInFrontOfTargetCar()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        ModifyBoard.Instance = new ModifyBoard();
        Solver.Instance = new Solver();

        PuzzleGenerator.Instance = new PuzzleGenerator();

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,2,0,0,0 },
            { 0,0,2,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(false, Solver.Instance.BestFirstSearch(testBoard));
    }
}
