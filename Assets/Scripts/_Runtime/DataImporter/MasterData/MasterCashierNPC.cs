using System.Collections.Generic;
using System.Linq;

using Helper;
using Personal.Data;
using UnityEngine;
using static MasterCashierNPC;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterCashierNPC : MasterGeneric<CashierNPCEntity, DayInteraction>
{
	public class DayInteraction
	{
		public int DayIndex { get; private set; }
		public int InteractionIndex { get; private set; }

		public DayInteraction(int dayIndex, int interactionIndex)
		{
			DayIndex = dayIndex;
			InteractionIndex = interactionIndex;
		}

		public class EqualityComparer : IEqualityComparer<DayInteraction>
		{
			public bool Equals(DayInteraction x, DayInteraction y)
			{
				return x.DayIndex == y.DayIndex && x.InteractionIndex == y.InteractionIndex;
			}

			public int GetHashCode(DayInteraction x)
			{
				return x.DayIndex ^ x.InteractionIndex;
			}
		}
	}

	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<DayInteraction, CashierNPCEntity>(new DayInteraction.EqualityComparer());

		// Somehow using Linq to change it to dictionary does not work.
		foreach (var entity in Entities)
		{
			dictionary.Add(new DayInteraction(entity.dayID, entity.interactionID), entity);
		}
	}

	/// <summary>
	/// Get item data from MasterData.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public override CashierNPCEntity Get(DayInteraction dayInteraction)
	{
		var result = Dictionary.GetOrDefault(dayInteraction);
		return result;
	}
}
