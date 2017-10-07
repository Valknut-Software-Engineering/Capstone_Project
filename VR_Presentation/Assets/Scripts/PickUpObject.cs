using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Make objects visible in inspector 
[System.Serializable]
public static class Globals
{
    public static int imageCount;
    public static int audioCount;
    public static int videoCount;
    public static int skyBoxCount;
    public static int objectCount;
    public static int prefabCount;
}

public class PickUpObject : MonoBehaviour
{
    public GameObject thecamera;
    public GameObject mainCam;
    public GameObject pickedUpObject;

    public GameObject contentImages;
    public GameObject contentAudio;
    public GameObject contentVideos;
    public GameObject contentSkybox;
    public GameObject contentObjects;
	public GameObject contentComplexObjects;
    public GameObject contentMain;

    public bool flagImages = false;
    public bool flagAudio = false;
    public bool flagVideo = false;
    public bool flagSkybox= false;
    public bool flagObjects = false;
    public bool flagMain = false;
	public bool flagComplexObjects;

    public bool isCarrying = false;
	public bool snapToGrid = false;
	public bool useRotationOffset = false;
	
    public float distance;
    public float smooth;

	public string currentDir;

    //Array for the skybox materials  
    public List<Material> skyboxes;
    //String array of all of the skybox materials
    string[] path_Skybox_Materials;
    //Variable for the size of the materials array
    int size_Sky = 0;
    //Variable to cycle through the skybox array
    int counter_Sky = 0;

    //List to store image files 
    public List<Texture2D> image_Files;
    //Variable to store the file path  
    string[] path_Image_Files;
    //Variable for the size of the images array
    int size_Imgs = 0;
    //Variable to cycle through the image array
    int counter_Imgs = 0;

    //List to store ogg auido files 
    public List<AudioClip> audio_Files;
    //Variable to store the file path  
    string[] path_Audio_Files;

    //List to store ogv videos 
    public List<MovieTexture> ogv_Files;
    //List to store ogg auido files 
    public List<AudioClip> ogg_Files;
    //Variable to store the file path  
    string[] path_Video_Files;
    //Variable to store the file path  
    string[] path_Audio_Vid_Files;

    //List to store prefabs
    public List<GameObject> prefab_Files;
    //Variable to store the file path  
    string[] path_Prefabs_Files;
    //Variable for the size of the materials array
    int size_Prefabs = 0;
    //Variable to cycle through the skybox array
    int counter_Prefabs = 0;

    // Use this for initialization
    void Start()
    {
		currentDir = Application.dataPath + "/../";

        Globals.imageCount = 0;
        Globals.audioCount = 0;
        Globals.videoCount = 0;
        Globals.skyBoxCount = 0;
        Globals.prefabCount = 0;
        Globals.objectCount = 4;

        mainCam = GameObject.FindWithTag("MainCamera");
        thecamera = GameObject.Find("360Capture"); 

        flagImages = false;
        flagAudio = false;
        flagVideo = false;
        flagSkybox = false;

        
        //Call load image function 
        StartCoroutine(load_Images());        

        //Call load audio function 
        StartCoroutine(load_Audio());

        //Call load video function 
        StartCoroutine(load_Videos());

        //Call load skybox function 
        StartCoroutine(load_Skybox());

        //Call load prefabs function 
        StartCoroutine(load_Prefabs());

        contentMain = GameObject.Find("Canvas").transform.Find("ContentMain").gameObject;
    }

    //Load skybox materials from resource folder
    IEnumerator load_Skybox()
    {
        //Get the path of the skybox materials
        path_Skybox_Materials = Directory.GetFiles(currentDir + "\\Assets\\Resources", "*.mat");
        
        //Assign the size variable
        size_Sky = path_Skybox_Materials.Length;

        //Load all of the materials in the resource folder and perform string handling 
        for (int i = 0; i < path_Skybox_Materials.Length; i++)
        {
            //Perform string handling 
            string temp = path_Skybox_Materials[i];
            string[] split_String = temp.Split('\\');
            temp = split_String[split_String.Length - 1];
            string[] material_Split = temp.Split('.');

            //Add material to material array 
            skyboxes.Add(Resources.Load(material_Split[0]) as Material);
        }
        Globals.skyBoxCount = skyboxes.Count;
        yield return 0;
    }
     
