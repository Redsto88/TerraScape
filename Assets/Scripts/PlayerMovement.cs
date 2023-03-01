using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    private bool godMode = true;
    
    public float rotationAngle = 45f;
    private bool isRotating = false;

    [SerializeField] private TextMeshProUGUI speedText;
    
    
    public float speed = 5f;
    

    // Update is called once per frame
    void Update()
    {
        float HorizontalRight = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
        float VerticalRight = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");
        if(godMode){

            //déplacement selon la caméra
            float Horizontal = Input.GetAxis("Horizontal");
            float Vertical = Input.GetAxis("Vertical");
            Vector3 camAngle = Camera.main.transform.eulerAngles;
            //déplacement selon un plan orthogonal à la caméra
            Vector3 déplacement = new Vector3(Horizontal, 0, Vertical);
            //rotation du vecteur de déplacement selon l'angle de la caméra
            déplacement = Quaternion.Euler(camAngle.x, camAngle.y, camAngle.z) * déplacement;
            transform.position += déplacement * Time.deltaTime * speed;

            //changement de la vitesse de déplacement
            if(Mathf.Abs(VerticalRight)>0.7f){
                speed -= 0.1f * Mathf.Sign(VerticalRight);
                speed = Mathf.Clamp(speed, 0, 25);
                int speedInt = (int)((speed*100));
                speedText.text = "Speed: " + (((float)speedInt)/100).ToString();
            }



        }

        
        if(Mathf.Abs(HorizontalRight)>0.7f && !isRotating){
            isRotating = true;
            transform.Rotate(Vector3.up, rotationAngle * Mathf.Sign(HorizontalRight));
        }
        if(Mathf.Abs(HorizontalRight)<0.7f && isRotating){
            isRotating = false;
        }
        
    }
}
