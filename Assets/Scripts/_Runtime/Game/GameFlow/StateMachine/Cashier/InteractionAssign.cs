using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Personal.FSM
{
	[ExecuteInEditMode]
	public class InteractionAssign : MonoBehaviour
	{
		public List<StateBase> OrderedStateList { get => GetComponentsInChildren<StateBase>().ToList(); }
	}
}