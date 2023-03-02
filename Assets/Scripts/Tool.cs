using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainTool : ScriptableObject
{
    abstract public void ApplyTool(Vector3 pos, Vector3 normal, Terrain terrainData);
    abstract public void ChangeSize(float newSize);
}
