using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;
using Helper;

namespace Personal.Transition
{
	public class Transition : MonoBehaviour
	{
		[SerializeField] Transform transitionPanelIN;
		[SerializeField] Transform transitionPanelOUT;

		TransitionSettings transitionSettings;

		Material multiplyColorMaterial;
		Material additiveColorMaterial;

		Animator animatorIn;
		Animator animatorOut;

		AnimationClip animationIn;
		AnimationClip animationOut;

		void Awake()
		{
			animatorIn = transitionPanelIN.GetComponentInChildren<Animator>();
			animatorOut = transitionPanelOUT.GetComponentInChildren<Animator>();

			// There will only be 1 clip within the state.
			animationIn = animatorIn.runtimeAnimatorController.animationClips[0];
			animationOut = animatorOut.runtimeAnimatorController.animationClips[0];
		}

		public async UniTask Begin(TransitionSettings transitionSettings, TransitionPlayType transitionPlayType,
								TransitionManagerSettings fullSettings, Action inBetweenAction, bool isIgnoreTimescale, CancellationToken token)
		{
			await Initialize(transitionSettings, transitionPlayType, fullSettings, inBetweenAction, isIgnoreTimescale, token);
		}

		public async UniTask Begin(TransitionSettings transitionSettings, TransitionPlayType transitionPlayType,
								   TransitionManagerSettings fullSettings, Func<UniTask<bool>> inBetweenFunc, bool isIgnoreTimescale, CancellationToken token)
		{
			await Initialize(transitionSettings, transitionPlayType, fullSettings, inBetweenFunc, isIgnoreTimescale, token);
		}

		async UniTask Initialize<T>(TransitionSettings transitionSettings, TransitionPlayType transitionPlayType,
								   TransitionManagerSettings fullSettings, T actionOrFunc, bool isIgnoreTimescale, CancellationToken token)
		{
			SetupTransitionAndMaterial(transitionSettings, fullSettings);
			float speed = transitionSettings.TransitionSpeed;

			if (transitionPlayType.HasFlag(TransitionPlayType.In)) await TransitionIn(speed, isIgnoreTimescale, token);

			// Make sure the transition timing is at the end of it.
			animatorIn.gameObject.SetActive(true);
			await UniTask.NextFrame();

			if (animatorIn.gameObject.activeInHierarchy) animatorIn.Play(0, -1, 1f);

			if (actionOrFunc != null && actionOrFunc.GetType() == typeof(Action))
			{
				((Action)(object)actionOrFunc).Invoke();
			}
			else if (actionOrFunc != null && actionOrFunc.GetType() == typeof(Func<UniTask<bool>>))
			{
				await ((Func<UniTask<bool>>)(object)actionOrFunc).Invoke();
			}

			// Give it a grace period after changing scene.
			await UniTask.Delay(1000, isIgnoreTimescale, cancellationToken: token);
			if (transitionPlayType.HasFlag(TransitionPlayType.Out)) await TransitionOut(speed, isIgnoreTimescale, token);
		}


		void SetupTransitionAndMaterial(TransitionSettings transitionSettings, TransitionManagerSettings fullSettings)
		{
			this.transitionSettings = transitionSettings;

			// Setting the materials
			multiplyColorMaterial = fullSettings.MultiplyColorMaterial;
			additiveColorMaterial = fullSettings.AddColorMaterial;

			// Checking if the materials were correctly set
			if (multiplyColorMaterial == null || additiveColorMaterial == null)
				Debug.LogWarning("There are no color tint materials set for the transition. Changing the color tint will not affect the transition anymore!");

		}

		async UniTask TransitionIn(float speed, bool isIgnoreTimescale, CancellationToken token)
		{
			// Setting up the transition objects
			transitionPanelOUT.gameObject.SetActive(false);
			transitionPanelIN.gameObject.SetActive(true);

			await HandleTransition(transitionSettings.TransitionIn.transform, animatorIn, animationIn, speed, isIgnoreTimescale, token);
		}

		async UniTask TransitionOut(float speed, bool isIgnoreTimescale, CancellationToken token)
		{
			// Setting up the transition
			transitionPanelIN.gameObject.SetActive(false);
			transitionPanelOUT.gameObject.SetActive(true);

			await HandleTransition(transitionSettings.TransitionOut.transform, animatorOut, animationOut, speed, isIgnoreTimescale, token);

			transitionPanelOUT.gameObject.SetActive(false);
		}

		async UniTask HandleTransition(Transform transition, Animator animator, AnimationClip animationClip, float speed, bool isIgnoreTimescale, CancellationToken token)
		{
			HandleTransitionColor(transition);
			HandleFlipping(transition);
			HandleAnimator(animator, speed, isIgnoreTimescale);

			await UniTask.Delay(animationClip.length.SecondsToMilliseconds(), isIgnoreTimescale, cancellationToken: token);
		}

		// Changing the color of the transition
		void HandleTransitionColor(Transform trans)
		{
			if (transitionSettings.IsCutoutTransition) return;

			SetColor(trans);
			foreach (Transform child in trans)
			{
				SetColor(child);
			}
		}

		void SetColor(Transform trans)
		{
			if (!trans.TryGetComponent(out Image image)) return;

			if (transitionSettings.ColorTintMode == ColorTintMode.Multiply)
			{
				image.material = multiplyColorMaterial;
			}
			else if (transitionSettings.ColorTintMode == ColorTintMode.Add)
			{
				image.material = additiveColorMaterial;
			}
			image.material.SetColor("_Color", transitionSettings.ColorTint);
		}

		// Flipping the scale if needed
		void HandleFlipping(Transform trans)
		{
			if (transitionSettings.FlipX)
				trans.localScale = trans.localScale.With(x: -trans.localScale.x);
			if (transitionSettings.FlipY)
				trans.localScale = trans.localScale.With(y: -trans.localScale.y);
		}

		// Changing the animator's update mode and speed
		void HandleAnimator(Animator animator, float speed, bool isIgnoreTimescale)
		{
			if (!animator) return;

			animator.updateMode = isIgnoreTimescale ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
			animator.speed = speed;
		}
	}
}
