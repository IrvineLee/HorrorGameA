using UnityEngine;

namespace Helper
{
	public static class IntExtensions
	{
		/// <summary>
		/// Keep the value within the size of count. 
		/// isLoopable handles whether accumulated values resets to 0 or max value.
		/// In case of true, index value will sequently ++/--.
		/// Ex: index 5, count 3, isLoopable true, returns 2.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="isLoopable"></param>
		/// <returns></returns>
		public static int WithinCount(this int index, int count, bool isLoopable = false)
		{
			if (!isLoopable)
			{
				if (index > count - 1) return 0;
				else if (index < 0) return count - 1;
			}

			if (index > count - 1) return index % count;
			else if (index < 0) return count + (index % count);

			return index;
		}

		/// <summary>
		/// Convert value from 0~100 to 0~1. 
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static int ConvertRatio0To1(this int original)
		{
			return original / 100;
		}

		/// <summary>
		/// Convert value from 0~1 to 0~100.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static int ConvertRatio0To100(this int original)
		{
			return original * 100;
		}
	}
}
