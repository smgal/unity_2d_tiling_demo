
using System;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputDevice: MonoBehaviour
{
	public enum KEY
	{
		UP,
		DOWN,
		LEFT,
		RIGHT,
	};

	private static bool[] _key_is_pressing = new bool[Enum.GetValues(typeof(KEY)).Length];

	public static bool IsKeyPressing(KEY key)
	{
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

	////////////////////////////////////////////////////////////////////////////

	public Image target_image;

	private bool _on_pressing = false;
	private Vector2 _pressed_pos;

	public void OnKeyDown()
	{
		Debug.Log("-- OnKeyDown()");
	}

	void Update()
	{
		int count = Input.touchCount;

		for (int i = 0; i < count; i++)
		{
			if (Input.GetTouch(i).phase == TouchPhase.Began)
				{ }
			else if (Input.GetTouch(i).phase == TouchPhase.Moved)
				{ }
			else if (Input.GetTouch(i).phase == TouchPhase.Ended)
				_TouchUp();
			else if (Input.GetTouch(i).phase == TouchPhase.Canceled)
				_TouchUp();
		}

		if (Input.GetMouseButtonUp(0))
			_TouchUp();
	}

	public void TouchDown()
	{
		_TouchDown();
	}

	private void _TouchDown()
	{
		_pressed_pos = Input.mousePosition;
		_on_pressing = true;

		Vector3 local_pos = target_image.transform.InverseTransformPoint(_pressed_pos.x, _pressed_pos.y, 0.0f);
		// debug_text.text = String.Format("({0:D3},{1:D3})", (int)local_pos.x, (int)local_pos.y);

		/*  normalized_local_pos
		 *  
		 *  (-1, 1)     ( 1, 1)
		 *    +------------+
		 *    |            |
		 *    |            |
		 *    |  <Target>  |
		 *    |            |
		 *    |            |
		 *    +------------+
		 *  (-1,-1)     ( 1,-1)
		 */

		if (Math.Abs(local_pos.x) > Math.Abs(local_pos.y))
		{
			if (local_pos.x >= 0.0f)
				_key_is_pressing[(int)KEY.RIGHT] = true;
			else
				_key_is_pressing[(int)KEY.LEFT] = true;
		}
		else
		{
			if (local_pos.y >= 0.0f)
				_key_is_pressing[(int)KEY.UP] = true;
			else
				_key_is_pressing[(int)KEY.DOWN] = true;
		}
	}

	private void _TouchUp()
	{
		_on_pressing = false;

		_key_is_pressing[(int)KEY.RIGHT] = false;
		_key_is_pressing[(int)KEY.LEFT] = false;
		_key_is_pressing[(int)KEY.UP] = false;
		_key_is_pressing[(int)KEY.DOWN] = false;
	}
}
