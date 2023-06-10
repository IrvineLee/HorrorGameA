using System;
using System.Collections;
using System.Collections.Generic;
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

		public Renderer Renderer { get => renderer; }

		protected List<StateBase> orderedStateList = new List<StateBase>();
		new Renderer renderer;

		CoroutineRun fadeRendererCR = new CoroutineRun();

		protected override async UniTask Awake()
		{
			await base.Awake();

			NavMeshAgent = GetComponentInChildren<NavMeshAgent>(true);
			DialogueSystemTrigger = GetComponentInChildren<DialogueSystemTrigger>(true);
			AnimatorController = GetComponentInChildren<AnimatorController>(true);
			HeadLookAt = GetComponentInChildren<HeadLookAt>(true);

			renderer = GetComponentInChildren<Renderer>();
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
			float startValue = renderer.material.GetFloat("_CutoffHeight");
			float endValue = isFlag ? ConstantFixed.fullyVisibleRendValue : ConstantFixed.fullyDisappearRendValue;

			float differences = Mathf.Abs(startValue - endValue);
			float ratio = differences / ConstantFixed.fullyVisibleRendValue;
			float remainingDuration = ratio * duration;

			fadeRendererCR?.StopCoroutine();
			Action<float> callbackMethod = (result) => renderer.material.SetFloat("_CutoffHeight", result);
			fadeRendererCR = CoroutineHelper.LerpWithinSeconds(startValue, endValue, remainingDuration, callbackMethod);
		}

		void OnDisable()
		{
			if (NavMeshAgent) NavMeshAgent.enabled = false;
		}
	}
}