using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;

public class MapTiling: MonoBehaviour
{
	public GameObject parent;
	public GameObject anchor;
	public GameObject prefabTileContainer;
	public GameObject prefabTile;
	public Sprite tempSprite;
	public float offsetX = 0.0f;
	public float offsetY = 0.0f;

	// map renderer
	private GameObject _tileContainer;
	private List<GameObject> _tiles = new List<GameObject>();

	void Start()
	{
		float VIEW_W = anchor.GetComponent<RectTransform>().rect.width * 2;
		float VIEW_H = anchor.GetComponent<RectTransform>().rect.height * 2;

		float TILE_W = 24;
		float TILE_H = 24;

		// Anchor의 중심
		offsetX = VIEW_W / 2;
		offsetY = -VIEW_H / 2;

		Vector3 scale = prefabTile.transform.localScale;
		// 2000.0f / 24.0f 이면 전체 크기
		// (256.0f / 24.0f) 전체 가로에 들어가는 타일 개수
		scale.x = VIEW_W / TILE_W / (256.0f / TILE_W);
		scale.y = VIEW_H / TILE_H / (256.0f / TILE_H);

		prefabTile.transform.localScale = scale;
	}

	void Update()
	{
		DisplayMainMap();
	}

	private void DisplayMainMap()
	{
		foreach (GameObject o in _tiles)
		{
			LeanPool.Despawn(o);
		}

		_tiles.Clear();

		LeanPool.Despawn(_tileContainer);

		float ANCHOR_X = anchor.transform.position.x;
		float ANCHOR_Y = anchor.transform.position.y;

		_tileContainer = LeanPool.Spawn(prefabTileContainer);

		{
			float x = ANCHOR_X + offsetX;
			float y = ANCHOR_Y + offsetY;
			float z = 0.0f;

			var t = LeanPool.Spawn(prefabTile);
			t.transform.position = new Vector3(x, y, z);
			t.transform.SetParent(_tileContainer.transform);

			var renderer = t.GetComponent<SpriteRenderer>();
			renderer.sprite = this.tempSprite;

			_tiles.Add(t);
		}

	}
}
