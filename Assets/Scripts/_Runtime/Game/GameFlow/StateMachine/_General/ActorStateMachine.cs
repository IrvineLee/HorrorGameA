using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Character;
using Personal.Character.NPC;
using Personal.Character.Animation;
using Personal.Constant;
using Helper;

namespace Personal.FSM
{
	public class ActorStateMachine : StateMachineBase, IRendererDissolve
	{
		// These are typically used for NPCs, but can be used by players too...
		public TargetInfo TargetInfo { get; protected set; }
		public HeadModelLookAt HeadModelLookAt { get; protected set; }

		public NavMeshAgent NavMeshAgent { get; protected set; }
		public DialogueSystemTrigger DialogueSystemTrigger { get; protected set; }
		public ActorController ActorController { get; protected set; }
		public AnimatorController AnimatorController { get; protected set; }
		public Transform LookAtTarget { get; protected set; }

		protected List<StateBase> orderedStateList = new List<StateBase>();
		protected List<Material> materialList = new List<Material>();

		CoroutineRun fadeRendererCR = new CoroutineRun();

		/// <summary>
		/// Call this to make sure this awake gets called first and initialized.
		/// </summary>
		/// <param name="targetInfo"></param>
		/// <param name="interactionAssign"></param>
		/// <returns></returns>
		public virtual async UniTask Begin(StateMachineBase initiatorFSM, TargetInfo targetInfo, InteractionAssign interactionAssign)
		{
			await UniTask.NextFrame(PlayerLoopTiming.LastPostLateUpdate);
		}

		protected override void Initialize()
		{
			NavMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			DialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>(true);
			AnimatorController = GetComponentInChildren<AnimatorController>(true);
			HeadModelLookAt = GetComponentInChildren<HeadModelLookAt>(true);

			Renderer renderer = GetComponentInChildren<Renderer>();
			materialList = renderer?.materials.ToList();
		}

		void Update()
		{
			if (state == null) return;

			state.OnUpdateRun();
		}

		public void SetLookAtTarget(Transform target)
		{
			LookAtTarget = target;
		}

		protected async UniTask PlayOrderedState()
		{
			foreach (var state in orderedStateList)
			{
				state.SetMyFSM(this);
				await SetState(state);
			}
		}

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

		protected override void OnPostDisable()
		{
			if (NavMeshAgent) NavMeshAgent.enabled = false;
		}
	}
}