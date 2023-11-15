using System;
using UnityEngine;

namespace Personal.Save
{
	[Serializable]
	public class PlayerSavedData
	{
		[SerializeField] string sceneName = "";
		[SerializeField] Vector3 position = Vector3.zero;

		[SerializeField] InventoryData inventoryData = new InventoryData();
		[SerializeField] QuestData questData = new QuestData();
		[SerializeField] GlossaryData glossaryData = new GlossaryData();

		public string SceneName { get => sceneName; set => sceneName = value; }
		public Vector3 Position { get => position; set => position = value; }

		public InventoryData InventoryData { get => inventoryData; set => inventoryData = value; }
		public QuestData QuestData { get => questData; set => questData = value; }
		public GlossaryData GlossaryData { get => glossaryData; set => glossaryData = value; }
	}
}