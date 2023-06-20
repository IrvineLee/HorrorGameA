using System;
using System.Collections.Generic;

using Helper;
using Personal.Data;
using static MasterCashierNPC;

[ExcelAsset(AssetPath = "Data/MasterData/Data")]
public class MasterCashierNPC : MasterGeneric<CashierNPCEntity, DayInteraction>
{
	[Serializable]
	public class DayInteraction
	{
		public int DayID { get; private set; }
		public int InteractionID { get; private set; }

		public DayInteraction(int dayID, int interactionID)
		{
			DayID = dayID;
			InteractionID = interactionID;
		}

		public class EqualityComparer : IEqualityComparer<DayInteraction>
		{
			public bool Equals(DayInteraction x, DayInteraction y)
			{
				return x.DayID == y.DayID && x.InteractionID == y.InteractionID;
			}

			public int GetHashCode(DayInteraction x)
			{
				return x.DayID ^ x.InteractionID;
			}
		}
	}

	public override void OnAfterDeserialize()
	{
		dictionary = new Dictionary<DayInteraction, CashierNPCEntity>(new DayInteraction.EqualityComparer());

		// Somehow using Linq to change it to dictionary makes the comparing results not work.
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
