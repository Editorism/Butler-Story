using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	public  Button startConfirmation;

	// Use this for initialization
	void Start () {
		
		Button btn = startConfirmation.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);

	}
	
	// Update is called once per frame
	void TaskOnClick () {
		
		SceneManager.LoadSceneAsync("Level_01");

	}
		
}
