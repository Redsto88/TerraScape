using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Brush : MonoBehaviour {
    public abstract float Sample(float x, float y);
}
