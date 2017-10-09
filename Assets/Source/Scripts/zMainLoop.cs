using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zMainLoop : MonoBehaviour
{
	void Awake()
	{
		// DontDestroyOnLoad(...gameObject);
	}

	void Start ()
	{
		
	}
	
	void Update ()
	{
		
	}

	void OnApplicationQuit()
	{
		Debug.Log("Application ending after " + Time.time + " seconds");
#if !UNITY_EDITOR
		Application.CancelQuit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
	}

	public void OnMainMenuQuit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
