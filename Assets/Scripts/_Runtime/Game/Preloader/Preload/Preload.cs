using System;
using UnityEngine;

namespace Personal.Preloader
{
	public class Preload : MonoBehaviour
	{
		/// <summary>
		/// 1st. Checks whether the preload scene has been loaded. 
		/// </summary>
		public static bool IsPreloadSceneLoaded { get; protected set; }

		/// <summary>
		/// 2nd. Checks whether GameManager has been loaded.
		/// </summary>
		public static bool IsLoaded { get; protected set; }
	}
}