
using System;

namespace nd
{
	public static class type
	{
		public struct Pos
		{
			public int x, y;
			public Pos(int _x, int _y) { x = _x; y = _y; }
		};

		public struct Size
		{
			public int w, h;
		}

		public struct Dir
		{
			public int dx, dy;
		}
	}
}



