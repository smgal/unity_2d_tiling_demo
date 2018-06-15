
using System;

public static class nd
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

	public static int Abs(int a)
	{
		return (a >= 0) ? a : -a;
	}

	public static int Sign(int a)
	{
		return (a > 0) ? 1 : ((a < 0) ? -1 : 0);
	}

	public static int Clamp(int val, int min, int max)
	{
		if (val < min)
			return min;
		else if (val > max)
			return max;
		else
			return val;
	}

	public static bool InRange(int val, int min, int max)
	{
		return (val >= min) && (val <= max);
	}
}



