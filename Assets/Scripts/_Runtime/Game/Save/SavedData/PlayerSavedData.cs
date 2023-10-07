using UnityEngine;
using System;

namespace Personal.Save
{
	[Serializable]
	public class PlayerSavedData
	{
		[SerializeField] SlotSavedData slotSavedData = new SlotSavedData();
		[SerializeField] InventoryData inventoryData = new InventoryData();
		[SerializeField] QuestData questData = new QuestData();
		[SerializeField] GlossaryData glossaryData = new GlossaryData();

		public SlotSavedData SlotSavedData { get => slotSavedData; set => slotSavedData = value; }
		public InventoryData InventoryData { get => inventoryData; set => inventoryData = value; }
		public QuestData QuestData { get => questData; set => questData = value; }
		public GlossaryData GlossaryData { get => glossaryData; set => glossaryData = value; }
	}
}