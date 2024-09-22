using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

public class PuzzleGeneratorTests
{
    Board boardInstance;
    PuzzleGenerator puzzleGeneratorInstance;
    ModifyBoard modifyBoardInstance;
    SpawnVehicles spawnVehicleInstance;

    [SetUp]
    public void Init()
    {
        boardInstance = InstanceCreator.GetBoard();
        boardInstance.size = 6;
        boardInstance.GenerateBoard();
        puzzleGeneratorInstance = InstanceCreator.GetPuzzleGenerator();
        modifyBoardInstance = InstanceCreator.GetModifyBoard();
        spawnVehicleInstance = InstanceCreator.GetSpawnVehicles();
    }

    [Test]
    public void ShouldCreateVehicle()
    {
        puzzleGeneratorInstance.boardInstance = boardInstance;
        Vehicle vehicle = puzzleGeneratorInstance.CreateVehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical);

        Assert.AreEqual(1, vehicle.id);
        Assert.AreEqual(2, vehicle.size);
        Assert.AreEqual(new int[] { 0, 2 }, vehicle.startPosition);
        Assert.AreEqual(Direction.Vertical, vehicle.direction);
    }


    [Test]
    public async void ShouldInsertVehicleToBoard()
    {
        Vehicle vehicle = puzzleGeneratorInstance.CreateVehicle(1, 2, new int[] { 0, 2 }, Direction.Vertical);
        bool result = await puzzleGeneratorInstance.InsertVehicle(vehicle, boardInstance.board);

        int[,] testBoard = new int[,] {
            { 0,0,1,0,0,0 },
            { 0,0,1,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 },
            { 0,0,0,0,0,0 }
        };

        Assert.AreEqual(testBoard, boardInstance.board);
        Assert.AreEqual(true, result);
        Assert.AreEqual(1, puzzleGeneratorInstance.vehicles.Count);

        Assert.AreEqual(1, puzzleGeneratorInstance.vehicles[0].id);
        Assert.AreEqual(2, puzzleGeneratorInstance.vehicles[0].size);
        Assert.AreEqual(new int[] { 0, 2 }, puzzleGeneratorInstance.vehicles[0].startPosition);
        Assert.AreEqual(Direction.Vertical, puzzleGeneratorInstance.vehicles[0].direction);
        Assert.AreEqual(1, puzzleGeneratorInstance.carCount);

        for (int i = 0; i < boardInstance.places.Count; i++)
        {
            Assert.AreNotEqual(new int[] { 0, 2 }, boardInstance.places[i].placePosition);
            Assert.AreNotEqual(new int[] { 1, 2 }, boardInstance.places[i].placePosition);
        }
    }

    [Test]
    public async void ShouldDeleteVehicles()
    {
        ShouldInsertVehicleToBoard();

        puzzleGeneratorInstance.DeleteVehicles();

        Assert.AreEqual(0, puzzleGeneratorInstance.vehicles.Count);
        Assert.AreEqual(0, spawnVehicleInstance.vehicleGOs.Count);
    }

    [Test]
    public async void ShouldRemovePlaces()
    {
        puzzleGeneratorInstance.RemovePlaces(new int[] {2,2});

        for(int i = 0; i < boardInstance.places.Count;i++)
        {
            Assert.AreNotEqual(new int[] { 2, 2 }, boardInstance.places[i].placePosition);
            Assert.AreNotEqual(new int[] { 1, 2 }, boardInstance.places[i].placePosition);
            Assert.AreNotEqual(new int[] { 2, 1 }, boardInstance.places[i].placePosition);
        }
    }
}
