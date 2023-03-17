using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] TerrainTool _tool;
    [SerializeField] Brush _brush;
    [SerializeField] TerrainInteractor interactor;
    [SerializeField] List<Brush> brushes;

    public static TerrainTool Tool => Instance._tool;
    public static Brush Brush => Instance._brush;

    public static void ChangeTool(TerrainTool tool)
    {
        Instance._tool = tool;
    }

    public static void ChangeBrush(Brush brush)
    {
        Instance._brush = brush;
    }
    
    private void Start() {
        ChangeTool(Instance.GetComponent<AddHeightTool>());
    }

    
}
