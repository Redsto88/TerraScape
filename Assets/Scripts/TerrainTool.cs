using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TerrainTool : MonoBehaviour
{
    [SerializeField] protected GameObject paramMenu;
    public GameObject ParamMenu => paramMenu;

    [SerializeField] protected Reticle reticle;
    public Reticle Reticle => reticle;

    bool inProgress = false;

    abstract public void Apply(Vector3 pos, Vector3 normal, Terrain terrainData, float multiplier=1f);

    public virtual void UpdateReticle(Vector3 pos, Vector3 normal)
    {
        reticle.transform.position = pos;
    }

    public virtual void ApplySecondary(Vector3 pos, Vector3 normal, Terrain terrainData)
    {
        Apply(pos, normal, terrainData, -1f);
    }

    public virtual void OnUseStart(){
        inProgress = true;
    }

    public virtual void OnUseEnd(){
        inProgress = false;
    }

    public virtual void OnHover(Vector3 pos, Vector3 normal)
    {
        reticle.OnHover( pos,  normal);
    }

    public virtual void OnHoverStart()
    {
        reticle.gameObject.SetActive(true);
    }

    public virtual void OnLeaveHover()
    {
        reticle.gameObject.SetActive(false);
    }
}
