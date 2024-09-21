using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceCreator : MonoBehaviour
{

    public static PuzzleGenerator GetPuzzleGenerator()
    {
        return GameObject.Find("GenerationManager").GetComponent<PuzzleGenerator>();
    }

    public static Board GetBoard()
    {
        return GameObject.Find("GenerationManager").GetComponent<Board>();
    }

    public static ModifyBoard GetModifyBoard()
    {
        return GameObject.Find("GenerationManager").GetComponent<ModifyBoard>();
    }

    public static Solver GetSolver()
    {
        return GameObject.Find("GenerationManager").GetComponent<Solver>();
    }

    public static SpawnGrid GetSpawnGrid()
    {
        return GameObject.Find("VisualManager").GetComponent<SpawnGrid>();
    }
    public static SpawnVehicles GetSpawnVehicles()
    {
        return GameObject.Find("VisualManager").GetComponent<SpawnVehicles>();
    }
    public static GameData GetGameData()
    {
        return GameObject.Find("MiscLogicManager").GetComponent<GameData>();
    }
    public static UIManager GetUIManager()
    {
        return GameObject.Find("UIManager").GetComponent<UIManager>();
    }
    public static VehicleMovement GetVehicleMovement()
    {
        return GameObject.Find("Main Camera").GetComponent<VehicleMovement>();
    }
}
