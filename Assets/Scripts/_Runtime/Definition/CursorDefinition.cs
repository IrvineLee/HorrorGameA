using System;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Definition
{
	[CreateAssetMenu(fileName = "CursorDefinition", menuName = "ScriptableObjects/CursorDefinition", order = 0)]
	[Serializable]
	public class CursorDefinition : ScriptableObject
	{
		public enum CrosshairType
		{
			FPS = 0,
			Talk,
			Search,

			Nothing = 999,
		}

		[Serializable]
		public class Crosshair
		{
			[SerializeField] CrosshairType crosshairType = CrosshairType.FPS;
			[SerializeField] Sprite sprite = null;

			public CrosshairType CrosshairType { get => crosshairType; }
			public Sprite Sprite { get => sprite; }

			public Crosshair(CrosshairType crosshairType, Sprite sprite)
			{
				this.crosshairType = crosshairType;
				this.sprite = sprite;
			}
		}

		[SerializeField] Sprite mouseCursor = null;
		[SerializeField] List<Crosshair> crosshairList = new();

		public IReadOnlyDictionary<CrosshairType, Sprite> CrosshairDictionary { get => crosshairDictionary; }

		Dictionary<CrosshairType, Sprite> crosshairDictionary = new();

		public Sprite MouseCursor { get => mouseCursor; }

		public void Initialize()
		{
			foreach (var crosshair in crosshairList)
			{
				crosshairDictionary.Add(crosshair.CrosshairType, crosshair.Sprite);
			}
		}
	}
}