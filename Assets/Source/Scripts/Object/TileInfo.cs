using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileInfo
{
	private const int _DEFAULT_ORDER_OF_TILE = -2;
	private const int _DEFAULT_ORDER_OF_SPRITE = -1;

	// Order
	//
	//  2   <Be Floating By Perspective>
	//  1   <Object Above PC>
	//  0   <PC> <Rigid Object>
	// -1   <Object On Ground>
	// -2   <Ground>
	// -8   <Not Determined>
	//

	// TODO: Need to fill out this table later
	private static int[] _ORDER_OF_TILES =
	{//	 0  1  2  3  4  5  6  7  8  9
		-2,-2,-2,-2,-2,-2,-2,-2, 2, 0, // 000
		 2, 0, 2, 0, 2, 0, 0, 0, 0, 0, // 010
		 0, 0, 0, 0, 2, 0, 2, 0, 2, 0, // 020
		 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 030
	};

	// TODO: Need to fill out this table later
	private static int[] _ORDER_OF_SPRITES =
	{//	 0  1  2  3  4  5  6  7  8  9
		-8, 1, 1, 0,-1,-1,-1,-1,-1,-1, // 000
		 0, 1, 1, 1, 1, 1              // 010
	};

	public enum TYPE
	{
		TILE,
		SPRITE
	};

	public static int GetOrder(TYPE type, int index)
	{
		switch (type)
		{
			case TYPE.TILE:
				return (index >= 0 && index < _ORDER_OF_TILES.Length) ? _ORDER_OF_TILES[index] : _DEFAULT_ORDER_OF_TILE;
			case TYPE.SPRITE:
				return (index >= 0 && index < _ORDER_OF_SPRITES.Length) ? _ORDER_OF_SPRITES[index] : _DEFAULT_ORDER_OF_SPRITE;
		}

		return _DEFAULT_ORDER_OF_TILE;
	}
}
