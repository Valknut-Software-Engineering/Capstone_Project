//////////////////////////////////////
//Script created by Alexander Ameye //
//Version: 2.2 (FREE)						    //
//////////////////////////////////////

using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
	// INSPECTOR SETTINGS
	[Header("Door Settings")]
	[Tooltip("The initial angle of the door/window.")]
	public float InitialAngle = 0.0F;
	[Tooltip("The amount of degrees the door/window rotates.")]
	public float RotationAngle = 90.0F;

	public enum PositionOfHinge
	{
		Left,
		Right,
	}
	public PositionOfHinge HingePosition;

	public enum SideOfRotation
	{
		Left,
		Right,
	}
	public SideOfRotation RotationSide;

	[Tooltip("Rotating speed of the door/window.")]
	public float Speed = 3F;
	[Tooltip("0 = infinite times")]
	public int TimesMoveable = 0;

	// PRIVATE SETTINGS
	private int n = 0; //For 'TimesMoveable' loop.
	[HideInInspector] public bool Running = false;

	// DEBUGGING
	[Header("Debug Settings")]
	[Tooltip("Visualizes the position of the hinge in-game by a colored cube.")]
	public bool VisualizeHinge = false;
	[Tooltip("The color of the visualization of the hinge.")]
	public Color HingeColor = Color.cyan;

	//Define an initial and final rotation.
	private Quaternion FinalRot, InitialRot;
	private int State;

	//Create a hinge.
	GameObject hinge;

	// START FUNCTION
	void Start ()
	{
		//Give the object the name "Door" for future reference.
		gameObject.tag = "Door";

		//Create a hinge.
		hinge = new GameObject();
		hinge.name = "hinge";

		//Calculate sine/cosine of initial angle (needed for hinge positioning).
		float CosDeg = Mathf.Cos ((transform.eulerAngles.y * Mathf.PI) / 180);
		float SinDeg = Mathf.Sin ((transform.eulerAngles.y * Mathf.PI) / 180);

		//Read transform (position/rotation/scale) of the door.
		float PosDoorX = transform.position.x;
		float PosDoorY = transform.position.y;
	  float PosDoorZ = transform.position.z;

		float RotDoorX = transform.localEulerAngles.x;
		float RotDoorZ = transform.localEulerAngles.z;

		float ScaleDoorX = transform.localScale.x;
		float ScaleDoorZ = transform.localScale.z;

		//Create a placeholder of the hinge's position/rotation.
		Vector3 HingePosCopy = hinge.transform.position;
		Vector3 HingeRotCopy = hinge.transform.localEulerAngles;

		// HINGE LEFT
		if (HingePosition == PositionOfHinge.Left)
		{
			// CALCULATE
			if (transform.localScale.x > transform.localScale.z)
			{
				HingePosCopy.x = (PosDoorX - (ScaleDoorX / 2 * CosDeg));
				HingePosCopy.z = (PosDoorZ + (ScaleDoorX / 2 * SinDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -InitialAngle;
				HingeRotCopy.z = RotDoorZ;
			}

			else
			{
				HingePosCopy.x = (PosDoorX + (ScaleDoorZ / 2 * SinDeg));
				HingePosCopy.z = (PosDoorZ + (ScaleDoorZ / 2 * CosDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -InitialAngle;
				HingeRotCopy.z = RotDoorZ;
     		}
		}

		// RIGHT
		if (HingePosition == PositionOfHinge.Right)
		{
			// CALCULATE
			if(transform.localScale.x > transform.localScale.z)
			{
				HingePosCopy.x = (PosDoorX + (ScaleDoorX / 2 * CosDeg));
				HingePosCopy.z = (PosDoorZ - (ScaleDoorX / 2 * SinDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -InitialAngle;
				HingeRotCopy.z = RotDoorZ;
			}

			else
			{
				HingePosCopy.x = (PosDoorX - (ScaleDoorZ / 2 * SinDeg));
				HingePosCopy.z = (PosDoorZ - (ScaleDoorZ / 2 * CosDeg));
				HingePosCopy.y = PosDoorY;

				HingeRotCopy.x = RotDoorX;
				HingeRotCopy.y = -InitialAngle;
				HingeRotCopy.z = RotDoorZ;
			}
		}

		// HINGE POSITIONING
		hinge.transform.position = HingePosCopy;
		transform.parent = hinge.transform;
		hinge.transform.localEulerAngles = HingeRotCopy;

		// DEBUGGING
		if (VisualizeHinge == true)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.position = HingePosCopy;
			cube.transform.localScale = new Vector3(0.25f,0.5f,0.25f);
			cube.GetComponent<Renderer>().material.color = HingeColor;
		}


		// ERROR CODES (UN-COMMENT THIS WHEN YOU'RE NOT BUILDING THE GAME)
		/*if (Mathf.Abs(InitialAngle) + Mathf.Abs(RotationAngle) == 180 || Mathf.Abs(InitialAngle) + Mathf.Abs(RotationAngle) > 180)
		{
 			UnityEditor.EditorUtility.DisplayDialog ("Error 001","The difference between 'Initial Angle' and 'Rotation Angle' can't exceed or be equal to 180 degrees.", "Ok", "");
 			UnityEditor.EditorApplication.isPlaying = false;
		}*/

		// ANGLES
		if (RotationSide == SideOfRotation.Left)
		{
			InitialRot = Quaternion.Euler (0, -InitialAngle, 0);
			FinalRot = Quaternion.Euler(0, -InitialAngle - RotationAngle, 0);
		}

		if (RotationSide == SideOfRotation.Right)
		{
			InitialRot = Quaternion.Euler (0, -InitialAngle, 0);
			FinalRot = Quaternion.Euler(0, -InitialAngle + RotationAngle, 0);
		}
	}

	// OPEN FUNCTION
	public IEnumerator Open ()
    {
		if (n < TimesMoveable || TimesMoveable == 0)
		{
			//Change state from 1 to 0 and back (= alternate between FinalRot and InitialRot).
			if (hinge.transform.rotation == (State == 0 ? FinalRot : InitialRot)) State ^= 1;

			//Set 'FinalRotation' to 'FinalRot' when moving and to 'InitialRot' when moving back.
			Quaternion FinalRotation = ((State == 0) ? FinalRot : InitialRot);

    	//Make the door/window rotate until it is fully opened/closed.
    	while (Mathf.Abs(Quaternion.Angle(FinalRotation, hinge.transform.rotation)) > 0.01f)
    	{
				Running = true;
				hinge.transform.rotation = Quaternion.Lerp (hinge.transform.rotation, FinalRotation, Time.deltaTime * Speed);

      	yield return new WaitForEndOfFrame();
    	}

			Running = false;

			if (TimesMoveable == 0) n = 0;

			else n++;
		}
	}
}
