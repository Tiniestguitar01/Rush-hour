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

    
    [Test]
    public void ThinkFunMostDifficultPuzzle()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        ModifyBoard.Instance = new ModifyBoard();
        Solver.Instance = new Solver();

        PuzzleGenerator.Instance = new PuzzleGenerator();

        int[,] testBoard = new int[,] {
            { 12,0,0,7,7,7 },
            { 12,13,13,6,0,0 },
            { 12,10,0,6,5,5 },
            { 9,10,11,11,3,4 },
            { 9,0,1,2,3,4 },
            { 8,8,1,2,0,4 }
        };

        Assert.AreEqual(true, Solver.Instance.BestFirstSearch(testBoard));
    }

    [Test]
    public void HardestPossiblePuzzle()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        ModifyBoard.Instance = new ModifyBoard();
        Solver.Instance = new Solver();

        PuzzleGenerator.Instance = new PuzzleGenerator();

        int[,] testBoard = new int[,] {
            { 0,2,2,2,4,0 },
            { 3,3,1,0,4,5 },
            { 0,0,1,6,6,5 },
            { 13,10,10,9,7,7 },
            { 13,11,11,9,0,8 },
            { 12,12,12,9,0,8 }
        };

        Assert.AreEqual(true, Solver.Instance.BestFirstSearch(testBoard));
    }
}
