using UnityEngine;
using System.Collections;

public class cont_test : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		float hori = Input.GetAxis ("X axis");
		float vert = Input.GetAxis ("Y axis");
		if(( hori != 0) ||  (vert != 0) ){
			Debug.Log ("stick:"+hori+","+vert );
		}
	}

	void OnGUI () {
		GUILayout.Label(AxisLabel("X axis"));
		GUILayout.Label(AxisLabel("Y axis"));
		GUILayout.Label(AxisLabel("3rd axis"));
		GUILayout.Label(AxisLabel("6th axis"));
		GUILayout.Label(AxisLabel("7th axis"));
		GUILayout.Label(AxisLabel("8th axis"));
	
		GUILayout.Label(AxisLabel("joystick button 0"));
		GUILayout.Label(AxisLabel("joystick button 1"));
		GUILayout.Label(AxisLabel("joystick button 2"));
		GUILayout.Label(AxisLabel("joystick button 3"));
		GUILayout.Label(AxisLabel("joystick button 4"));
		GUILayout.Label(AxisLabel("joystick button 5"));
		GUILayout.Label(AxisLabel("joystick button 6"));
		GUILayout.Label(AxisLabel("joystick button 7"));
	}

	string AxisLabel(string axisName) {
		return axisName + ":" + Input.GetAxisRaw(axisName);
	}

	string ButtonLabel(string buttonName) {
		return buttonName + ":" + Input.GetButton(buttonName);
	}
}