
using UnityEngine;

public class GameObj: MonoBehaviour
{
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
	}

}