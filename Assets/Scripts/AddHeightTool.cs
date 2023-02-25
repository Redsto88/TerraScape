using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddHeightTool", menuName = "TerraScape/AddHeightTool", order = 0)]
public class AddHeightTool : TerrainTool
{
    [SerializeField] float height = 10f;
    [SerializeField] float radius = 3f;
    [SerializeField] AnimationCurve fallOff;//TODO: use

    public override void ApplyTool(Vector3 pos, Vector3 normal, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;
        int resolution = terrainData.heightmapResolution;

        Vector3 relativePos = (pos - terrain.gameObject.transform.position);
        Vector2 floatingCenterIndex = new Vector2(relativePos.x / terrainData.size.x, relativePos.z / terrainData.size.z) * (resolution - 1);
        Vector2 radiusInIndexes = new Vector2(radius / terrainData.size.x, radius / terrainData.size.z) * (resolution - 1);

        int minX = Mathf.Clamp(Mathf.RoundToInt(floatingCenterIndex.x - radiusInIndexes.x), 0, resolution - 1);
        int maxX = Mathf.Clamp(Mathf.RoundToInt(floatingCenterIndex.x + radiusInIndexes.x), 0, resolution - 1);
        int minY = Mathf.Clamp(Mathf.RoundToInt(floatingCenterIndex.y - radiusInIndexes.y), 0, resolution - 1);
        int maxY = Mathf.Clamp(Mathf.RoundToInt(floatingCenterIndex.y + radiusInIndexes.y), 0, resolution - 1);

        if(minX >= maxX || minY >= maxY)
            throw new System.Exception("Zone incorrecte");

        float[,] newHeights = terrainData.GetHeights(minX, minY, maxX - minX, maxY - minY);
        for (int y = 0; y < newHeights.GetLength(0); y++)
        {
            for (int x = 0; x < newHeights.GetLength(1); x++)
            {
                Vector2 brushUV = new Vector2((float)x / radiusInIndexes.x, (float)y / radiusInIndexes.y);
                float finalUV = 1f - (brushUV - new Vector2(0.5f, 0.5f)).magnitude;
                //TODO: SetHeightsDelayLOD
                newHeights[y,x] = newHeights[y,x] + (height / terrainData.size.y) * Time.deltaTime * fallOff.Evaluate(finalUV);
            }
        }

        terrainData.SetHeights(minX, minY, newHeights);
    }
}
