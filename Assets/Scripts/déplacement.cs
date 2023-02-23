using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class déplacement : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 déplacement =  new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += déplacement * Time.deltaTime * 5;
        
    }
}
