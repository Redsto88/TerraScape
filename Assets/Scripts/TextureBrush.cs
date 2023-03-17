using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBrush : Brush
{
    [SerializeField] Texture2D texture;

    public override float Sample(float x, float y)
    {
        float angle = - GameManager.Interactor.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;//TODO:vérifier que c'est bien
        return Sample(x, y, angle);
    }

    public float Sample(float x, float y, float angle)
    {
        float centerOffsetX = x - 0.5f;
        float centerOffsetY = y - 0.5f;
        float newX = Mathf.Cos(angle) * centerOffsetX + Mathf.Sin(angle) * centerOffsetY + 0.5f;
        float newY = -Mathf.Sin(angle) * centerOffsetX + Mathf.Cos(angle) * centerOffsetY + 0.5f;
        return texture.GetPixelBilinear(newX, newY).r;
    }

}
