using UnityEngine;
using System;

namespace Personal.Save
{
	[Serializable]
	public class PlayerSavedData
	{
		[SerializeField] InventoryData inventoryData = new InventoryData();
		[SerializeField] QuestData questData = new QuestData();
		[SerializeField] GlossaryData glossaryData = new GlossaryData();

		public InventoryData InventoryData { get => inventoryData; set => inventoryData = value; }
		public QuestData QuestData { get => questData; set => questData = value; }
		public GlossaryData GlossaryData { get => glossaryData; set => glossaryData = value; }
	}
}