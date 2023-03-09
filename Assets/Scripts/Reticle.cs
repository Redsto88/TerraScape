using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField] Transform anchor;

    public virtual void SetSize(float size)
    {
        anchor.localScale = new Vector3(size, size, size);
    }

    public virtual void OnHover(Vector3 pos, Vector3 normal)
    {
        transform.position = pos;
    }
}
