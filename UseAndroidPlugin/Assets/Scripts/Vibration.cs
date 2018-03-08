using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration{

	private AndroidJavaObject vibratorPlugin;

	public const int MAX_AMPLITUDE = 255;

	public Vibration()
	{
		#if !UNITY_EDITOR && UNITY_ANDROID
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		vibratorPlugin = new AndroidJavaObject("xyz.peke2.myfirstplugin.Vibration", context);
		#endif
	}

	public bool hasAmplitudeControl()
	{
		#if !UNITY_EDITOR && UNITY_ANDROID
		return vibratorPlugin.Call<bool>("hasAmplitudeControl");
		#else
		return false;
		#endif
	}

	public void oneShot(long milliseconds, int amplitude)
	{
		#if !UNITY_EDITOR && UNITY_ANDROID
		vibratorPlugin.Call("oneShot", milliseconds, amplitude);
		#endif
	}

	public void vibrate(long[] timings, int[] amplitudes)
	{
		#if !UNITY_EDITOR && UNITY_ANDROID
		//	-1 : リピートなし
		//	リピート時はタイミング配列のインデックスを指定(繰り返しなら0でOK？)
		vibratorPlugin.Call("vibrate", timings, amplitudes, -1);
		#endif
	}

	public void cancel()
	{
		#if !UNITY_EDITOR && UNITY_ANDROID
		vibratorPlugin.Call("cancel");
		#endif
	}
}
