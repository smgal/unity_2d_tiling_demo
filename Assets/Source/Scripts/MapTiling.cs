using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;

public class MapTiling: MonoBehaviour
{
	public GameObject prefabTileContainer;
	public GameObject prefabTile;
	public Sprite tempSprite;

	// map renderer
	private GameObject _tileContainer;
	private List<GameObject> _tiles = new List<GameObject>();

	void Start()
	{
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
		_tileContainer = LeanPool.Spawn(prefabTileContainer);

		{
			float x = 100.0f;
			float y = 100.0f;
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
