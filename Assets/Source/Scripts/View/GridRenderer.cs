
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Lean;

public class GridRenderer: MonoBehaviour
{
	public GameObject anchor;
	public GameObject player;
	public Animator   player_anim;
	public GameObject prefab_tile_container;
	public GameObject prefab_bg_tile;
	public GameObject prefab_obj_tile;
	public GameObject prefab_fg_tile;

	public List<Sprite> sprite_list;

	// map renderer
	private const int _MAX_LAYER = 3;
	private GameObject _tile_container;
	private List<GameObject> _tiles = new List<GameObject>();
	private List<SpriteRenderer>[] _tile_renderer = new List<SpriteRenderer>[_MAX_LAYER] { new List<SpriteRenderer>(), new List<SpriteRenderer>(), new List<SpriteRenderer>() } ;

	///////////////////////////////////////////////////////////////////////////////
	// 상수 정의

	// 실제 표시되는
	private const int NUM_TILES_X_RADIUS = 4;
	private const int NUM_TILES_Y_RADIUS = 4;
	// 실제 출력하는
	private const int NUM_TILES_X_EXTENDED = (NUM_TILES_X_RADIUS + 1);
	private const int NUM_TILES_Y_EXTENDED = (NUM_TILES_Y_RADIUS + 1);

	private const float TILE_W = 48;
	private const float TILE_H = 48;

	private const float _SPEED = 500.0f;
	private Vector3 _SCALE;

	private Vector3 _grid_pos;
	private Transform _grid_tr;

	// Use this for initialization
	void Start()
	{
		Debug.Assert((anchor.transform as RectTransform).pivot.x == 0.5);
		Debug.Assert((anchor.transform as RectTransform).pivot.y == 0.5);

		Vector2 VIEW_CENTER = new Vector2
		(
			player.transform.position.x,
			player.transform.position.y
		);

		_SCALE = new Vector3
		(
			anchor.GetComponent<RectTransform>().rect.width / TILE_W / (2 * NUM_TILES_X_RADIUS + 1),
			anchor.GetComponent<RectTransform>().rect.height / TILE_H / (2 * NUM_TILES_Y_RADIUS + 1)
		);

		_SCALE *= CONFIG.GUI_SCALE;

		{
			prefab_bg_tile.transform.localScale = _SCALE;
			prefab_obj_tile.transform.localScale = _SCALE;
			prefab_fg_tile.transform.localScale = _SCALE;
		}
		
		{
			Vector3 local_position = player.transform.localPosition;
			local_position.y += 60 * CONFIG.GUI_SCALE;
			player.transform.localPosition = local_position;
		}

		LeanPool.Despawn(_tile_container);
		_tile_container = LeanPool.Spawn(prefab_tile_container);

		{
			Vector2 TILE_SCALE = new Vector2
			(
				TILE_W * _SCALE.x,
				TILE_H * _SCALE.y
			);

			const float Z_INIT = 1.0f;
			const float Z_GAP_LAYER = -0.2f;
			const float Z_GAP_VERTICAL = -0.01f;

			for (int layer = 0; layer < _MAX_LAYER; layer++)
			{
				GameObject prefab_tile = (layer == 0) ? prefab_bg_tile : ((layer == 1) ? prefab_obj_tile : prefab_fg_tile);

				float z = Z_INIT + Z_GAP_LAYER * layer;

				for (int dy = -NUM_TILES_Y_EXTENDED; dy <= NUM_TILES_Y_EXTENDED; dy++)
				{
					for (int dx = -NUM_TILES_X_EXTENDED; dx <= NUM_TILES_X_EXTENDED; dx++)
					{
						float x = VIEW_CENTER.x + dx * TILE_SCALE.x;
						float y = VIEW_CENTER.y - dy * TILE_SCALE.y;

						var tile = LeanPool.Spawn(prefab_tile);
						tile.transform.position = new Vector3(x, y, z);
						tile.transform.SetParent(_tile_container.transform);

						var renderer = tile.GetComponent<SpriteRenderer>();
						renderer.sprite = sprite_list[0];

						_tiles.Add(tile);
						_tile_renderer[layer].Add(renderer);
					}

					z += Z_GAP_VERTICAL;
				}
			}
		}

		_grid_tr = _tile_container.transform;
		_grid_pos = _tile_container.transform.position;

		_UpdateGrid();
	}

