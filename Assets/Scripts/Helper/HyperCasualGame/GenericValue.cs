using System;
using UnityEngine;

namespace HyperCasualGame.Helper
{
	[Serializable]
	public class GenericValue<T>
	{
		[SerializeField] T value1;
		[SerializeField] T value2;

		public T Value1 { get => value1; }
		public T Value2 { get => value2; }

		public GenericValue(T min, T max)
		{
			this.value1 = min;
			this.value2 = max;
		}
	}
}