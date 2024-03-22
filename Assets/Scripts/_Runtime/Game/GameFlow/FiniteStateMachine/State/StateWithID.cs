using System;
using UnityEngine;

namespace Personal.FSM
{
	public abstract class StateWithID : StateBase
	{
		[SerializeField] protected int moveToTargetID = -1;
		[SerializeField] protected int lookAtTargetID = -1;

		protected Transform moveToTargetTrans;
		protected Transform lookAtTargetTrans;

		public int MoveToTargetID { get => moveToTargetID; }
		public int LookAtTargetID { get => lookAtTargetID; }

		public void SetTarget(Transform moveToTargetTrans, Transform lookAtTargetTrans)
		{
			this.moveToTargetTrans = moveToTargetTrans;
			this.lookAtTargetTrans = lookAtTargetTrans;
		}
	}
}