    //Load videos from folder function
    IEnumerator load_Images()
    {
        
        //Get the path of the image files
        path_Image_Files = Directory.GetFiles(currentDir + "\\Assets\\Resources\\Images", "*.jpg");

        //Assign the size variable
        size_Imgs = path_Image_Files.Length;

        //Loop through each of the file paths inside if the directory 
        for (int i = 0; i < path_Image_Files.Length; i++)
        {
            //Load files from disk 
            WWW images_On_Disk = new WWW("file://" + path_Image_Files[i]);

            //Wait until images are finished loading 
            while (!images_On_Disk.isDone)
            {
                yield return null;
            }

            //Add files to array of images
            image_Files.Add(images_On_Disk.texture);
        }       
        Globals.imageCount = image_Files.Count;        
    }

    //Load audio off files from folder
    IEnumerator load_Audio()
    {
        //Get the path of the audio files
        path_Audio_Files = Directory.GetFiles(currentDir + "\\Assets\\Resources\\Audio", "*.ogg");

        //Loop through each of the file paths inside if the directory 
        for (int i = 0; i < path_Audio_Files.Length; i++)
        {
            //Load files from disk 
            WWW audio_On_Disk = new WWW("file://" + path_Audio_Files[0]);

            //Wait until auido files are finished loading 
            while (!audio_On_Disk.isDone)
            {
                yield return null;
            }

            //Add files to array of ogv videos 
            audio_Files.Add(audio_On_Disk.GetAudioClip());
        }
        Globals.audioCount = audio_Files.Count;
    }

    //Load videos from folder 
    IEnumerator load_Videos()
    {
        //Get the path of the video files
        path_Video_Files = Directory.GetFiles(currentDir + "\\Assets\\Resources\\Videos", "*.ogv");

        //Get the path of the audio files for the video
        path_Audio_Vid_Files = Directory.GetFiles(currentDir + "\\Assets\\Resources\\Videos", "*.ogg");

        //Loop through each of the file paths inside if the directory 
        for (int i = 0; i < path_Video_Files.Length; i++)
        {
            //Load files from disk 
            WWW videos_On_Disk = new WWW("file://" + path_Video_Files[0]);

            //Wait until videos are finished loading 
            while (!videos_On_Disk.isDone)
            {
                yield return null;
            }

            //Load files from disk 
            WWW audio_On_Disk = new WWW("file://" + path_Audio_Vid_Files[0]);

            //Wait until auido files are finished loading 
            while (!audio_On_Disk.isDone)
            {
                yield return null;
            }

            //Add files to array of ogv videos 
            ogv_Files.Add(videos_On_Disk.GetMovieTexture());


            //Add files to array of ogv videos 
            ogg_Files.Add(audio_On_Disk.GetAudioClip());
        }
        Globals.videoCount = ogg_Files.Count;
    }

    //Load prefabs from resources folder 
    IEnumerator load_Prefabs()
    {
        //Get the path of the skybox materials
        path_Prefabs_Files = Directory.GetFiles(currentDir + "\\Assets\\Resources", "*.prefab");
        //Assign the size variable
        size_Prefabs = path_Prefabs_Files.Length;

        //Load all of the materials in the resource folder and perform string handling 
        for (int i = 0; i < path_Prefabs_Files.Length; i++)
        {
            //Perform string handling 
            string temp = path_Prefabs_Files[i];
            string[] split_String = temp.Split('\\');
            temp = split_String[split_String.Length - 1];
            string[] prefab_Split = temp.Split('.');

            //Add material to material array 
            prefab_Files.Add(Resources.Load(prefab_Split[0]) as GameObject);

            //Add pickupable script
            prefab_Files[i].AddComponent<Pickupable>();
            //Add rigidbody
            if (prefab_Files[i].GetComponent<Rigidbody>() == null)
            {
                prefab_Files[i].AddComponent<Rigidbody>();
            }
            //Add box collider
            if (prefab_Files[i].GetComponent<BoxCollider>() == null)
            {
                prefab_Files[i].AddComponent<BoxCollider>();
            }
            //Add convex mesh collider or destroy standard one if it has one 
            if (prefab_Files[i].GetComponent<MeshCollider>() == null)
            {
                prefab_Files[i].AddComponent<MeshCollider>().convex = true;
            }
            else
            {
                DestroyImmediate(prefab_Files[i].GetComponent<MeshCollider>(), true);
                prefab_Files[i].AddComponent<MeshCollider>().convex = true;
            }
        }
        Globals.prefabCount = prefab_Files.Count;
        yield return 0;
    }

