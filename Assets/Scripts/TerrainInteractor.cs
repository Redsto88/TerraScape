using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TerrainInteractor : MonoBehaviour
{
    [SerializeField]
    InputAction _toolAction;

    [SerializeField]
    InputAction _secondaryToolAction;

    [SerializeField]
    XRRayInteractor _rayInteractor;

    bool terrainHover = false;


    private void OnEnable() {
        _toolAction.Enable();
        _secondaryToolAction.Enable();
    }

    private void OnDisable() {
        _toolAction.Disable();
        _secondaryToolAction.Disable();
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

        if(!_toolAction.IsPressed())
            return;

        Terrain terrain = raycastHit.collider.gameObject.GetComponent<Terrain>();

        if(!_secondaryToolAction.IsPressed())
            GameManager.Tool.Apply(raycastHit.point, raycastHit.normal, terrain);
        else
            GameManager.Tool.ApplySecondary(raycastHit.point, raycastHit.normal, terrain);
    }

}
