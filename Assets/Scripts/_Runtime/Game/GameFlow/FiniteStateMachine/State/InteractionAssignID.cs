using System;
using UnityEngine;

namespace Personal.FSM
{
	public class InteractionAssignID : MonoBehaviour
	{
		[SerializeField] int id = 0;

		public int ID { get => id; }
	}
}