
using System;
using UnityEngine;

public static class InputDevice
{
	public enum KEY
	{
		UP,
		DOWN,
		LEFT,
		RIGHT,
		MAX
	};

	private static bool[] _key_is_pressing = new bool[(int)KEY.MAX];

	public static bool IsKeyPressing(KEY key)
	{
		var a = Enum.GetValues(typeof(KEY)).Length;
		return _key_is_pressing[(int)key];
	}

	public static void Process()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
			_key_is_pressing[(int)KEY.RIGHT] = true;
		if (Input.GetKeyUp(KeyCode.RightArrow))
			_key_is_pressing[(int)KEY.RIGHT] = false;

		if (Input.GetKeyDown(KeyCode.LeftArrow))
			_key_is_pressing[(int)KEY.LEFT] = true;
		if (Input.GetKeyUp(KeyCode.LeftArrow))
			_key_is_pressing[(int)KEY.LEFT] = false;

		if (Input.GetKeyDown(KeyCode.UpArrow))
			_key_is_pressing[(int)KEY.UP] = true;
		if (Input.GetKeyUp(KeyCode.UpArrow))
			_key_is_pressing[(int)KEY.UP] = false;

		if (Input.GetKeyDown(KeyCode.DownArrow))
			_key_is_pressing[(int)KEY.DOWN] = true;
		if (Input.GetKeyUp(KeyCode.DownArrow))
			_key_is_pressing[(int)KEY.DOWN] = false;
	}
}
