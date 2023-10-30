using System;
using UnityEngine;

namespace Helper
{
	public class ClassHelper : MonoBehaviour
	{
		/// <summary>
		/// This class defines the amount of class count.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public class TCount<T>
		{
			[SerializeField] T tType;
			[SerializeField] int count;

			public T TType { get => tType; }
			public int Count { get => count; }

			public TCount(T tType, int count)
			{
				this.tType = tType;
				this.count = count;
			}

			public void Add(int value = 1) { count += value; }
		}
	}
}