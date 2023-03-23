using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class TerrainTool : MonoBehaviour
{
    [SerializeField] protected GameObject paramMenu;
    public GameObject ParamMenu => paramMenu;

    [SerializeField] protected Reticle reticle;
    public Reticle Reticle => reticle;

    Texture2D brush;
    public Texture2D Brush => brush;

    bool inProgress = false;
    public bool InProgress => inProgress;

    [SerializeField] UnityEvent<float> brushChangeCallback;

    protected virtual void Start() {
        brush = GameManager.Brushes[0];
        brushChangeCallback.Invoke(0);
    }

    public virtual void OnSelected()
    {
        
    }

    abstract public void Apply(Vector3 pos, Vector3 normal, Terrain terrainData);

    public virtual void UpdateReticle(Vector3 pos, Vector3 normal)
    {
        reticle.transform.position = pos;
    }

    public virtual void ApplySecondary(Vector3 pos, Vector3 normal, Terrain terrainData)
    {
        Apply(pos, normal, terrainData);
    }

    public virtual void OnUseStart(RaycastHit hit){
        inProgress = true;
    }

    public virtual void OnUseEnd(RaycastHit hit){
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

    public void ChangeBrush(float brushIndex)
    {
        brush = GameManager.Brushes[(int)brushIndex];
    }

    public float Sample(float x, float y)
    {
        float angle = - GameManager.Interactor.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        return Sample(x, y, angle);
    }

    public float Sample(float x, float y, float angle)
    {
        float centerOffsetX = x - 0.5f;
        float centerOffsetY = y - 0.5f;
        float newX = Mathf.Cos(angle) * centerOffsetX + Mathf.Sin(angle) * centerOffsetY + 0.5f;
        float newY = -Mathf.Sin(angle) * centerOffsetX + Mathf.Cos(angle) * centerOffsetY + 0.5f;
        return brush.GetPixelBilinear(newX, newY).r;
    }
}
