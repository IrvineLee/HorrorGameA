using System;
using UnityEngine;

using Helper;
using Personal.Save;
using Personal.GameState;
using Personal.Item;

namespace Personal.Manager
{
	/// <summary>
	/// Handle glossary.
	/// </summary>
	public class GlossaryManager : GameInitializeSingleton<GlossaryManager>
	{
		GlossaryData glossaryData;

		protected override void Initialize()
		{
			glossaryData = GameStateBehaviour.Instance.SaveObject.PlayerSavedData.GlossaryData;
		}

		/// <summary>
		/// This will +1 to the inserted type.
		/// </summary>
		/// <param name="usedType"></param>
		/// <returns></returns>
		public int AddUsedType<T>(T usedType) where T : Enum
		{
			if (typeof(T) == typeof(ItemType))
			{
				ItemType itemType = (ItemType)(object)usedType;
				glossaryData.UsedItemDictionary.AddTo(itemType);

				return glossaryData.UsedItemDictionary[itemType];
			}
			return 0;
		}

		/// <summary>
		/// Get the count for inserted type.
		/// </summary>
		/// <param name="usedType"></param>
		/// <returns></returns>
		public int GetUsedType<T>(T usedType) where T : Enum
		{
			if (typeof(T) == typeof(ItemType))
			{
				glossaryData.UsedItemDictionary.GetOrDefault((ItemType)(object)usedType);
			}
			return 0;
		}
	}
}