    void findAll()
    {
        contentImages = GameObject.Find("Canvas").transform.Find("ContentImages").gameObject;
        contentAudio = GameObject.Find("Canvas").transform.Find("ContentAudio").gameObject;
        contentVideos = GameObject.Find("Canvas").transform.Find("ContentVideos").gameObject;
        contentSkybox = GameObject.Find("Canvas").transform.Find("ContentSkybox").gameObject;
        contentObjects = GameObject.Find("Canvas").transform.Find("ContentSpawnObjects").gameObject;
		contentObjects = GameObject.Find("Canvas").transform.Find("ContentSpawnComplexObjects").gameObject;
        contentMain = GameObject.Find("Canvas").transform.Find("ContentMain").gameObject;
    }

    void disableContentAll()
    {
        findAll();

        contentImages.SetActive(false);
        contentAudio.SetActive(false);
        contentVideos.SetActive(false);
        contentSkybox.SetActive(false);
        contentObjects.SetActive(false);
		contentComplexObjects.SetActive(false);
        contentMain.SetActive(false);
    }    
    
    void applyImage(int signal)
    {
        if(signal >= Globals.imageCount)
        {
            signal = 0;
        }

        //Rayscan box for hit
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        //Get the line of sight for the object
        Ray myRay_Image = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit_Image;

        //Assign image to an object (material gets created for it) 
        Renderer object_Source;

        //Check if the object is in line of sight
        if (Physics.Raycast(myRay_Image, out hit_Image))
        {
            //Get the raycasted objects renderer properties 
            object_Source = hit_Image.transform.gameObject.GetComponent<Renderer>();
            //Assign the image to the object as a material 
            object_Source.material.mainTexture = image_Files[signal];
                        
            //Reset counter if it is over the arrays size 
            if (counter_Imgs >= size_Imgs)
            {
                counter_Imgs = 0;
            }
        }        
    }

    void applyAudio(int signal)
    {
        if (signal >= Globals.imageCount)
        {
            signal = 0;
        }

        //Rayscan box for hit
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        //Get the line of sight for the object
        Ray myRay_Audio = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit_Audio;

        //Variable for the audio source of the raycasted object 
        AudioSource ray_Audio_Source;

        //Check if the object is in line of sight
        if (Physics.Raycast(myRay_Audio, out hit_Audio))
        {
            //Get he objects audio source 
            ray_Audio_Source = hit_Audio.transform.gameObject.GetComponent<AudioSource>();

            //Check if there is an audio component present
            if (ray_Audio_Source == null)
            {
                //Add an audio source to the object 
                ray_Audio_Source = hit_Audio.transform.gameObject.AddComponent<AudioSource>();
                //Get he objects audio source 
                ray_Audio_Source = hit_Audio.transform.gameObject.GetComponent<AudioSource>();
            }

            //Assign the audio clip to the object and then play it 
            ray_Audio_Source.clip = audio_Files[0];
            ray_Audio_Source.Play();
        }
    }

    void applyVideo(int signal)
    {
        if (signal >= Globals.imageCount)
        {
            signal = 0;
        }

        //Rayscan box for hit
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        //Get the line of sight for the object
        Ray myRay_Video = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit_Video;

        //Variable for the audio source of the raycasted object 
        AudioSource ray_Audio_Source;

        //Variable for the video source of the raycasted object 
        Renderer ray_Video_Source;

        //Movietexture assigned to the object
        MovieTexture objct;

        //Check if the object is in line of sight
        if (Physics.Raycast(myRay_Video, out hit_Video))
        {
            //Get he objects audio source 
            ray_Audio_Source = hit_Video.transform.gameObject.GetComponent<AudioSource>();

            //Check if there is an audio component present
            if (ray_Audio_Source == null)
            {
                //Add an audio source to the object 
                ray_Audio_Source = hit_Video.transform.gameObject.AddComponent<AudioSource>();
                //Get he objects audio source 
                ray_Audio_Source = hit_Video.transform.gameObject.GetComponent<AudioSource>();
            }

            //Get the renderer property of the object in the raycast view 
            ray_Video_Source = hit_Video.transform.gameObject.GetComponent<Renderer>();

            //Assign movie texture to the object along with its audio 
            ray_Video_Source.material.SetTexture("_MainTex", ogv_Files[0]);
            ray_Audio_Source.clip = ogg_Files[0];
            objct = hit_Video.transform.gameObject.GetComponent<MovieTexture>();
            //Play both audio and video 
            ray_Audio_Source.Play();
            ogv_Files[signal].Play();
        }
    }

