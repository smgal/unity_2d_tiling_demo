
using System;
using System.Collections.Generic;

using UnityEngine;
using LitJson;

namespace nd
{
	public static class map
	{
		public static bool Load(string map_name, string file_name, ref Map ref_map)
		{
			TextAsset json_file = Resources.Load("Text/" + file_name) as TextAsset;

			if (json_file == null)
				return false;

			JsonData json_map = JsonMapper.ToObject(json_file.text);

			if (json_map == null)
			{
				Debug.LogWarning(String.Format("Unable to load '{0}' as map format", file_name));
				return false;
			}

			int map_size_w = Convert.ToInt32(json_map["width"].ToString());
			int map_size_h = Convert.ToInt32(json_map["height"].ToString());

			Map map = new Map(new nd.type.Size { w = map_size_w, h = map_size_h });

			map.map_name = map_name;
			map.file_name = file_name;
			map.byname = json_map["displayName"].ToString();

			map.events = new Dictionary<int, string>();

			Array.Clear(map.data, 0, map.data.Length);

			int map_size = map_size_w * map_size_h;
			int map_pitch = map_size_w;
			int num_map_layer = json_map["data"].Count / map_size;

			const int RM_MAP_LAYER_TILE = 0;
			const int RM_MAP_LAYER_OBJECT_LOWER = 2;
			const int RM_MAP_LAYER_OBJECT = 3;
			const int RM_MAP_LAYER_SHADOW = 4; // TL:1 TR:2 BL:4 BR:8
			const int RM_MAP_LAYER_EVENT = 5;

			for (int layer = 0; layer < num_map_layer; layer++)
			{
				switch (layer)
				{
					case RM_MAP_LAYER_TILE:
						for (int y = 0; y < map_size_h; y++)
							for (int x = 0; x < map_size_w; x++)
							{
								int data = Convert.ToInt32(json_map["data"][layer * map_size + y * map_pitch + x].ToString());
								data = (data < 0x600) ? data : data - 0x600;

								map.data[x, y] = data;
							}
						break;

					case RM_MAP_LAYER_OBJECT_LOWER:
						for (int y = 0; y < map_size_h; y++)
							for (int x = 0; x < map_size_w; x++)
							{
								int data = Convert.ToInt32(json_map["data"][layer * map_size + y * map_pitch + x].ToString());
								//map.data[x, y].ix_obj0 = data;
							}
						break;

					case RM_MAP_LAYER_OBJECT:
						for (int y = 0; y < map_size_h; y++)
							for (int x = 0; x < map_size_w; x++)
							{
								int data = Convert.ToInt32(json_map["data"][layer * map_size + y * map_pitch + x].ToString());
								map.data[x, y] |= (data << 16);
							}
						break;

					case RM_MAP_LAYER_SHADOW:
						for (int y = 0; y < map_size_h; y++)
							for (int x = 0; x < map_size_w; x++)
							{
								int data = Convert.ToInt32(json_map["data"][layer * map_size + y * map_pitch + x].ToString());
								//map.data[x, y].shadow = data;
							}
						break;

					case RM_MAP_LAYER_EVENT:
						for (int y = 0; y < map_size_h; y++)
							for (int x = 0; x < map_size_w; x++)
							{
								int data = Convert.ToInt32(json_map["data"][layer * map_size + y * map_pitch + x].ToString());
								//map.data[x, y].ix_event = (data > 0) ? (EVENT_BIT.TYPE_EVENT | data) : EVENT_BIT.NONE;
							}
						break;

				}
			}

			var event_list = json_map["events"];
			if (event_list != null)
			{
				for (int i = 0; i < event_list.Count; i++)
				{
				}
			}

			ref_map = map;

			return true;
		}
	}
}
