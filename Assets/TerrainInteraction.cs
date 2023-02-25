using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TerrainInteraction : MonoBehaviour
{
    public void OnActivate(ActivateEventArgs args)
    {
        ((XRRayInteractor)args.interactorObject).TryGetHitInfo
            (out Vector3 position, out Vector3 normal, out int positionInLine, out bool isValidTarget);

        Debug.Log(position);
    }
}
