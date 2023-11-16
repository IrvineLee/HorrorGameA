using UnityEngine;

namespace Helper
{
	public static class IntExtensions
	{
		/// <summary>
		/// Keep the value within the size of count. 
		/// isContinuallyCalculate handles whether accumulated values continue to calculate over or stop at 0/max value.
		/// In case of true, index value will sequently ++/--.
		/// Ex: index 5, count 3, isContinuallyCalculate true, returns 2.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <param name="isContinuallyCalculate"></param>
		/// <returns></returns>
		public static int WithinCountLoopOver(this int index, int count, bool isContinuallyCalculate = false)
		{
			if (!isContinuallyCalculate)
			{
				if (index > count - 1) return 0;
				else if (index < 0) return count - 1;
			}

			if (index > count - 1) return index % count;
			else if (index < 0) return count + (index % count);

			return index;
		}

		/// <summary>
		/// Make sure the index is within 0~count. If below 0 returns 0. If over count returns count.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public static int WithinCount(this int index, int count)
		{
			if (index < 0) index = 0;
			else if (index >= count) index = count - 1;

			return index;
		}

		/// <summary>
		/// Swap the value.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="swap"></param>
		public static void Swap(this ref int original, ref int swap)
		{
			int temp = original;
			original = swap;
			swap = temp;
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

		/// <summary>
		/// Check to see whether the value is between minValue and maxValue.
		/// </summary>
		/// <param name="original"></param>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static bool IsWithin(this int original, int minValue, int maxValue,
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
		static bool InequalityChecker(int valueA, Inequality inequality, int valueB)
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
