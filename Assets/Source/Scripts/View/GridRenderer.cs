
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Lean;

public class GridRenderer: MonoBehaviour
{
	public GameObject anchor;
	public GameObject player;
	public GameObject prefab_tile_container;
	public GameObject prefab_tile;

	public List<Sprite> sprite_list;

	// map renderer
	private GameObject _tile_container;
	private List<GameObject> _tiles = new List<GameObject>();
	private List<SpriteRenderer> _tile_renderer = new List<SpriteRenderer>();

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

	///////////////////////////////////////////////////////////////////////////////
	// 다른 곳에 정의 되어야 함

	//private int[,] _map = new int[50, 50];
	private Player _player = new Player();
	private Map _map = null;

	// Use this for initialization
	void Start()
	{
		Debug.Assert((anchor.transform as RectTransform).pivot.x == 0.5);
		Debug.Assert((anchor.transform as RectTransform).pivot.y == 0.5);

		Rect VIEW_RECT = new Rect
		(
			anchor.transform.position.x,
			anchor.transform.position.y,
			anchor.GetComponent<RectTransform>().rect.width,
			anchor.GetComponent<RectTransform>().rect.height
		);

		Vector2 VIEW_CENTER = new Vector2
		(
			VIEW_RECT.x,
			VIEW_RECT.y
		);

		_SCALE = new Vector3
		(
			VIEW_RECT.width / TILE_W / (2 * NUM_TILES_X_RADIUS + 1),
			VIEW_RECT.height / TILE_H / (2 * NUM_TILES_Y_RADIUS + 1),
			1
		);

		{
			prefab_tile.transform.localScale = _SCALE;
		}

		{
			Vector3 pos = anchor.transform.position;
			pos.z = player.transform.position.z;
			player.transform.position = pos;

			player.transform.localScale = _SCALE;
		}

		LeanPool.Despawn(_tile_container);
		_tile_container = LeanPool.Spawn(prefab_tile_container);

		{
			Vector2 TILE_SCALE = new Vector2
			(
				TILE_W * _SCALE.x,
				TILE_H * _SCALE.y
			);

			float z = 0.0f;

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
					_tile_renderer.Add(renderer);
				}
			}
		}

		_grid_tr = _tile_container.transform;
		_grid_pos = _tile_container.transform.position;

		_tile_renderer[0].sprite = sprite_list[22];
		_tile_renderer[2].sprite = sprite_list[33];

		{
			_map = new Map(new nd.Size { w = 50, h = 50 });

			int count = 0;
			for (int y = 0; y < _map.Size.h; y++)
			for (int x = 0; x < _map.Size.w; x++)
			{
				_map[x, y] = count++ % 100;
			}

			_UpdateGrid();
		}
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
		int _party_x = _player.Pos.x;
		int _party_y = _player.Pos.y;

		for (int dy = -NUM_TILES_Y_EXTENDED; dy <= NUM_TILES_Y_EXTENDED; dy++)
		for (int dx = -NUM_TILES_X_EXTENDED; dx <= NUM_TILES_X_EXTENDED; dx++)
		{
			_tile_renderer[_GetGridIndex(dx, dy)].sprite = sprite_list[_map[_party_x + dx, _party_y + dy]];
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
				//_player_anim.SetTrigger("PlayerTurnsRight");
				_grid_pos += new Vector3(-TILE_H * _SCALE.y, 0.0f);
				_party_dx = 1;
			}
			else if (InputDevice.IsKeyPressing(InputDevice.KEY.LEFT))
			{
				//_player_anim.SetTrigger("PlayerTurnsLeft");
				_grid_pos += new Vector3(+TILE_H * _SCALE.y, 0.0f);
				_party_dx = -1;
			}
			else if (InputDevice.IsKeyPressing(InputDevice.KEY.UP))
			{
				//_player_anim.SetTrigger("PlayerTurnsUp");
				_grid_pos += new Vector3(0.0f, -TILE_H * _SCALE.x);
				_party_dy = -1;
			}
			else if (InputDevice.IsKeyPressing(InputDevice.KEY.DOWN))
			{
				//_player_anim.SetTrigger("PlayerTurnsDown");
				_grid_pos += new Vector3(0.0f, +TILE_H * _SCALE.x);
				_party_dy = 1;
			}

			if (_party_dx != 0 || _party_dy != 0)
				_player.SetDiretion(_party_dx, _party_dy);
		}

		if (_grid_tr.position != _grid_pos)
		{
			_grid_tr.position = Vector3.MoveTowards(_grid_tr.position, _grid_pos, Time.deltaTime * _SPEED);

			if (_grid_tr.position == _grid_pos)
			{
				_grid_tr.position = _grid_pos = Vector3.zero;

				_player.Move(_player.Diretion);

				_UpdateGrid();
			}
		}
	}
}
