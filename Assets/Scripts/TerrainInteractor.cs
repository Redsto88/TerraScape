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
    XRRayInteractor _rayInteractor;



    private void OnEnable() {
        _toolAction.Enable();
    }

    private void OnDisable() {
        _toolAction.Disable();
    }

    private void Update() {
        if(!_toolAction.IsPressed())
            return;

        if (!_rayInteractor.TryGetCurrent3DRaycastHit(out var raycastHit))
            return;

        if(!(raycastHit.collider is TerrainCollider))
            return;

        Terrain terrain = raycastHit.collider.gameObject.GetComponent<Terrain>();

        GameManager.Tool.ApplyTool(raycastHit.point, raycastHit.normal, terrain);
    }

}
