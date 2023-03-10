using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainTool : MonoBehaviour
{
    [SerializeField] GameObject paramMenu;
    public GameObject ParamMenu => paramMenu;

    [SerializeField] GameObject reticle;
    public GameObject Reticle => reticle;

    [SerializeField] Sprite icon;
    public Sprite Icon => icon;

    abstract public void Apply(Vector3 pos, Vector3 normal, Terrain terrainData);
    public virtual void ApplySecondary(Vector3 pos, Vector3 normal, Terrain terrainData)
    {
        Apply(pos, normal, terrainData);
    }

    public virtual void OnStart(){}
    public virtual void OnEnd(){}
}
