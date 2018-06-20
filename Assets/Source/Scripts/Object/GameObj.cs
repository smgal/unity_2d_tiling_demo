
using System.Collections.Generic;

using UnityEngine;

public class GameObj: MonoBehaviour
{
	/*
		public GameObject widget_window_frame;
		public GameObject widget_bg_tiles;
		public GameObject widget_obj_tiles;
		public GameObject widget_fg_tiles;
	*/
	public List<GameObject> widget_resizable;
	public GameObject widget_player;

	public static Player player;
	public static Map map;

	void Awake()
	{
		player = new Player();
		player.Warp(10, 10);
		player.SetDiretion(0, 1);

		nd.map.Load("TEST", "Map001", ref map);
	}

	void Start()
	{
		// 가로가 더 긴 경우 대응 (예를 들어, iPhoneX = 1127x2346)
		if (CONFIG.GUI_SCALE < 1.0f)
		{
			foreach (var widget in widget_resizable)
			{
				Vector3 local_scale = widget.transform.localScale;

				local_scale.x *= (float)CONFIG.GUI_SCALE;
				local_scale.y *= (float)CONFIG.GUI_SCALE;

				widget.transform.transform.localScale = local_scale;

				// TODO: break가 아니라면 prefab_bg_tile.transform.localScale = _SCALE; 와 동일한가?
				break;
			}
			{
				Vector3 local_scale = widget_player.transform.localScale;

				local_scale.x *= (float)CONFIG.GUI_SCALE;
				local_scale.y *= (float)CONFIG.GUI_SCALE;

				widget_player.transform.transform.localScale = local_scale;

				Vector3 local_position = widget_player.transform.localPosition;
				local_position.x *= CONFIG.GUI_SCALE;
				local_position.y *= CONFIG.GUI_SCALE;

				widget_player.transform.localPosition = local_position;
			}

		}
	}
}
