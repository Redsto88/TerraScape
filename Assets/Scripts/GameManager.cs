using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Terrain terrain;
    [SerializeField] TerrainTool _tool;
    [SerializeField] TerrainInteractor interactor;
    [SerializeField] List<Texture2D> brushes;

    public static TerrainTool Tool => Instance._tool;
    public static TerrainInteractor Interactor => Instance.interactor;
    public static List<Texture2D> Brushes => Instance.brushes;
    public static Terrain MainTerrain => Instance.terrain;

    public static void ChangeTool(TerrainTool tool)
    {
        Instance._tool = tool;
        tool.OnSelected();
    }
    
    private void Start() {
        ChangeTool(Instance.GetComponent<AddHeightTool>());
    }

    
}
