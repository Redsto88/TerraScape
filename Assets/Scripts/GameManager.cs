using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TerrainTool _tool;
    [SerializeField] TerrainInteractor interactor;
    public static TerrainTool Tool => Instance._tool;

    public static void ChangeTool(TerrainTool tool)
    {
        Instance._tool = tool;
    }

    private void Start() {
        //tool = new AddHeightTool();
    }
}
