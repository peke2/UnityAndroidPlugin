using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sound : MonoBehaviour {

	Button buttonPlay;
	Button buttonStop;
	Dropdown dropdownSoundList;

	Dictionary<string , AudioClip>	audioList;
	Dictionary<string, Param> paramList;
	AudioSource audioSource;

	Vibration vibration;
	bool hasAmplitudeControl;


	// Use this for initialization
	void Start () {
		GameObject gobj;
		gobj = GameObject.Find("ButtonPlay");
		buttonPlay = gobj.GetComponent<Button>();
		gobj = GameObject.Find("ButtonStop");
		buttonStop = gobj.GetComponent<Button>();

		buttonPlay.onClick.AddListener(onButtonPlay);
		buttonStop.onClick.AddListener(onButtonStop);
	
		gobj = GameObject.Find("Dropdown");
		dropdownSoundList = gobj.GetComponent<Dropdown>();

		audioList = new Dictionary<string, AudioClip>();
		paramList = new Dictionary<string, Param>();

		vibration = new Vibration();
		hasAmplitudeControl = vibration.hasAmplitudeControl();

		AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds");
		List<string> list = new List<string>();
		foreach (AudioClip ac in clips)
		{
			string name = ac.name;
			audioList[name] = ac;
			paramList[name] = createParam(ac, 10, hasAmplitudeControl);
			list.Add(ac.name);
		}
		dropdownSoundList.AddOptions(list);

		audioSource = gameObject.AddComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onButtonPlay()
	{
		Dropdown.OptionData optdata;
		optdata = dropdownSoundList.options[dropdownSoundList.value];
		string name = optdata.text;

		AudioClip clip = audioList[name];

		audioSource.clip = clip;
		audioSource.Play();

		Param param = paramList[name];
		vibration.vibrate(param.timings, param.amplitudes);
			
	}

	void onButtonStop()
	{
		audioSource.Stop();
		vibration.cancel();
	}

	public class Param
	{
		public long[] timings;
		public int[] amplitudes;

		public Param(int size)
		{
			timings = new long[size];
			amplitudes = new int[size];
		}
	}


	/// <summary>
	/// 振動用の強さを計算
	/// </summary>
	/// <returns>振動に渡す値</returns>
	/// <param name="v">1〜-1の値</param>
	public static float clampVibration(float v, int amplitude, bool hasAmplitudeControl)
	{
		v = Mathf.Abs(v);
		v = Mathf.Clamp01(v);

		if( hasAmplitudeControl == false )
		{
			//	ON/OFFしかないので、どちらかに寄せる
			if (v > 0.5f) {
				v = 1;
			} else {
				v = 0;
			}
		}
		v *= amplitude;
		return v;
	}

	public static Param createParam(AudioClip clip, int stepMilliseconds, bool hasAmplitudeControl)
	{
		//	各サンプル間の秒数から指定されたステップ時間のサンプル数を求める
		float oneSampleMilliSec = 1.0f / clip.frequency * 1000;
		int step = (int)((float)stepMilliseconds / oneSampleMilliSec);

		float sec = (float)clip.samples / clip.frequency;
		Debug.Log("["+clip.name+"]="+sec.ToString()+"秒");

		int size = clip.samples * clip.channels;
		float[] samples = new float[size];
		clip.GetData(samples, 0);

		int count = clip.samples / step;
		Param param = new Param(count);


		float total_vib_sec = (float)(count * stepMilliseconds)/1000.0f;
		Debug.Log("total="+total_vib_sec.ToString()+"秒");

		for(int i=0; i<count; i++) {
			float v;
			v = samples[i*clip.channels*step];	//	ステレオの場合「LRLR」の順番でデータが入っている

			v = clampVibration(v, Vibration.MAX_AMPLITUDE, hasAmplitudeControl);

			param.amplitudes[i] = (int)v;
			param.timings[i] = stepMilliseconds;
		}

		return param;
	}


	public static Param createParamFrequency(float hertz, int milliseconds, int amplitude, int stepMilliseconds, bool hasAmplitudeControl)
	{
		int length = milliseconds / stepMilliseconds;	//	端数は切り捨て
		Param param = new Param(length);

		float cycleMsec = milliseconds / hertz;

		int amp = Mathf.Clamp(amplitude, 0, Vibration.MAX_AMPLITUDE);

		for (int i = 0; i < length; i++) {
			float t = (float)(i*stepMilliseconds) / cycleMsec;
			float rad = 2 * Mathf.PI * t;
			float r = Mathf.Sin(rad);

			r = clampVibration(r, amp, hasAmplitudeControl);

			param.timings[i] = stepMilliseconds;
			param.amplitudes[i] = (int)r;
		}

		return param;
	}
}


