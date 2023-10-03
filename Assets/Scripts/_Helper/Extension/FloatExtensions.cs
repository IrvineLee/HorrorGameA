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
		/// Round it to decimal places.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="decimalPlaces"></param>
		/// <returns></returns>
		public static float Round(this float original, int decimalPlaces = 1)
		{
			float powerOfValue = Mathf.Pow(10, decimalPlaces);
			float powerOfValueOpposite = powerOfValue / Mathf.Pow(powerOfValue, 2);

			return Mathf.Round(original * powerOfValue) * powerOfValueOpposite;
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
		/// Convert value from 0~1 to -1~1.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static float Convert01ToNeg1Pos1(this float original)
		{
			return 2 * original - 1;
		}

		/// <summary>
		/// Change from seconds to milliseconds.
		/// </summary>
		/// <param name="original"></param>
		/// <returns></returns>
		public static int SecondsToMilliseconds(this float original)
		{
			return (int)(original * 1000);
		}

		/// <summary>
		/// Check to see whether the value is between minValue and maxValue.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static bool IsWithin(this float original, float minValue, float maxValue,
			Inequality minInequality = Inequality.GreaterThanOrEqualTo,
			Inequality maxInequality = Inequality.LessThanOrEqualTo)
		{
			bool isWithinA = InequalityChecker(original, minInequality, minValue);
			bool isWithinB = InequalityChecker(original, maxInequality, maxValue);

			return (isWithinA && isWithinB);
		}

		/// <summary>
		/// Check to see whethher the passed-in value is true.
		/// </summary>
		/// <param name="valueA"></param>
		/// <param name="inequality"></param>
		/// <param name="valueB"></param>
		/// <returns></returns>
		static bool InequalityChecker(float valueA, Inequality inequality, float valueB)
		{
			if (inequality == Inequality.GreaterThanOrEqualTo) return valueA >= valueB;
			else if (inequality == Inequality.GreaterThan) return valueA > valueB;
			else if (inequality == Inequality.LessThanOrEqualTo) return valueA <= valueB;
			else if (inequality == Inequality.LessThan) return valueA < valueB;
			else if (inequality == Inequality.EqualTo) return valueA == valueB;

			return valueA != valueB;
		}
	}
}
