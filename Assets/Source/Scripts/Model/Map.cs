
using System;
using System.Collections.Generic;

public class Map
{
	public string map_name;
	public string file_name;
	public string byname;
	public Dictionary<int, string> events;

	public int[,] data = null;

	private nd.type.Size _size;

	public Map(nd.type.Size size)
	{
		_size = size;
		data = new int[size.w, size.h];
	}

	public int this[int x, int y]
	{
		get { return (x >= 0 && x < _size.w && y >= 0 && y < _size.h) ? this.data[x, y] : 0; }
		set { if (x >= 0 && x < _size.w && y >= 0 && y < _size.h) this.data[x, y] = value; }
	}

	public nd.type.Size Size
	{
		get { return _size; }
	}
}
