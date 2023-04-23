using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Helper;
using Personal.Entity;

namespace Personal.Data
{
	[ExcelAsset(AssetPath = "Data/MasterData/Data")]
	public class MasterGeneric<T1, T2> : ScriptableObject, ISerializationCallbackReceiver where T1 : GenericEntity
		// Typically T2 is an int, but in some cases where the dictionary key is much more complex it might become a class.
	{
		public List<T1> Entities;

		public IReadOnlyDictionary<T2, T1> Dictionary { get => dictionary; }

		protected Dictionary<T2, T1> dictionary = new Dictionary<T2, T1>();

		public virtual void OnBeforeSerialize() { }

		public virtual void OnAfterDeserialize()
		{
			dictionary = Entities.ToDictionary(i => (T2)(object)i.id);
		}

		/// <summary>
		/// Get item data from MasterData.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public virtual T1 Get(T2 id)
		{
			var result = Dictionary.GetOrDefault(id);
			return result;
		}
	}
}