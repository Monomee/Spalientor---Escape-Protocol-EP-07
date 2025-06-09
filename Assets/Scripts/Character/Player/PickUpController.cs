using SUPERCharacter;
using System.Collections;
using System.Collections.Generic;
using FirstPersonView;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public Transform gunHoldPos;
   
    public float throwForce = 500f; 
    public float pickUpRange = 5f; 
    private float rotationSensitivity = 1f; //rotate sen object held
    private GameObject heldObj; 
    private Rigidbody heldObjRb; 
    private bool canDrop = true;
    private bool isHoldingGun = false;
    private int LayerNumber; 

    private FirstPersonController mouseLookScript;
    float originalvalue;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");

        mouseLookScript = player.GetComponent<FirstPersonController>();
        originalvalue = mouseLookScript.sensitivity;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (heldObj == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, pickUpRange))
                {
                    if (hit.transform.gameObject.CompareTag("canPickUp"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }else if (hit.transform.gameObject.CompareTag("weapon"))
                    {
                        PickUpGun(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if(canDrop == true)
                {
                    StopClipping(); 
                    DropObject();
                }
            }
        }
        if (heldObj != null) 
        {
            MoveObject(); //keep object position at holdPos
            if(!isHoldingGun) RotateObject();
            if (Input.GetKeyDown(KeyCode.Q) && canDrop == true) 
            {
                StopClipping();
                ThrowObject();
            }

        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) 
        {
            heldObj = pickUpObj; 
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); 
            heldObjRb.isKinematic = true;
            heldObj.transform.SetParent(holdPos);
            heldObj.layer = LayerNumber;
            isHoldingGun = false;
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void PickUpGun(GameObject pickUpGun)
    {
        if (pickUpGun.GetComponent<Rigidbody>())
        {
            heldObj = pickUpGun;
            heldObjRb = pickUpGun.GetComponent<Rigidbody>();

            heldObjRb.isKinematic = true;
            heldObj.transform.SetParent(gunHoldPos);
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            heldObj.transform.localScale = Vector3.one;

            heldObj.layer = LayerNumber;
            isHoldingGun = true;
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; 
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        if (isHoldingGun)
        {
            Gun gunScript = heldObj.GetComponent<Gun>();
            if (gunScript != null)
            {
                gunScript.enabled = false;
            }
        }
        heldObj = null;
        isHoldingGun = false;
    }
    void MoveObject()
    {       
        heldObj.transform.position = isHoldingGun ? gunHoldPos.transform.position:holdPos.transform.position;
    }
    void RotateObject()
    {
        if (!isHoldingGun)
        {
            if (Input.GetKey(KeyCode.R))
            {
                canDrop = false; //make sure throwing can't occur during rotating

                //disable player being able to look around

                mouseLookScript.sensitivity = 0;

                float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
                float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
                //rotate the object depending on mouse X-Y Axis
                heldObj.transform.Rotate(Vector3.down, XaxisRotation);
                heldObj.transform.Rotate(Vector3.right, YaxisRotation);
            }
            else
            {
                //re-enable player being able to look around
                mouseLookScript.sensitivity = originalvalue;

                canDrop = true;
            }
        }
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        
        Vector3 throwDirection = Camera.main.transform.forward;
        heldObjRb.AddForce(throwDirection * throwForce);

        if (isHoldingGun)
        {
            Gun gunScript = heldObj.GetComponent<Gun>();
            if (gunScript != null)
            {
                gunScript.enabled = false;
            }
        }
        heldObj = null;
        isHoldingGun = false;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
