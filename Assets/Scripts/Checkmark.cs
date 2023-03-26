using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkmark : MonoBehaviour
{
    public void Move(Transform target)
    {
        transform.position = target.position + new Vector3(55, 0, 0);
    }
}
