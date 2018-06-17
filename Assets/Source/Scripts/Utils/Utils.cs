
using System;

namespace nd
{
	public static class util
	{
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
}
