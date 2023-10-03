using System;
using UnityEngine;

namespace Personal.Preloader
{
	public class Preload : MonoBehaviour
	{
		public static bool IsLoaded { get; protected set; }
		public static bool IsPreloadSceneLoaded { get; protected set; }
	}
}