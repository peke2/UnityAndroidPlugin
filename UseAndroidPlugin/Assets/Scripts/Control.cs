using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Control : MonoBehaviour {

	InputField inputAmplitude;
	InputField inputMillideconds;
	InputField inputHertz;
	Button buttonOneShot;
	Button buttonCancel;
	Button buttonHertz;
	Button buttonCancelHertz;
	Text textHasAmplitude;

	Vibration vibration;

	// Use this for initialization
	void Start () {
		GameObject gobj;
		gobj = GameObject.Find("InputFieldAmplitude");
		inputAmplitude = gobj.GetComponent<InputField>();

		gobj = GameObject.Find("InputFieldMilliseconds");
		inputMillideconds = gobj.GetComponent<InputField>();

		gobj = GameObject.Find("InputFieldHertz");
		inputHertz = gobj.GetComponent<InputField>();

		gobj = GameObject.Find("ButtonOneShot");
		buttonOneShot = gobj.GetComponent<Button>();
		buttonOneShot.onClick.AddListener(onOneShot);

		gobj = GameObject.Find("ButtonCancel");
		buttonCancel = gobj.GetComponent<Button>();
		buttonCancel.onClick.AddListener(onCancel);

		gobj = GameObject.Find("ButtonHertz");
		buttonHertz = gobj.GetComponent<Button>();
		buttonHertz.onClick.AddListener(onHertz);

		gobj = GameObject.Find("ButtonCancelHertz");
		buttonCancelHertz = gobj.GetComponent<Button>();
		buttonCancelHertz.onClick.AddListener(onCancel);

		gobj = GameObject.Find("TextHasAmplitude");
		textHasAmplitude = gobj.GetComponent<Text>();

		vibration = new Vibration();

		textHasAmplitude.text = "hasAmplitudeControl:" + ((vibration.hasAmplitudeControl()==true)? "true":"false");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	int getAmplitude()
	{
		int amp;
		if (!Int32.TryParse(inputAmplitude.text, out amp)) {
			amp = 1;
		}

		return amp;
	}


	void onOneShot()
	{
		int amp, ms;
		amp = getAmplitude();

		if (!Int32.TryParse(inputMillideconds.text, out ms)) {
			ms = 0;
		}

		amp = Mathf.Clamp(amp, 1, 255);

		Debug.Log("oneShot("+ms.ToString()+","+amp.ToString()+")");
		vibration.oneShot(ms, amp);
	}

	void onCancel()
	{
		Debug.Log("cancel()");
		vibration.cancel();
	}

	void onHertz()
	{
		float hertz;
		int ms;
		if (!float.TryParse(inputHertz.text, out hertz)) {
			hertz = 1;
		}
		if (!Int32.TryParse(inputMillideconds.text, out ms)) {
			ms = 0;
		}
		int amp;
		amp = getAmplitude();

		hertz = Mathf.Max(hertz, 1);

		bool hasAmp = vibration.hasAmplitudeControl();

		Debug.Log("hertz("+ms.ToString()+","+hertz.ToString()+")");
		Sound.Param param;
		param = Sound.createParamFrequency(hertz, ms, amp, 1, hasAmp);
		vibration.vibrate(param.timings, param.amplitudes);
	}

}
