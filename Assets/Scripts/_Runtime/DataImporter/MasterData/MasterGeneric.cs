using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Helper;
using Personal.Entity;

namespace Personal.Data
{
	[ExcelAsset(AssetPath = "Data/MasterData/Data")]
	public class MasterGeneric<T> : ScriptableObject, ISerializationCallbackReceiver where T : GenericEntity
	{
		public List<T> Entities;

		public IReadOnlyDictionary<int, T> Dictionary { get; private set; }

		public virtual void OnBeforeSerialize() { }

		public virtual void OnAfterDeserialize()
		{
			Dictionary = Entities.ToDictionary(i => i.id);
		}

		/// <summary>
		/// Get item data from MasterData.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public T Get(int id)
		{
			var result = Dictionary.GetOrDefault(id);
			return result;
		}
	}
}