using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddHeightTool", menuName = "TerraScape/AddHeightTool", order = 0)]
public class AddHeightTool : TerrainTool
{
    [SerializeField] float height = 10f;
    [SerializeField] float radius = 3f;
    [SerializeField] AnimationCurve fallOff;//TODO: use

    public void ApplyTool2(Vector3 pos, Vector3 normal, Terrain terrain)
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

    public override void ApplyTool(Vector3 pos, Vector3 normal, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;
        int resolution = terrainData.heightmapResolution;

        Vector3 objectCenter = (pos - terrain.gameObject.transform.position);
        Vector2 indexSpaceCenter = new Vector2(objectCenter.x / terrainData.size.x, objectCenter.z / terrainData.size.z) * (resolution - 1);
        Vector2 indexSpaceRadius = new Vector2(radius / terrainData.size.x, radius / terrainData.size.z) * (resolution - 1);

        int minX = Mathf.RoundToInt(indexSpaceCenter.x - indexSpaceRadius.x);
        int maxX = Mathf.RoundToInt(indexSpaceCenter.x + indexSpaceRadius.x);
        int minY = Mathf.RoundToInt(indexSpaceCenter.y - indexSpaceRadius.y);
        int maxY = Mathf.RoundToInt(indexSpaceCenter.y + indexSpaceRadius.y);
        int clampedMinX = Mathf.Clamp(minX, 0, resolution - 1);
        int clampedMaxX = Mathf.Clamp(maxX, 0, resolution - 1);
        int clampedMinY = Mathf.Clamp(minY, 0, resolution - 1);
        int clampedMaxY = Mathf.Clamp(maxY, 0, resolution - 1);

        if(clampedMinX > clampedMaxX || clampedMinY > clampedMaxY)
            throw new System.Exception("Zone incorrecte");

        float[,] newHeights = terrainData.GetHeights(clampedMinX, clampedMinY, clampedMaxX - clampedMinX, clampedMaxY - clampedMinY);

        for (int y = clampedMinY ; y < clampedMaxY; y++)
        {
            float brushY = (float)(y - minY) / (maxY - minY - 1);

            for (int x = clampedMinX; x < clampedMaxX; x++)
            {
                float brushX = (float)(x - minX) / (maxX - minX - 1);

                //float curveUV = Mathf.Clamp01(new Vector2(1f - Mathf.Abs(brushX - 0.5f) * 2f, 1f - Mathf.Abs(brushY - 0.5f) * 2f).magnitude);
                float curveUV = 1f - new Vector2(Mathf.Abs(brushX - 0.5f) * 2f, Mathf.Abs(brushY - 0.5f) * 2f).magnitude;
                //TODO: SetHeightsDelayLOD
                newHeights[y - clampedMinY, x - clampedMinX] += (height / terrainData.size.y) * Time.deltaTime * fallOff.Evaluate(curveUV);
            }
        }

        terrainData.SetHeights(clampedMinX, clampedMinY, newHeights);
    }
}
