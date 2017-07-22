using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public GameObject thecamera;
    public GameObject mainCam;
    public GameObject pickedUpObject;
    public bool isCarrying = false;
    public float distance;
    public float smooth;
   
 
      
        // Use this for initialization
    void Start()
    {
        mainCam = GameObject.FindWithTag("MainCamera");
        thecamera = GameObject.Find("recordCam");
    }

    // Update is called once per frame
    void Update()
    {
       
       
        thecamera.transform.position = mainCam.transform.position;
       
        //Spawn new object and place in hand
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            spawnCube();

        }

        if (isCarrying)
        {
            carry(pickedUpObject);
            checkDrop();
            
        }
        else
        {
            pickUp();
        }
    }

    void carry(GameObject obj)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, mainCam.transform.position + mainCam.transform.forward * distance, Time.deltaTime * smooth);

        //ALL AXIS MAKE LARGER
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            obj.transform.localScale += new Vector3(0.01F, 0.01F, 0.01F);
        }
        //ALL AXIS MAke SMALler
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            obj.transform.localScale -= new Vector3(0.01F, 0.01F, 0.01F);
        }
        // AXIS MAKE LARGER
        if (Input.GetKey(KeyCode.Keypad1))
        {
            obj.transform.localScale += new Vector3(0.01F, 0, 0);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            obj.transform.localScale += new Vector3(0, 0.01F, 0);
        }
        if (Input.GetKey(KeyCode.Keypad3))
        {
            obj.transform.localScale += new Vector3(0, 0, 0.01F);
        }

        //AXIS MAKE SMALLER
        if (Input.GetKey(KeyCode.Keypad4))
        {
            obj.transform.localScale -= new Vector3(0.01F, 0, 0);
        }
        if (Input.GetKey(KeyCode.Keypad5))
        {
            obj.transform.localScale -= new Vector3(0, 0.01F, 0F);
        }
        if (Input.GetKey(KeyCode.Keypad6))
        {
            obj.transform.localScale -= new Vector3(0, 0, 0.01F);
        }


        //Rotation
        if (Input.GetKey(KeyCode.Keypad7))
        {
            obj.transform.Rotate(0.5F, 0, 0);
        }
        if (Input.GetKey(KeyCode.Keypad8))
        {
            obj.transform.Rotate(0, 0.5F, 0F);
        }
        if (Input.GetKey(KeyCode.Keypad9))
        {
            obj.transform.Rotate(0, 0, 0.5F);
        }

        //RESET Rotation
        if (Input.GetKey(KeyCode.R))
        {
            obj.transform.rotation = Quaternion.Euler(0, 0, 0);
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.angularVelocity = Vector3.zero;
        }

        //Duplicate current object
        if (Input.GetKeyDown(KeyCode.C))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (Physics.Raycast(myRay, out hit))
            {
                Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();

                //if the object the RayCast hit has the canPickup script:
                if (isPickUpable != null)
                {
                    GameObject sourceObj = isPickUpable.gameObject;
                    GameObject cloneObj;
                    cloneObj = Instantiate(sourceObj, new Vector3(2 * 2.0F, 0, 0), Quaternion.identity);

                    isCarrying = true;
                    isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                    pickedUpObject = cloneObj;
                }
            }
        }

    }

    void spawnCube()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.AddComponent<Rigidbody>(); // Add the rigidbody.
        cube.AddComponent<Pickupable>(); // Add the canPickup script.

        isCarrying = true;
        cube.GetComponent<Rigidbody>().useGravity = false;
        if (pickedUpObject) { pickedUpObject.GetComponent<Rigidbody>().useGravity = true; }
        pickedUpObject = cube;
    }



    //// Spawn other primitive objects \\\\

    void spawnSphere()
    {

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(0, 1.5F, 0);

    }

    void spawnCapsule()
    {

        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsule.transform.position = new Vector3(2, 1, 0);

    }

    void spawnCylinder()
    {

        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = new Vector3(-2, 1, 0);

    }


    void pickUp()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (Physics.Raycast(myRay, out hit))
            {
               Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();

                //if the object the RayCast hit has the canPickup script:
                if (isPickUpable != null)
                {
                    isCarrying = true;
                    isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    pickedUpObject = isPickUpable.gameObject;
                }
            }
        }
    }

   
    void checkDrop()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dropObject();
        }
    }

    void dropObject()
    {
        isCarrying = false;
        pickedUpObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        pickedUpObject = null;
    }
}







