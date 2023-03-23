using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class TerrainInteractor : MonoBehaviour
{
    [SerializeField]
    InputAction _toolAction;

    [SerializeField]
    InputAction _secondaryToolAction;

    [SerializeField]
    XRRayInteractor _rayInteractor;

    [SerializeField]
    float minImpulse = 0.1f;

    [SerializeField]
    float maxImpulse = 0.3f;

    [SerializeField]
    float uiImpulse = 0.2f;

    [SerializeField]
    float uiImpulseDuration = 0.1f;

    bool terrainHover = false;


    private void OnEnable() {
        _toolAction.Enable();
        _secondaryToolAction.Enable();
    }

    private void OnDisable() {
        _toolAction.Disable();
        _secondaryToolAction.Disable();
    }

    public void UIImpulse()
    {
        _rayInteractor.SendHapticImpulse(uiImpulse, uiImpulseDuration);
    }

    private void ToolImpulse()
    {
        _rayInteractor.SendHapticImpulse(Mathf.Lerp(minImpulse, maxImpulse, GameManager.Tool.StrengthSlider.normalizedValue), 0.02f);
    }

    private void Update() {
        

        if (_rayInteractor.TryGetCurrentUIRaycastResult(out _)
            || !_rayInteractor.TryGetCurrent3DRaycastHit(out var raycastHit)
            || !(raycastHit.collider is TerrainCollider))
        {
            if(terrainHover)
            {
                terrainHover = false;
                GameManager.Tool.OnLeaveHover();
            }
            return;
        }

        if(!terrainHover)
        {
            terrainHover = true;
            GameManager.Tool.OnHoverStart();
        }

        GameManager.Tool.OnHover(raycastHit.point, raycastHit.normal);

        if (!_toolAction.IsPressed())
        {
            if (GameManager.Tool.InProgress)
            {
                GameManager.Tool.OnUseEnd(raycastHit);
            }
            return;
        }

        Terrain terrain = raycastHit.collider.gameObject.GetComponent<Terrain>();

        if (!GameManager.Tool.InProgress)
        {
            GameManager.Tool.OnUseStart(raycastHit);
        }
        
        if (!_secondaryToolAction.IsPressed())
        {
            GameManager.Tool.Apply(raycastHit.point, raycastHit.normal, terrain);
            ToolImpulse();
        }
        else
        {
            GameManager.Tool.ApplySecondary(raycastHit.point, raycastHit.normal, terrain);
            ToolImpulse();
        }
            
    }

}
