using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHeightTool : TerrainTool
{
    [SerializeField] float strength = 10f;
    [SerializeField] float radius = 3f;
    [SerializeField] AnimationCurve fallOff;

    public void ChangeSize(float newSize)
    {
        radius = newSize;
    }

    public void ChangeStrength(float newStrength)
    {
        strength = newStrength;
    }

    public override void Apply(Vector3 pos, Vector3 normal, Terrain terrain)
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

                float curveUV = 1f - new Vector2(Mathf.Abs(brushX - 0.5f) * 2f, Mathf.Abs(brushY - 0.5f) * 2f).magnitude;
                newHeights[y - clampedMinY, x - clampedMinX] += (strength / terrainData.size.y) * Time.deltaTime * fallOff.Evaluate(curveUV);
            }
        }

        //TODO: SetHeightsDelayLOD
        terrainData.SetHeights(clampedMinX, clampedMinY, newHeights);
    }
}
