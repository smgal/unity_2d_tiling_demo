
using System;

public class Map
{
	private nd.Size _size;
	public int[,] data = null;

	public Map(nd.Size size)
	{
		_size = size;
		data = new int[size.w, size.h];
	}

	public int this[int x, int y]
	{
		get { return (x >= 0 && x < _size.w && y >= 0 && y < _size.h) ? this.data[x, y] : 0; }
		set { if (x >= 0 && x < _size.w && y >= 0 && y < _size.h) this.data[x, y] = value; }
	}

	public nd.Size Size
	{
		get { return _size; }
	}
}
