using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBrush : Brush
{
    [SerializeField] Texture2D texture;

    public override float Sample(float x, float y)
    {
        float angle = GameManager.Interactor.transform.rotation.y;//TODO:vérifier que c'est bien
        return Sample(x, y, angle);
    }

    public float Sample(float x, float y, float angle)
    {
        float newX = Mathf.Clamp01(Mathf.Cos(angle) * x + Mathf.Sin(angle) * y);
        float newY = Mathf.Clamp01(- Mathf.Sin(angle) * x + Mathf.Cos(angle) * y);
        return texture.GetPixelBilinear(newX, newY).r;
    }

}
