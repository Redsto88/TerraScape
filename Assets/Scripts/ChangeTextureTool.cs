using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeTextureTool : TerrainTool
{
    [SerializeField] private int textureNumber;
    [SerializeField] private int secondaryTextureNumber;
    private int textureCount = 4;
    
    protected override void Start() {
        base.Start();
    }

    public override void OnSelected() {
        base.Start();
        ChangeSize(radius);
        radiusCallback.Invoke(radius);
    }

    public void ChangeSize(float newSize)
    {
        radius = newSize;
        reticle.SetSize(newSize);
    }

    public void ChangeStrength(float newStrength)
    {
        strength = newStrength;
    }
    
    public void ChangeSecondaryTexture(float newTexture)
    {
        secondaryTextureNumber = (int) newTexture;
    }
    public void ChangeTexture(float newTexture)
    {
        textureNumber = (int)newTexture;
    }

    private void PaintTexture(Vector3 pos, int texture, Terrain terrain)
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

                float sample = Sample(brushX, brushY);
                float curveUV = 1f - new Vector2(Mathf.Abs(brushX - 0.5f) * 2f, Mathf.Abs(brushY - 0.5f) * 2f).magnitude;
                float newValue = Mathf.Clamp01(newAlphas[y - clampedMinY, x - clampedMinX, texture] +
                                               strength * Time.deltaTime * sample);
                newAlphas[y - clampedMinY, x - clampedMinX, texture] = newValue;
                //Normalisation des autres textures
                float alphaSum = 0f;
                for (int i = 0; i < textureCount; i++)
                {
                    if (i == texture) continue;
                    alphaSum += newAlphas[y - clampedMinY, x - clampedMinX, i];
                }
                for (int i = 0; i < textureCount; i++)
                {
                    if (i == texture) continue;
                    newAlphas[y - clampedMinY, x - clampedMinX, i] *= (1 - newValue) / alphaSum;
                }
            }
        }

        terrainData.SetAlphamaps(clampedMinX, clampedMinY, newAlphas);
    }
    public override void Apply(Vector3 pos, Vector3 normal, Terrain terrain)
    {
        PaintTexture(pos, textureNumber, terrain);
    }
    public override void ApplySecondary(Vector3 pos, Vector3 normal, Terrain terrain)
    {
        PaintTexture(pos, secondaryTextureNumber, terrain);
    }
}
