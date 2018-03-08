using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hello : MonoBehaviour {

	// Use this for initialization
	void Start () {
		using (AndroidJavaObject jc = new AndroidJavaObject("xyz.peke2.myfirstplugin.Hello"))
		{
			string word = jc.CallStatic<string>("say");
			Debug.Log("plugintest:say()=>"+word);
			int sum = jc.CallStatic<int>("add", 4,99);
			Debug.Log("plugintest:add(4,99)=>"+sum.ToString());
		
			using (AndroidJavaObject param = new AndroidJavaObject("xyz.peke2.myfirstplugin.HelloParam")) {
				param.Set<int[]>("values", new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
				param.Set<string>("name", "Parameter");
				//int total = jc.CallStatic<int>("sum", param);
				//Debug.Log("plugintest:sum=>" + total.ToString());
				//int[] values = param.Get<int[]>("values");
				//Debug.Log("plugintest:value=>" + values[3].ToString());
				//string name = jc.CallStatic<string>("getName", param);
				//Debug.Log("plugintest:name=>" + name);
			}
		}

		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		using (AndroidJavaObject vib = new AndroidJavaObject("xyz.peke2.myfirstplugin.Vibration", context)) {
			bool is_enabled = vib.Call<bool>("hasFunction");
			Debug.Log("plugintest:vib=>"+is_enabled.ToString());
			bool has_amplitude = vib.Call<bool>("hasAmplitudeControl");
			Debug.Log("plugintest:amplitude=>"+has_amplitude.ToString());

			vib.Call("oneShot", 3000, 16);
		}

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(1, 0, 0.5f);
	}
}
