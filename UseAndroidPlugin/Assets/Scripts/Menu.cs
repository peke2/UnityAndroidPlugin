using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

	Dropdown dropdown;
	Button buttonGo;
	string selecting = null;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);


		GameObject gobj;
		gobj = GameObject.Find("DropdownMenuSelector");
		dropdown = gobj.GetComponent<Dropdown>();

		gobj = GameObject.Find("Button");
		buttonGo = gobj.GetComponent<Button>();
		buttonGo.onClick.AddListener(onButton);

		changeScene();
	}


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	string getSelectingName(Dropdown dd)
	{
		Dropdown.OptionData data = dd.options[dd.value];
		return data.text;
	}

	void changeScene()
	{
		string name = getSelectingName(dropdown);

		//	今のシーン名と同じなら何もしない
		if (selecting != null && selecting == name) {
			return;
		}

		selecting = name;
		SceneManager.LoadScene(selecting);
	}

	void onButton()
	{
		changeScene();
	}
}
