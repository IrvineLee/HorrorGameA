using System.Collections.Generic;
using UnityEngine;

namespace Personal.FSM
{
	public class InteractionAssign : MonoBehaviour
	{
		public List<StateBase> OrderedStateList { get; private set; } = new();

		public bool IsComplete { get; private set; }

		void Awake()
		{
			foreach (Transform child in transform)
			{
				OrderedStateList.Add(child.GetComponent<StateBase>());
			}
		}

		public void SetToComplete() { IsComplete = true; }
	}
}