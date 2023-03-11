using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBrush : Brush
{
    [SerializeField] Texture2D texture;

    public override float Sample(float x, float y)
    {
        float angle = 0f;//TODO:
        return Sample(x, y, angle);
    }

    public float Sample(float x, float y, float angle)
    {
        //TODO:
        float newX = x;
        float newY = y;
        return texture.GetPixelBilinear(newX, newY).r;
    }

}
