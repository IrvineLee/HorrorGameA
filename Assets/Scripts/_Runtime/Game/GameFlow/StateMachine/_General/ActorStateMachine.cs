using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using PixelCrushers.DialogueSystem;
using Personal.GameState;
using Personal.Character.Animation;
using Personal.Character;
using Personal.Constant;
using Helper;

namespace Personal.FSM
{
	public class ActorStateMachine : StateMachineBase, IRendererDissolve
	{
		public TargetInfo TargetInfo { get; protected set; }
		public NavMeshAgent NavMeshAgent { get; protected set; }
		public DialogueSystemTrigger DialogueSystemTrigger { get; protected set; }
		public AnimatorController AnimatorController { get; protected set; }
		public HeadLookAt HeadLookAt { get; protected set; }

		protected List<StateBase> orderedStateList = new List<StateBase>();
		protected List<Material> materialList = new List<Material>();

		CoroutineRun fadeRendererCR = new CoroutineRun();

		protected override void Initialize()
		{
			NavMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			DialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>(true);
			AnimatorController = GetComponentInChildren<AnimatorController>(true);
			HeadLookAt = GetComponentInChildren<HeadLookAt>(true);

			Renderer renderer = GetComponentInChildren<Renderer>();
			materialList = renderer?.materials.ToList();
		}

		protected override void OnUpdate()
		{
			if (state == null) return;

			state.OnUpdateRun();
		}

		protected async UniTask PlayOrderedState()
		{
			foreach (var state in orderedStateList)
			{
				state.SetFSM(this);
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
			float endValue = isFlag ? ConstantFixed.fullyVisibleRendValue : ConstantFixed.fullyDisappearRendValue;

			float differences = Mathf.Abs(startValue - endValue);
			float ratio = differences / ConstantFixed.fullyVisibleRendValue;
			float remainingDuration = ratio * duration;

			fadeRendererCR?.StopCoroutine();
			Action<float> callbackMethod = (result) =>
			{
				foreach (var material in materialList)
				{
					material.SetFloat("_CutoffHeight", result);
				}
			};
			fadeRendererCR = CoroutineHelper.LerpWithinSeconds(startValue, endValue, remainingDuration, callbackMethod);

			OnRendererDissolving(isFlag);
		}

		protected virtual void OnRendererDissolving(bool isFlag) { }

		void OnDisable()
		{
			if (NavMeshAgent) NavMeshAgent.enabled = false;
		}
	}
}