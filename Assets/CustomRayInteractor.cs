using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class CustomRayInteractor : XRRayInteractor
{

    [SerializeField]
    InputAction _terrainToolAction;

    protected override void OnEnable() {
        base.OnEnable();

        _terrainToolAction.Enable();
    }


    protected override void OnDisable() {
        base.OnDisable();

        _terrainToolAction.Disable();
    }

    private void Update() {
        if(!_terrainToolAction.IsPressed())
            return;

        if (!TryGetCurrent3DRaycastHit(out var raycastHit))
            return;

        if(!(raycastHit.collider is TerrainCollider))
            return;

        TerrainData terrainData = raycastHit.collider.gameObject.GetComponent<Terrain>().terrainData;

        Debug.Log("editing");

    }
}