	private int _GetGridIndex(int dx, int dy)
	{
		int offset_x = NUM_TILES_X_RADIUS + 1;
		int offset_y = NUM_TILES_Y_RADIUS + 1;
		int pitch_x = 2 * offset_x + 1;
		int pitch_y = 2 * offset_x + 1;

		int index = pitch_x * (offset_y + dy) + (offset_x + dx);

		return (index >= 0) && (index < pitch_x * pitch_y) ? index : 0;
	}

	private void _UpdateGrid()
	{
		int _party_x = GameObj.player.Pos.x;
		int _party_y = GameObj.player.Pos.y;

		for (int dy = -NUM_TILES_Y_EXTENDED; dy <= NUM_TILES_Y_EXTENDED; dy++)
		for (int dx = -NUM_TILES_X_EXTENDED; dx <= NUM_TILES_X_EXTENDED; dx++)
		{
			int ix = GameObj.map[_party_x + dx, _party_y + dy];
			int ix_tile = ix & 0xFFFF;
			int ix_sprite = (ix >> 16) & 0xFFFF;

			int[] ix_layer = new int[_MAX_LAYER] { -1, -1, -1 };

			// Sprite
			if (ix_sprite > 0)
			{
				if (TileInfo.GetOrder(TileInfo.TYPE.SPRITE, ix_sprite) <= 0)
					ix_layer[1] = 128 + ix_sprite;
				else
					ix_layer[2] = 128 + ix_sprite;
			}

			// Tile
			{
				if (TileInfo.GetOrder(TileInfo.TYPE.TILE, ix_tile) <= 0)
					ix_layer[0] = ix_tile;
				else
					ix_layer[2] = ix_tile;
			}
			
			for (int layer = 0; layer < _MAX_LAYER; layer++)
				_tile_renderer[layer][_GetGridIndex(dx, dy)].sprite = (ix_layer[layer] >= 0) ? sprite_list[ix_layer[layer]] : null;
		}
	}

	void Update()
	{
		if (_grid_tr.position == _grid_pos)
		{
			int _party_dx = 0;
			int _party_dy = 0;

			if (InputDevice.IsKeyPressing(InputDevice.KEY.RIGHT))
			{
				_grid_pos += new Vector3(-TILE_H * _SCALE.y, 0.0f);
				_party_dx = 1;
			}
			else if (InputDevice.IsKeyPressing(InputDevice.KEY.LEFT))
			{
				_grid_pos += new Vector3(+TILE_H * _SCALE.y, 0.0f);
				_party_dx = -1;
			}
			else if (InputDevice.IsKeyPressing(InputDevice.KEY.UP))
			{
				_grid_pos += new Vector3(0.0f, -TILE_H * _SCALE.x);
				_party_dy = -1;
			}
			else if (InputDevice.IsKeyPressing(InputDevice.KEY.DOWN))
			{
				_grid_pos += new Vector3(0.0f, +TILE_H * _SCALE.x);
				_party_dy = 1;
			}

			if (_party_dx != 0 || _party_dy != 0)
			{
				if (GameObj.player.Diretion.dx != _party_dx || GameObj.player.Diretion.dy != _party_dy)
				{
					if (_party_dy != 0)
						player_anim.SetTrigger((_party_dy >= 0) ? "PlayerTurnsDown" : "PlayerTurnsUp");
					else
						player_anim.SetTrigger((_party_dx >= 0) ? "PlayerTurnsRight" : "PlayerTurnsLeft");
				}

				GameObj.player.SetDiretion(_party_dx, _party_dy);

				int ix = GameObj.map[GameObj.player.Pos.x + _party_dx, GameObj.player.Pos.y + _party_dy];
				int ix_tile = ix & 0xFFFF;
				int ix_sprite = (ix >> 16) & 0xFFFF;

				if (TileInfo.GetOrder(TileInfo.TYPE.TILE, ix_tile) == 0 ||
				    TileInfo.GetOrder(TileInfo.TYPE.SPRITE, ix_sprite) == 0)
				{
					// Can't move
					_grid_pos = _grid_tr.position;
				}
			}
		}

		if (_grid_tr.position != _grid_pos)
		{
			_grid_tr.position = Vector3.MoveTowards(_grid_tr.position, _grid_pos, Time.deltaTime * _SPEED);

			if (_grid_tr.position == _grid_pos)
			{
				_grid_tr.position = _grid_pos = Vector3.zero;

				GameObj.player.Move(GameObj.player.Diretion);

				_UpdateGrid();
			}
		}
	}
}
