using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardSpawnTests
{
    SpawnGrid spawnGridInstance;
    Board boardInstance;

    private GameObject cell = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Cell.prefab");

    [SetUp]
    public void Init()
    {
        GameObject InstanceCreatorGO = new GameObject("InstanceCreator");
        GameObject GenerationManagerGO = new GameObject("GenerationManager");
        GameObject VisualManagerGO = new GameObject("VisualManager");
        GameObject MiscLogicManagerGO = new GameObject("MiscLogicManager");
        GameObject UIManagerGO = new GameObject("UIManager");

        InstanceCreatorGO.AddComponent<InstanceCreator>();
        UIManagerGO.AddComponent<UIManager>();
        MiscLogicManagerGO.AddComponent<GameData>();
        boardInstance = GenerationManagerGO.AddComponent<Board>();
        spawnGridInstance = VisualManagerGO.AddComponent<SpawnGrid>();
        spawnGridInstance.cell = cell;

    }

    [Test]
    public void VehicleSpawnTestsSimplePasses()
    {
        spawnGridInstance.Start();
        boardInstance.Start();
        spawnGridInstance.distance = 1;
        spawnGridInstance.offset = 0;
        spawnGridInstance.Spawn();

        for (int x = 0; x < boardInstance.size; x++)
        {
            for (int z = 0; z < boardInstance.size; z++)
            {
                Debug.Log(x + "," + z);
                Debug.Log(x * (boardInstance.size) + z);
                Assert.AreEqual(boardInstance.BoardCoordinateToWordSpace(new int[] { x, z }), spawnGridInstance.instantiatedCells[x * (boardInstance.size) + z].transform.position);
            }
        }

    }

}