    void applySkybox(int signal)
    {
        if (signal >= Globals.skyBoxCount)
        {
            signal = 0;
        }

        RenderSettings.skybox = skyboxes[signal];        
    }


    // Update is called once per frame
    void Update()
    {
        thecamera.transform.position = mainCam.transform.position;

        //Check for object spawning click 
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Display name of prefab in console
            Debug.Log(prefab_Files[counter_Prefabs].name);
            //Spawn in the prefab object
            GameObject spawn = prefab_Files[counter_Prefabs];
            //Spawn in the object
            Instantiate(spawn, mainCam.transform.position, Quaternion.identity);

            //Incriment counter to loop through prefab array
            counter_Prefabs++;
            //Reset counter if it is too big
            if (counter_Prefabs >= size_Prefabs)
            {
                counter_Prefabs = 0;
            }
        }

        if (flagImages)
        {
            int signal = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                signal = 0;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                signal = 1;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                signal = 2;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                signal = 3;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                signal = 4;
                applyImage(signal);
            }
        }
        if (flagAudio)
        {
            int signal = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                signal = 0;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                signal = 1;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                signal = 2;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                signal = 3;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                signal = 4;
                applyAudio(signal);
            }

        }
        if (flagVideo)
        {
            int signal = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                signal = 0;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                signal = 1;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                signal = 2;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                signal = 3;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                signal = 4;
                applyVideo(signal);
            }

        }
        if (flagSkybox)
        {
            int signal = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                signal = 0;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                signal = 1;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                signal = 2;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                signal = 3;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                signal = 4;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                signal = 5;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                signal = 6;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                signal = 7;
                applySkybox(signal);
            }
        }
        if (flagObjects)
        {            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {               
                spawnCube();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {               
                spawnSphere();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {                
                spawnCapsule();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {                
                spawnCylinder();
            } 
        }

		if (flagComplexObjects)
		{            
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{               
				spawnCube();
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{               
				spawnSphere();
			}
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{                
				spawnCapsule();
			}
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{                
				spawnCylinder();
			} 
		}

        if (flagImages && Input.GetKeyDown(KeyCode.B))
        {            
            disableContentAll();
            findAll();
            flagMain = true;
            contentImages.SetActive(false);
            flagImages = false;
            contentMain.SetActive(true);
        }

        if (flagAudio && Input.GetKeyDown(KeyCode.B))
        {           
            disableContentAll();
            findAll();
            flagMain = true;
            contentAudio.SetActive(false);
            flagAudio = false;
            contentMain.SetActive(true);
        }

        if (flagVideo && Input.GetKeyDown(KeyCode.B))
        {           
            disableContentAll();
            findAll();
            flagMain = true;
            contentVideos.SetActive(false);
            flagVideo = false;
            contentMain.SetActive(true);
        }

        if (flagSkybox && Input.GetKeyDown(KeyCode.B))
        {           
            disableContentAll();
            findAll();
            flagMain = true;
            contentSkybox.SetActive(false);
            flagSkybox = false;
            contentMain.SetActive(true);
        }

        if (flagObjects && Input.GetKeyDown(KeyCode.B))
        {           
            disableContentAll();
            findAll();
            flagMain = true;
            contentObjects.SetActive(false);
            flagObjects = false;
            contentMain.SetActive(true);
        }

		if (flagComplexObjects && Input.GetKeyDown(KeyCode.B))
		{           
			disableContentAll();
			findAll();
			flagMain = true;
			contentComplexObjects.SetActive(false);
			flagComplexObjects = false;
			contentMain.SetActive(true);
		}


        //Toggle snap to grid
        if (Input.GetKeyDown(KeyCode.G)) {
            if (snapToGrid)
                snapToGrid = false;
            else
                snapToGrid = true;
        }

        //Toggle player rotation offset
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (useRotationOffset)
                useRotationOffset = false;
            else
                useRotationOffset = true;
        }

        if (isCarrying)
        {
            carry(pickedUpObject);
            checkDrop();
        }
        else
        {
            interact();
        }

        // Check for skybox keybing to change it 
        if (Input.GetKeyDown(KeyCode.Alpha4) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects)
        {
            if (flagSkybox)
            {
                disableContentAll();
                findAll();
                flagSkybox = false;
                contentMain.SetActive(true);
            }
            else
            {
                disableContentAll();
                findAll();
                flagMain = false;
                contentSkybox.SetActive(true);
                flagSkybox = true;
            }


        }

        // Check for keybind click in order to add image to objecT as a texture 
		if (Input.GetKeyDown(KeyCode.Alpha1) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
        {            
            disableContentAll();
            findAll();
            flagMain = false;
            contentImages.SetActive(true);
            flagImages = true;
        }


        // Check for keybind click in order to play audio  
		if (Input.GetKeyDown(KeyCode.Alpha2) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
        {            
            disableContentAll();
            findAll();
            flagMain = false;
            contentAudio.SetActive(true);
            flagAudio = true;
        }

        // Check for keybind click in order to add movie texture 
		if (Input.GetKeyDown(KeyCode.Alpha3) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
        {            
            disableContentAll();
            findAll();
            flagMain = false;
            contentVideos.SetActive(true);
            flagVideo = true;
        }

        // Check for keybind click in order to add movie texture 
		if (Input.GetKeyDown(KeyCode.Alpha5) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
        {            
            disableContentAll();
            findAll();
            flagObjects = false;
            contentObjects.SetActive(true);
            flagObjects = true;
        }

		// Check for keybind click in order to add movie texture 
		if (Input.GetKeyDown(KeyCode.Alpha6) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
		{            
			disableContentAll();
			findAll();
			flagComplexObjects = false;
			contentComplexObjects.SetActive(true);
			flagComplexObjects = true;
		}

        // Check for keybind click in order to add pickup script 
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;

            if (Physics.Raycast(myRay, out hit))
            {
                GameObject gameObj = hit.collider.gameObject;
                Debug.Log(hit.collider.gameObject.name);

                if (hit.collider.gameObject.GetComponent<TerrainCollider>() == true)
                {
                    return;
                }
                else
                {
                    add_Pickupable_Script();
                }   
            }
        }
    }

    // Apply script function 
    void add_Pickupable_Script()
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit;

        if (Physics.Raycast(myRay, out hit))
        {
            GameObject gameObj = hit.collider.gameObject;
            Pickupable isPickUpable = hit.collider.gameObject.GetComponent<Pickupable>();

            //if the object the RayCast hit has the canPickup script:
            if (isPickUpable != null)
            {
                if (gameObj.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rig = gameObj.GetComponent<Rigidbody>();
                    Destroy(rig);
                }

                Destroy(isPickUpable);
            }
            else
            {
                gameObj.AddComponent<Pickupable>();

                if (gameObj.GetComponent<Rigidbody>() == null)
                {
                    gameObj.AddComponent<Rigidbody>();
                }

                if (gameObj.GetComponent<BoxCollider>() == null)
                {
                    gameObj.AddComponent<BoxCollider>();
                }

                if (gameObj.GetComponent<MeshCollider>() == null)
                {
                    gameObj.AddComponent<MeshCollider>().convex = true;
                }
                else
                {
                    Destroy(gameObj.GetComponent<MeshCollider>());
                    gameObj.AddComponent<MeshCollider>().convex = true;
                }
            }
        }
    }

    void carry(GameObject obj)
    {
        if(!snapToGrid) {
			obj.transform.position = Vector3.Lerp(obj.transform.position, mainCam.transform.position + mainCam.transform.forward * distance, Time.deltaTime * smooth);
		}
		else //snap object to grid when moving
		{
			Vector3 curPosInGrid = mainCam.transform.position + mainCam.transform.forward * distance;
			Vector3 newPosInGrid = new Vector3(Mathf.Round(curPosInGrid.x), Mathf.Round(curPosInGrid.y), Mathf.Round(curPosInGrid.z));
			
			//obj.transform.position = Vector3.Lerp(obj.transform.position, newPosInGrid, Time.deltaTime * smooth);
			obj.transform.position = newPosInGrid;
		}
		
		//Rotate object as player rotates
		obj.transform.parent = this.transform; // Make the object that collided with the player a child of the player
		if(useRotationOffset) {
			obj.transform.localRotation = Quaternion.Euler(Vector3.forward); // Not exactly sure what this does but if I leave it out it becomes random
			obj.transform.localRotation = Quaternion.Euler(90,0,90); // offset the rotation to stay in relation to player
		}

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
            useRotationOffset = false;
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
                  
                    cloneObj = Instantiate(sourceObj, new Vector3(1F, 1F, 1F), Quaternion.identity);
                    cloneObj.transform.localScale = sourceObj.transform.localScale*3;
                    isCarrying = true;
                    if (cloneObj.gameObject.GetComponent<Rigidbody>() == null)
                        cloneObj.gameObject.AddComponent<Rigidbody>();

                    //isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    cloneObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    cloneObj.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                    pickedUpObject.GetComponent<Rigidbody>().useGravity = true;
                    pickedUpObject.GetComponent<Rigidbody>().isKinematic = false;

                    pickedUpObject.transform.parent = null;

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
		
        if (pickedUpObject) { dropObject(); }
        
        isCarrying = true;
        cube.GetComponent<Rigidbody>().useGravity = false;
        cube.GetComponent<Rigidbody>().isKinematic = true;
		pickedUpObject = cube;
    }

    //// Spawn other primitive objects \\\\
    void spawnSphere()
    {
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
		sphere.AddComponent<Rigidbody>(); // Add the rigidbody.
        sphere.AddComponent<Pickupable>(); // Add the canPickup script.
		
        if (pickedUpObject) { dropObject(); }
        
        isCarrying = true;
        sphere.GetComponent<Rigidbody>().useGravity = false;
        sphere.GetComponent<Rigidbody>().isKinematic = true;
		pickedUpObject = sphere;
    }

    void spawnCapsule()
    {
		GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        
		capsule.AddComponent<Rigidbody>(); // Add the rigidbody.
        capsule.AddComponent<Pickupable>(); // Add the canPickup script.
		
        if (pickedUpObject) { dropObject(); }
        
		isCarrying = true;
        capsule.GetComponent<Rigidbody>().useGravity = false;
		capsule.GetComponent<Rigidbody>().isKinematic = true;
        pickedUpObject = capsule;
    }

    void spawnCylinder()
    {
		GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        
		cylinder.AddComponent<Rigidbody>(); // Add the rigidbody.
        cylinder.AddComponent<Pickupable>(); // Add the canPickup script.

        if (pickedUpObject) { dropObject(); }
        
		isCarrying = true;
        cylinder.GetComponent<Rigidbody>().useGravity = false;
        cylinder.GetComponent<Rigidbody>().isKinematic = true;
		pickedUpObject = cylinder;
    }

    void interact()
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
                if (isPickUpable != null && gameObject.GetComponent<Rigidbody>() != null)
                {
                    isCarrying = true;
					if(isPickUpable.gameObject.GetComponent<Rigidbody>() == null)
						isPickUpable.gameObject.AddComponent<Rigidbody>();
					
					isPickUpable.gameObject.GetComponent<Rigidbody>().useGravity = false;
					isPickUpable.gameObject.GetComponent<Rigidbody>().isKinematic = true;
					pickedUpObject = isPickUpable.gameObject;
                }
            }
        }
		
		//Delete object
		if(Input.GetKey(KeyCode.Delete)) {
			int x = Screen.width /2;
			int y = Screen.height /2;
			
			Ray myRay = mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
			RaycastHit hit;
			
			if(Physics.Raycast(myRay, out hit)) {
				Pickupable isPickUpable = hit.collider.GetComponent<Pickupable>();
				
				//if the object the RayCast hit has the canPickup script:
				if(isPickUpable != null) {
					Destroy(isPickUpable.gameObject);
				}
			}
		}

    
    }

    void checkDrop()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
			dropObject();
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			floatObject();
		}
		
    }

    //Drop the object being held by the user
	void dropObject() {
		isCarrying = false;
		pickedUpObject.transform.parent = null;
		pickedUpObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
		pickedUpObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		pickedUpObject = null;
	}
	void floatObject() {
		isCarrying = false;
		pickedUpObject.transform.parent = null;
        Destroy(pickedUpObject.gameObject.GetComponent<Rigidbody>());
		pickedUpObject = null;
	}
}
