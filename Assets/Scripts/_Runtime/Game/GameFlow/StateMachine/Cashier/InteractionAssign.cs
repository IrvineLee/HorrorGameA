using System.Collections.Generic;
using UnityEngine;

namespace Personal.FSM
{
	public class InteractionAssign : MonoBehaviour
	{
		public List<StateBase> OrderedStateList { get; private set; } = new();

		void Awake()
		{
			foreach (Transform child in transform)
			{
				OrderedStateList.Add(child.GetComponent<StateBase>());
			}
		}
	}
}