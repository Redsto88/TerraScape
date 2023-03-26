using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerMovement : MonoBehaviour
{
    private bool godMode = true;
    
    public float rotationAngle = 45f;
    private bool isRotating = false;

    [SerializeField] private Transform leftController;

    [SerializeField] private TextMeshProUGUI speedText;

    [SerializeField] XRRayInteractor leftRayInteractor;
    
    
    public float speed = 5f;

    public bool isMoving = false;
    private float heightPosition;
    private float initialDistance;
    private Vector3 leftControllerStartPos;
    [SerializeField] private GameObject sphereMovingPoint;

    public float movementLimit = 100f;

    private Rigidbody rb;
    private Collider col;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }



    // Update is called once per frame
    void Update()
    {

        float HorizontalRight = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
        float VerticalRight = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");
        if(godMode){

            //BOUTONS
            bool X_button = Input.GetButton("XRI_Left_PrimaryButton");
            bool Y_button = Input.GetButton("XRI_Left_SecondaryButton");

            float deplacementHeight = 0;
            if(X_button){
                deplacementHeight -= 1;
            }
            if(Y_button){
                deplacementHeight += 1;
            }
            

            //STICK
            float Horizontal = Input.GetAxis("Horizontal");
            float Vertical = Input.GetAxis("Vertical");
            Vector3 camAngle = Camera.main.transform.eulerAngles;
            //déplacement selon un plan orthogonal à la caméra
            Vector3 deplacement = new Vector3(Horizontal, 0, Vertical);
            //rotation du vecteur de déplacement selon l'angle de la caméra
            deplacement = Quaternion.Euler(0, camAngle.y, 0) * deplacement;

            deplacement += new Vector3(0, deplacementHeight, 0);
            transform.position += deplacement * Time.deltaTime * speed;
            heightPosition += deplacement.y * Time.deltaTime * speed;

            //GACHETTE

            //si on appuie sur la gachette gauche
            if(Input.GetButton("XRI_Left_TriggerButton") && !isMoving){
                
                //raycast pour trouver le point de destination
                RaycastHit hit;
                Ray ray = new Ray(leftController.position, leftController.forward);
                //si le raycast touche un objet avec le tag "Terrain"
                if(Physics.Raycast(ray, out hit, 1000f)){
                    Debug.DrawRay(leftController.position, leftController.forward * 1000f, Color.green, 1f);
                    print(hit.collider.gameObject.tag);
                    if(hit.collider.gameObject.tag == "Terrain"){
                        isMoving = true;
                        leftControllerStartPos = leftController.position - transform.position;
                        //création d'un point de destination
                        sphereMovingPoint.transform.position = hit.point;
                        heightPosition = transform.position.y;
                        initialDistance = Vector3.Distance(transform.position, hit.point);
                        sphereMovingPoint.SetActive(true);

                        leftRayInteractor.SendHapticImpulse(0.2f, 0.15f);
                    }
                }
                else{
                    Debug.DrawRay(leftController.position, leftController.forward * 1000f, Color.red, 1f);
                }
                
            }
            //si on relache la gachette gauche
            if(!Input.GetButton("XRI_Left_TriggerButton") && isMoving){
                isMoving = false;
                sphereMovingPoint.SetActive(false);
                

            }
            
            if(isMoving){
                // float a = (sphereMovingPoint.transform.position.y - heightPosition)/leftController.forward.y;
                Vector3 leftControllerPos = leftController.position - transform.position;
                float dotProduct = (Vector3.Dot((leftControllerStartPos+transform.position - leftController.position), (sphereMovingPoint.transform.position - leftController.transform.position)));
                if (dotProduct>0.1f)
                {
                    dotProduct = 0.1f;
                }
                if(dotProduct<-0.1f){
                    dotProduct = -0.1f;
                }
                dotProduct*=10;
                print(" dot :" + dotProduct);
                float diff = Vector3.Distance(leftControllerPos,leftControllerStartPos) * dotProduct;
                print("diff :" + diff);
                Debug.Log(leftControllerPos);
                Debug.Log(leftControllerStartPos);
                
                
                diff = System.MathF.Sinh(10*diff) + 40*diff;
                
                if(initialDistance - diff < 3.0f){
                    diff = initialDistance - 3.0f;
                }
                //projection of Camera.forward on the plane orthogonal to the vector from the camera to the point
                Vector3 newPosition = sphereMovingPoint.transform.position - leftController.transform.forward * (initialDistance - diff);
                

                newPosition.x = Mathf.Clamp(newPosition.x, -movementLimit, movementLimit);
                newPosition.y = Mathf.Clamp(newPosition.y, 0, 1000f);
                newPosition.z = Mathf.Clamp(newPosition.z, -movementLimit, movementLimit);
                

                
                transform.position = newPosition + (transform.position - leftController.transform.position);
            
            }



            
        }


        //PAS GODMODE
        if(!godMode){
            //STICK
            float Horizontal = Input.GetAxis("Horizontal");
            float Vertical = Input.GetAxis("Vertical");
            Vector3 camAngle = Camera.main.transform.eulerAngles;
            //déplacement selon un plan orthogonal à la caméra
            Vector3 deplacement = new Vector3(Horizontal, 0, Vertical);
            //rotation du vecteur de déplacement selon l'angle de la caméra
            deplacement = Quaternion.Euler(0, camAngle.y, 0) * deplacement;
            rb.MovePosition(transform.position + deplacement * Time.deltaTime * speed);
        }


        //changement de mode
        if(Input.GetButtonDown("XRI_Left_GripButton")){
            if(!isMoving){
                godMode = !godMode;
            }
            if(godMode){
                rb.isKinematic = true;
                col.isTrigger = true;
            }
            else{
                rb.isKinematic = false;
                col.isTrigger = false;
            }
        }


        //QUELQUE SOIT LE MODE

        //changement de la vitesse de déplacement
            if(Mathf.Abs(VerticalRight)>0.7f){
                speed -= 0.1f * Mathf.Sign(VerticalRight);
                speed = Mathf.Clamp(speed, 0, 25);
                int speedInt = (int)((speed*100));
                speedText.text = "Movement speed: " + (((float)speedInt)/100).ToString();
            }

        //rotation de rotationAngle degrés
        if(Mathf.Abs(HorizontalRight)>0.7f && !isRotating){
            isRotating = true;
            transform.Rotate(Vector3.up, rotationAngle * Mathf.Sign(HorizontalRight));
        }
        if(Mathf.Abs(HorizontalRight)<0.7f && isRotating){
            isRotating = false;
        }

        //tp si le joueur est tombé
        if(transform.position.y < -5){
            //on raycast pour trouver le point de destination
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(0,1000,0), Vector3.down, out hit, Mathf.Infinity))
            {
                transform.position = hit.point + Vector3.up * 2f;
            }
        }
        
    }
}
