using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    TerrainTool tool;
    public static TerrainTool Tool => Instance.tool;

    private void Start() {
        //tool = new AddHeightTool();
    }
}