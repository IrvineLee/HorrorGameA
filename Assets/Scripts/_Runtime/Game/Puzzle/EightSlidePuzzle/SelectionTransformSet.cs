using System;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

namespace Personal.Puzzle.EightSlide
{
	public class SelectionTransformSet : MonoBehaviour
	{
		[Serializable]
		public class SelectionTarget
		{
			[SerializeField] Transform selection = null;
			[SerializeField] [ReadOnly] Transform target = null;

			public Transform Selection { get => selection; }
			public Transform Target { get => target; }

			public void SetTarget(Transform target) { this.target = target; }
		}

		[SerializeField] List<SelectionTarget> selectionTargetList = new();

		public List<SelectionTarget> SelectionTargetList { get => selectionTargetList; }

		SelectionTarget emptySelectionTarget;

		void Start()
		{
			foreach (var selectionTarget in selectionTargetList)
			{
				if (selectionTarget.Target != null) continue;

				emptySelectionTarget = selectionTarget;
				break;
			}
		}

		/// <summary>
		/// Automatically set the target for the index.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="target"></param>
		public void SetInitialTarget(int index, Transform target)
		{
			selectionTargetList[index].SetTarget(target);
		}

		/// <summary>
		/// Swap the target to empty target.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="fromIndex"></param>
		public void SwapTargetToEmpty(Transform target, int fromIndex)
		{
			emptySelectionTarget.SetTarget(target);
			emptySelectionTarget = selectionTargetList[fromIndex];
			emptySelectionTarget.SetTarget(null);
		}
	}
}
