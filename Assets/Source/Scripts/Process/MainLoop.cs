
using System;

using UnityEngine;
using UnityEngine.UI;

public class MainLoop : MonoBehaviour
{
	public Text text_debug_fps;

	void Awake()
	{
		// DontDestroyOnLoad(...gameObject);
	}

	void Start ()
	{
	}
	
	void Update ()
	{
		_DisplayDebugFps();

		Controller.Process();
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

	private int _fps_prev_second = -1;
	private int _fps_count = 0;

	private void _DisplayDebugFps()
	{
		if (text_debug_fps != null)
		{
			++_fps_count;

			int fps_current_second = (int)Time.realtimeSinceStartup;
			if (fps_current_second > _fps_prev_second)
			{
				text_debug_fps.text = String.Format("{0:F1}", 1.0f * _fps_count / (fps_current_second - _fps_prev_second));
				_fps_prev_second = fps_current_second;
				_fps_count = 0;
			}
		}
	}
}
