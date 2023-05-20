using UnityEngine;

namespace Helper
{
	public static class FloatExtensions
	{
		/// <summary>
		/// Fix -1.025547E-07 kinda value.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static float FloatPointFix(this float original)
		{
			return Mathf.Round(original * 10) / 10;
		}

		/// <summary>
		/// Convert float value to bool.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static bool ConvertToBool(this float original)
		{
			return original > 0;
		}

		/// <summary>
		/// Convert value from 0~100 to 0~1. 
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static float ConvertRatio0To1(this float original)
		{
			return original / 100;
		}

		/// <summary>
		/// Convert value from 0~1 to 0~100.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static float ConvertRatio0To100(this float original)
		{
			return original * 100;
		}

		/// <summary>
		/// Change from seconds to milliseconds.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static float SecondsToMilliseconds(this float original)
		{
			return original * 1000;
		}
	}
}
