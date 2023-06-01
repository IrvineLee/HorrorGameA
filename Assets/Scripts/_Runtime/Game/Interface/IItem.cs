using UnityEngine;

namespace Personal
{
	public interface IItem
	{
		/// <summary>
		/// Use the current item.
		/// </summary>
		void Use();

		/// <summary>
		/// Placed the item at position.
		/// </summary>
		/// <param name="position"></param>
		void PlaceAt(Vector3 position);
	}
}