using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextureTool : TerrainTool
{
    [SerializeField] float strength = 1f;
    [SerializeField] float radius = 3f;
    [SerializeField] private int textureNumber;
    private int textureCount = 4;
    [SerializeField] AnimationCurve fallOff;
    

    public void ChangeSize(float newSize)
    {
        radius = newSize;
    }

    public void ChangeStrength(float newStrength)
    {
        strength = newStrength;
    }
    
    public void ChangeTexture(int newTexture)
    {
        textureNumber = newTexture;
    }

    public override void Apply(Vector3 pos, Vector3 normal, Terrain terrain, float multiplier=1f)
    {
        TerrainData terrainData = terrain.terrainData;
        int resolution = terrainData.alphamapResolution;

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

        float[,,] newAlphas = terrainData.GetAlphamaps(clampedMinX, clampedMinY, clampedMaxX - clampedMinX, clampedMaxY - clampedMinY);

        for (int y = clampedMinY ; y < clampedMaxY; y++)
        {
            float brushY = (float)(y - minY) / (maxY - minY - 1);

            for (int x = clampedMinX; x < clampedMaxX; x++)
            {
                float brushX = (float)(x - minX) / (maxX - minX - 1);

                float curveUV = 1f - new Vector2(Mathf.Abs(brushX - 0.5f) * 2f, Mathf.Abs(brushY - 0.5f) * 2f).magnitude;
                newAlphas[y - clampedMinY, x - clampedMinX, textureNumber] += (strength / terrainData.size.y) * Time.deltaTime * fallOff.Evaluate(curveUV);
                //Normalisation des autres textures
                float alphaSum = 0f;
                for (int i = 0; i < textureCount; i++)
                {
                    if (i == textureNumber) continue;
                    alphaSum += newAlphas[y - clampedMinY, x - clampedMinX, i];
                }
                for (int i = 0; i < textureCount; i++)
                {
                    if (i == textureNumber) continue;
                    newAlphas[y - clampedMinY, x - clampedMinX, i] *= (1 - strength) / alphaSum;
                }
            }
        }

        //TODO: SetHeightsDelayLOD
        terrainData.SetAlphamaps(clampedMinX, clampedMinY, newAlphas);
    }
}
