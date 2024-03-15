using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using Helper;
using Personal.Character;
using Personal.Character.NPC;
using Personal.Character.Animation;
using Personal.Constant;
using Personal.FSM.Character;

namespace Personal.FSM
{
	public class ActorStateMachine : StateMachineBase, IRendererDissolve
	{
		public HeadModelLookAt HeadModelLookAt { get; protected set; }

		public NavMeshAgent NavMeshAgent { get; protected set; }
		public ActorController ActorController { get; protected set; }
		public AnimatorController AnimatorController { get; protected set; }
		public LookAtInfo LookAtInfo { get; protected set; }

		protected List<Material> materialList = new List<Material>();

		CoroutineRun fadeRendererCR = new CoroutineRun();

		protected override void EarlyInitialize()
		{
			HeadModelLookAt = GetComponentInChildren<HeadModelLookAt>(true);

			NavMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			AnimatorController = GetComponentInChildren<AnimatorController>(true);
			ActorController = GetComponentInChildren<ActorController>(true);

			Renderer renderer = GetComponentInChildren<Renderer>();
			materialList = renderer?.materials.ToList();
		}

		/// <summary>
		/// This begins the interactionAssign.
		/// </summary>
		/// <param name="interactionAssign">Most initiator is itself, but in certain cases, it might be the player/other NPCs.</param>
		/// <returns></returns>
		public virtual UniTask Begin(InteractionAssign interactionAssign, StateMachineBase initiatorFSM = null) { return UniTask.CompletedTask; }

		public void SetLookAtInfo(LookAtInfo lookAtInfo) { LookAtInfo = lookAtInfo; }

		/// <summary>
		/// Fade in/out the renderer.
		/// </summary>
		/// <param name="isFlag"></param>
		void IRendererDissolve.FadeInRenderer(bool isFlag, float duration)
		{
			// This is assuming that all materials are dissolvable.
			float startValue = materialList[0].GetFloat("_CutoffHeight");
			float endValue = isFlag ? ConstantFixed.FULLY_VISIBLE_REND_VALUE : ConstantFixed.FULLY_DISAPPEAR_REND_VALUE;

			float differences = Mathf.Abs(startValue - endValue);
			float ratio = differences / ConstantFixed.FULLY_VISIBLE_REND_VALUE;
			float remainingDuration = ratio * duration;

			fadeRendererCR?.StopCoroutine();
			Action<float> callbackMethod = (result) =>
			{
				foreach (var material in materialList)
				{
					material.SetFloat("_CutoffHeight", result);
				}
			};
			fadeRendererCR = CoroutineHelper.LerpWithinSeconds(startValue, endValue, remainingDuration, callbackMethod, OnRendererDissolveEnd);

			OnRendererDissolveBegin();
		}

		protected virtual void OnRendererDissolveBegin() { }
		protected virtual void OnRendererDissolveEnd() { }

		void OnDisable()
		{
			if (NavMeshAgent) NavMeshAgent.enabled = false;
		}
	}
}