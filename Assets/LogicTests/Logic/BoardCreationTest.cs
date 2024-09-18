using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardCreationTest
{
    [Test]
    public void ShouldCreateEmptyBoard()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoard, Board.Instance.board);
    }

    [Test]
    public void ShouldResultInBoardCreationError()
    {
        Board.Instance = new Board();
        Board.Instance.size = 6;
        Board.Instance.GenerateBoard();

        int[,] testBoard = new int[,] {
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,1,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreNotEqual(testBoard, Board.Instance.board);
    }
}
