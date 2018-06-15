
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

	////////////////////////

	private const int NUM_TILES_X_RADIUS = 4;
	private const int NUM_TILES_Y_RADIUS = 4;

	private const float TILE_W = 48;
	private const float TILE_H = 48;

	private Vector3 _SCALE;

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

			for (int dy = -NUM_TILES_Y_RADIUS-1; dy <= NUM_TILES_Y_RADIUS+1; dy++)
			{
				for (int dx = -NUM_TILES_X_RADIUS-1; dx <= NUM_TILES_X_RADIUS+1; dx++)
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
			int count = 0;
			for (int y = 0; y < 50; y++)
			for (int x = 0; x < 50; x++)
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
		for (int dy = -5; dy <= 5; dy++)
		for (int dx = -5; dx <= 5; dx++)
		{
			_tile_renderer[_GetGridIndex(dx, dy)].sprite = sprite_list[_map[_party_x + dx, _party_y + dy]];
		}
	}


	private const float _SPEED = 500.0f;

	private Vector3 _grid_pos;
	private Transform _grid_tr;

	private bool _is_left_key_down = false;
	private bool _is_right_key_down = false;
	private bool _is_up_key_down = false;
	private bool _is_down_key_down = false;

	private int[,] _map = new int[50, 50];
	private int _party_x = 25;
	private int _party_y = 25;
	private int _party_dx = 0;
	private int _party_dy = 0;

	void Update()
	{
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
				_is_right_key_down = true;
			if (Input.GetKeyUp(KeyCode.RightArrow))
				_is_right_key_down = false;

			if (Input.GetKeyDown(KeyCode.LeftArrow))
				_is_left_key_down = true;
			if (Input.GetKeyUp(KeyCode.LeftArrow))
				_is_left_key_down = false;

			if (Input.GetKeyDown(KeyCode.UpArrow))
				_is_up_key_down = true;
			if (Input.GetKeyUp(KeyCode.UpArrow))
				_is_up_key_down = false;

			if (Input.GetKeyDown(KeyCode.DownArrow))
				_is_down_key_down = true;
			if (Input.GetKeyUp(KeyCode.DownArrow))
				_is_down_key_down = false;
		}

		if (_is_right_key_down && _grid_tr.position == _grid_pos)
		{
			//_player_anim.SetTrigger("PlayerTurnsRight");
			_grid_pos += new Vector3(-TILE_H * _SCALE.y, 0.0f);
			_party_dx = 1;
		}
		else if (_is_left_key_down && _grid_tr.position == _grid_pos)
		{
			//_player_anim.SetTrigger("PlayerTurnsLeft");
			_grid_pos += new Vector3(+TILE_H * _SCALE.y, 0.0f);
			_party_dx = -1;
		}
		else if (_is_up_key_down && _grid_tr.position == _grid_pos)
		{
			//_player_anim.SetTrigger("PlayerTurnsUp");
			_grid_pos += new Vector3(0.0f, -TILE_H * _SCALE.x);
			_party_dy = -1;
		}
		else if (_is_down_key_down && _grid_tr.position == _grid_pos)
		{
			//_player_anim.SetTrigger("PlayerTurnsDown");
			_grid_pos += new Vector3(0.0f, +TILE_H * _SCALE.x);
			_party_dy = 1;
		}

		if (_grid_tr.position != _grid_pos)
		{
			_grid_tr.position = Vector3.MoveTowards(_grid_tr.position, _grid_pos, Time.deltaTime * _SPEED);

			if (_grid_tr.position == _grid_pos)
			{
				_grid_tr.position = _grid_pos = Vector3.zero;

				_party_x += _party_dx;
				_party_y += _party_dy;

				_party_dx = 0;
				_party_dy = 0;

				_UpdateGrid();
			}
		}
	}
}
