using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//Make objects visible in inspector
[System.Serializable]
public static class Globals
{

	//Count the number of files within the directories
    public static int imageCount;
    public static int audioCount;
    public static int videoCount;
    public static int skyBoxCount;
    public static int objectCount;
    public static int prefabCount;

	public static string currentDir = Application.dataPath + "/../";

	public static GameObject mainCam;
	public static GenericObject genericObj;
}

public class Main : MonoBehaviour
{
    public GameObject thecamera;

	//References to each contextual menu within the Canvas
    public GameObject contentImages;
    public GameObject contentAudio;
    public GameObject contentVideos;
    public GameObject contentSkybox;
    public GameObject contentObjects;
	public GameObject contentComplexObjects;
    public GameObject contentMain;


	//Contextual menu flags to open and close
    public bool flagImages = false;
    public bool flagAudio = false;
    public bool flagVideo = false;
    public bool flagSkybox= false;
    public bool flagObjects = false;
    public bool flagMain = false;
	public bool flagComplexObjects = false;

    public bool initialSnapToGrid = false;
	public bool initialUseRotationOffset = false;
    public float initialDistance = 15;
    public float initialSmooth = 15;

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

    // Use this for initialization
    void Start()
    {
		//Initialize
		Globals.imageCount = 0;
        Globals.audioCount = 0;
        Globals.videoCount = 0;
        Globals.skyBoxCount = 0;
        Globals.prefabCount = 0;
        Globals.objectCount = 4;

        Globals.mainCam = GameObject.FindWithTag("MainCamera");
        thecamera = GameObject.Find("360Capture");


		//Initialize
        flagImages = false;
        flagAudio = false;
        flagVideo = false;
        flagSkybox = false;
		flagComplexObjects = false;
		flagObjects = false;


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

		//Initialize generic carried object handler
		Globals.genericObj = new GenericObject(initialSnapToGrid, initialUseRotationOffset, initialDistance, initialSmooth);

		//Add secondary scripts
		gameObject.AddComponent<ObjectManipulation>();
    }

    //Load skybox materials from resource folder
    IEnumerator load_Skybox()
    {
        //Get the path of the skybox materials
        path_Skybox_Materials = Directory.GetFiles(Globals.currentDir + "\\Assets\\Resources", "*.mat");
        //Debug.Log(path_Skybox_Materials.ToString());
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
        path_Image_Files = Directory.GetFiles(Globals.currentDir + "\\Assets\\Resources\\Images", "*.jpg");

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
        path_Audio_Files = Directory.GetFiles(Globals.currentDir + "\\Assets\\Resources\\Audio", "*.ogg");

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
        path_Video_Files = Directory.GetFiles(Globals.currentDir + "\\Assets\\Resources\\Videos", "*.ogv");

        //Get the path of the audio files for the video
        path_Audio_Vid_Files = Directory.GetFiles(Globals.currentDir + "\\Assets\\Resources\\Videos", "*.ogg");

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
        path_Prefabs_Files = Directory.GetFiles(Globals.currentDir + "\\Assets\\Resources", "*.prefab");

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

	//Finding all references to contextual UI's
    void findAll()
    {
        contentImages = GameObject.Find("Canvas").transform.Find("ContentImages").gameObject;
        contentAudio = GameObject.Find("Canvas").transform.Find("ContentAudio").gameObject;
        contentVideos = GameObject.Find("Canvas").transform.Find("ContentVideos").gameObject;
        contentSkybox = GameObject.Find("Canvas").transform.Find("ContentSkybox").gameObject;
        contentObjects = GameObject.Find("Canvas").transform.Find("ContentSpawnObjects").gameObject;
		contentComplexObjects = GameObject.Find("Canvas").transform.Find("ContentSpawnComplexObjects").gameObject;
        contentMain = GameObject.Find("Canvas").transform.Find("ContentMain").gameObject;
    }


	//Disabling and hiding all references to contextual UI's
    void disableContentAll()
    {
		//First have to find all, reference is lost when disabled
        findAll();

		//Disable all
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
        if(signal < Globals.imageCount)
        {
            //Rayscan box for hit
            int x = Screen.width / 2;
            int y = Screen.height / 2;

        //Get the line of sight for the object
        Ray myRay_Image = Globals.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
        RaycastHit hit_Image;

            //Assign image to an object (material gets created for it)
            Renderer object_Source;

            //Check if the object is in line of sight
            if (Physics.Raycast(myRay_Image, out hit_Image))
            {
                //Check if it is not the terrain
                if (hit_Image.collider.gameObject.GetComponent<TerrainCollider>() == false)
                {
                    //Get the raycasted objects renderer properties
                    object_Source = hit_Image.transform.gameObject.GetComponent<Renderer>();
                    //Assign the image to the object as a material
                    object_Source.material.mainTexture = image_Files[signal];
                }
            }
        }
    }

    void applyAudio(int signal)
    {
        if (signal < Globals.audioCount)
        {
            //Rayscan box for hit
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            //Get the line of sight for the object
            Ray myRay_Audio = Globals.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit_Audio;

            //Variable for the audio source of the raycasted object
            AudioSource ray_Audio_Source;

            //Check if the object is in line of sight
            if (Physics.Raycast(myRay_Audio, out hit_Audio))
            {
                //Check if it is not the terrain
                if (hit_Audio.collider.gameObject.GetComponent<TerrainCollider>() == false)
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
                    ray_Audio_Source.clip = audio_Files[signal];
                    ray_Audio_Source.Stop();
                    ray_Audio_Source.Play();
                }
            }
        }
    }

