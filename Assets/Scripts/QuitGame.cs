using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour {

	public  Button quitConfirmation;
    public  Button menuConfirmation;
	// Use this for initialization
	void Start () {
		
		Button gameQuitBtn = quitConfirmation.GetComponent<Button> ();
        gameQuitBtn.onClick.AddListener (QuitButtonTaskOnClick);

        Button menuQuitBtn = menuConfirmation.GetComponent<Button>();
        menuQuitBtn.onClick.AddListener(MenuButtonTaskOnClick);

    }
	
	// Update is called once per frame
	void QuitButtonTaskOnClick () {

		Application.Quit ();
	}

    void MenuButtonTaskOnClick()
    {
        SceneManager.LoadSceneAsync("Main_Menu");
    }
}
