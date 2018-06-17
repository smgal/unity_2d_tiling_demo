
using System;

using UnityEngine;
using UnityEngine.UI;

public class MainLoop : MonoBehaviour
{
	public Text text_debug_fps;
	public Text text_debug_pos;
	public Text text_debug_log;

	void Awake()
	{
		// DontDestroyOnLoad(...gameObject);
	}

	void Start()
	{
	}
	
	void Update()
	{
		_DisplayDebugFps();

		Controller.Process();

		{
			int party_x = GameObj.player.Pos.x;
			int party_y = GameObj.player.Pos.y;

			text_debug_pos.text = String.Format("({0:D2},{1:D2})", party_x, party_y);

			int ix = GameObj.map[party_x, party_y];
			int ix_tile = ix & 0xFFFF;
			int ix_sprite = (ix >> 16) & 0xFFFF;

			text_debug_log.text = String.Format("tile: {0}, sprite: {1}", ix_tile, ix_sprite);
		}
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
