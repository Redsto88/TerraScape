using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkmark : MonoBehaviour
{
    public void Move(RectTransform target)
    {
        GetComponent<RectTransform>().localPosition = target.localPosition /*+ new Vector3(10, 0, 0)*/;
    }
}
