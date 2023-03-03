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



    private void OnEnable() {
        _toolAction.Enable();
        _secondaryToolAction.Enable();
    }

    private void OnDisable() {
        _toolAction.Disable();
        _secondaryToolAction.Disable();
    }

    private void Update() {
        if(!_toolAction.IsPressed())
            return;

        if (_rayInteractor.TryGetCurrentUIRaycastResult(out _))
            return;

        if (!_rayInteractor.TryGetCurrent3DRaycastHit(out var raycastHit))
            return;

        if(!(raycastHit.collider is TerrainCollider))
            return;

        Terrain terrain = raycastHit.collider.gameObject.GetComponent<Terrain>();

        if(!_secondaryToolAction.IsPressed())
            GameManager.Tool.Apply(raycastHit.point, raycastHit.normal, terrain);
        else
            GameManager.Tool.ApplySecondary(raycastHit.point, raycastHit.normal, terrain);
    }

}