    void applyVideo(int signal)
    {
        if (signal < Globals.videoCount)
        {
            //Rayscan box for hit
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            //Get the line of sight for the object
            Ray myRay_Video = Globals.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit_Video;

            //Variable for the audio source of the raycasted object
            AudioSource ray_Audio_Source;

            //Variable for the video source of the raycasted object
            Renderer ray_Video_Source;

            //Check if the object is in line of sight
            if (Physics.Raycast(myRay_Video, out hit_Video))
            {
                //Check if it is not the terrain
                if (hit_Video.collider.gameObject.GetComponent<TerrainCollider>() == false)
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
                    ray_Video_Source.material.mainTexture = ogv_Files[signal];
                    ray_Audio_Source.clip = ogg_Files[signal];

                    //Play both audio and video
                    ogv_Files[signal].Stop();
                    ray_Audio_Source.Stop();
                    ray_Audio_Source.Play();
                    ogv_Files[signal].Play();
                }
            }
        }
    }


	//Apply skybox renderer
    void applySkybox(int signal)
    {
        if (signal < Globals.skyBoxCount)
        {
            RenderSettings.skybox = skyboxes[signal];
        }
    }

    void spawnComplex(int signal)
    {
        // Check if the the signal is indeed smaller than the arrays size
        if (signal < Globals.prefabCount)
        {
            //Get the position of the player 
            Vector3 player_Position = Globals.mainCam.transform.position;
            //Get direction of the player 
            Vector3 player_Direction = Globals.mainCam.transform.forward;
            //Get rotation of the player
            Quaternion player_Rotation = Globals.mainCam.transform.rotation;
            //Spawn distance from the player 
            float spawnDistance = 12;
            //Spawning position 
            Vector3 spawn_Object = player_Position + player_Direction * spawnDistance;

            //Display name of prefab in console
            Debug.Log(prefab_Files[signal].name);
            //Spawn in the prefab object
            GameObject spawn = prefab_Files[signal];
            //Spawn in the object
            Instantiate(spawn, spawn_Object, player_Rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        thecamera.transform.position = Globals.mainCam.transform.position;

        // Stop movie textures
        if (Input.GetKeyDown(KeyCode.V))
        {
            //Rayscan box for hit
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            //Get the line of sight for the object
            Ray myRay_Video = Globals.mainCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit_Video;

            //Check if the object is in line of sight
            if (Physics.Raycast(myRay_Video, out hit_Video))
            {
                //Check if it is not the terrain
                if (hit_Video.collider.gameObject.GetComponent<TerrainCollider>() == false)
                {
                    //Variable for the video source of the raycasted object
                    Renderer ray_Video_Source = hit_Video.transform.gameObject.GetComponent<Renderer>();
                    //Get the name of the video file to stop
                    string vid_Name = ray_Video_Source.material.mainTexture.name;
                    //Get he objects audio source
                    AudioSource ray_Audio_Source = hit_Video.transform.gameObject.GetComponent<AudioSource>();

                    //Run through each video texture
                    for (int i = 0; i < Globals.videoCount; i++)
                    {
                        //If it is the same video texture
                        if (vid_Name == ogv_Files[i].name)
                        {
                            //Pause it
                            ogv_Files[i].Stop();
                            ray_Audio_Source.Stop();
                        }
                    }
                }
            }
        }

		//Within the images contextual UI, select specific image
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
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                signal = 5;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                signal = 6;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                signal = 7;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                signal = 8;
                applyImage(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                signal = 9;
                applyImage(signal);
            }
        }

		//Within the images contextual UI, select specific audio
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
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                signal = 5;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                signal = 6;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                signal = 7;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                signal = 8;
                applyAudio(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                signal = 9;
                applyAudio(signal);
            }
        }

		//Within the video contextual UI, select specific video
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
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                signal = 5;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                signal = 6;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                signal = 7;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                signal = 8;
                applyVideo(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                signal = 9;
                applyVideo(signal);
            }
        }

		//Within the skybox contextual UI, select specific skybox
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
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                signal = 8;
                applySkybox(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                signal = 9;
                applySkybox(signal);
            }
        }

		//Within the primitive object contextual UI, select specific object
        if (flagObjects)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Globals.genericObj.spawnCube();
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				Globals.genericObj.spawnSphere();
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Globals.genericObj.spawnCapsule();
            } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
                Globals.genericObj.spawnCylinder();
            }
        }

		//Within the complex object contextual UI, select specific object
		if (flagComplexObjects)
		{
            int signal = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1))
			{
                signal = 0;
                spawnComplex(signal);
			}
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
                signal = 1;
                spawnComplex(signal);
            }
			if (Input.GetKeyDown(KeyCode.Alpha3))
			{
                signal = 2;
                spawnComplex(signal);
            }
			if (Input.GetKeyDown(KeyCode.Alpha4))
			{
                signal = 3;
                spawnComplex(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                signal = 4;
                spawnComplex(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                signal = 5;
                spawnComplex(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                signal = 6;
                spawnComplex(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                signal = 7;
                spawnComplex(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                signal = 8;
                spawnComplex(signal);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                signal = 9;
                spawnComplex(signal);
            }
        }

		//If image contextual UI is active, 'B' goes back to main
        if (flagImages && Input.GetKeyDown(KeyCode.B))
        {
            disableContentAll();
            findAll();
            flagMain = true;
            contentImages.SetActive(false);
            flagImages = false;
            contentMain.SetActive(true);
        }

		//If audio contextual UI is active, 'B' goes back to main
        if (flagAudio && Input.GetKeyDown(KeyCode.B))
        {
            disableContentAll();
            findAll();
            flagMain = true;
            contentAudio.SetActive(false);
            flagAudio = false;
            contentMain.SetActive(true);
        }

		//If video contextual UI is active, 'B' goes back to main
        if (flagVideo && Input.GetKeyDown(KeyCode.B))
        {
            disableContentAll();
            findAll();
            flagMain = true;
            contentVideos.SetActive(false);
            flagVideo = false;
            contentMain.SetActive(true);
        }

		//If skybox contextual UI is active, 'B' goes back to main
        if (flagSkybox && Input.GetKeyDown(KeyCode.B))
        {
            disableContentAll();
            findAll();
            flagMain = true;
            contentSkybox.SetActive(false);
            flagSkybox = false;
            contentMain.SetActive(true);
        }

		//If primitive object contextual UI is active, 'B' goes back to main
        if (flagObjects && Input.GetKeyDown(KeyCode.B))
        {
            disableContentAll();
            findAll();
            flagMain = true;
            contentObjects.SetActive(false);
            flagObjects = false;
            contentMain.SetActive(true);
        }

		//If complex object contextual UI is active, 'B' goes back to main
		if (flagComplexObjects && Input.GetKeyDown(KeyCode.B))
		{
			disableContentAll();
			findAll();
			flagMain = true;
			contentComplexObjects.SetActive(false);
			flagComplexObjects = false;
			contentMain.SetActive(true);
		}

        // Check for skybox keybing to change it
		if (Input.GetKeyDown(KeyCode.Alpha4) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
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

        // Check for keybind click in order to add primitive
		if (Input.GetKeyDown(KeyCode.Alpha5) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
        {
            disableContentAll();
            findAll();
            flagObjects = false;
            contentObjects.SetActive(true);
            flagObjects = true;
        }


		// Check for keybind click in order to add prefab/complex
		if (Input.GetKeyDown(KeyCode.Alpha6) && !flagImages && !flagVideo && !flagSkybox && !flagAudio && !flagObjects && !flagComplexObjects)
		{
			disableContentAll();
			findAll();
			flagComplexObjects = false;
			contentComplexObjects.SetActive(true);
			flagComplexObjects = true;
		}

		// Check for keybind click in order to add pickup script
		if (Input.GetKeyDown(KeyCode.Tab)) {
			//Toggle Object interaction
			Globals.genericObj.toggleCanInteract();
        }
    }

}
