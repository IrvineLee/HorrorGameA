using System;
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

		AnimationClip animationIn;
		AnimationClip animationOut;

		void Awake()
		{
			// There will only be 1 clip within the state.
			animationIn = transitionPanelIN.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips[0];
			animationOut = transitionPanelOUT.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips[0];
		}

		public async UniTask Begin(TransitionSettings transitionSettings, TransitionPlayType transitionPlayType,
								   TransitionManagerSettings fullSettings, Action inBetweenAction)
		{
			SetupTransitionAndMaterial(transitionSettings, fullSettings);
			float speed = transitionSettings.TransitionSpeed;

			if (transitionPlayType.HasFlag(TransitionPlayType.In)) await TransitionIn(speed);
			inBetweenAction?.Invoke();
			if (transitionPlayType.HasFlag(TransitionPlayType.Out)) await TransitionOut(speed);
		}

		public async UniTask Begin(TransitionSettings transitionSettings, TransitionPlayType transitionPlayType,
								   TransitionManagerSettings fullSettings, Func<UniTask<bool>> inBetweenFunc)
		{
			SetupTransitionAndMaterial(transitionSettings, fullSettings);
			float speed = transitionSettings.TransitionSpeed;

			if (transitionPlayType.HasFlag(TransitionPlayType.In)) await TransitionIn(speed);
			await inBetweenFunc();
			if (transitionPlayType.HasFlag(TransitionPlayType.Out)) await TransitionOut(speed);
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

		async UniTask TransitionIn(float speed)
		{
			// Setting up the transition objects
			transitionPanelOUT.gameObject.SetActive(false);
			transitionPanelIN.gameObject.SetActive(true);

			await HandleTransition(transitionSettings.TransitionIn.transform, animationIn, speed);
		}

		async UniTask TransitionOut(float speed)
		{
			// Setting up the transition
			transitionPanelIN.gameObject.SetActive(false);
			transitionPanelOUT.gameObject.SetActive(true);

			await HandleTransition(transitionSettings.TransitionOut.transform, animationOut, speed);

			transitionPanelOUT.gameObject.SetActive(false);
		}

		async UniTask HandleTransition(Transform transition, AnimationClip animationClip, float speed)
		{
			HandleTransitionColor(transition.transform);
			HandleFlipping(transition.transform);
			HandleAnimatorSpeed(transition.transform, speed);

			await UniTask.Delay(animationClip.length.SecondsToMilliseconds(), true);
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

		// Changing the animator speed
		void HandleAnimatorSpeed(Transform trans, float speed)
		{
			if (trans.TryGetComponent(out Animator parentAnim) && transitionSettings.TransitionSpeed != 0)
			{
				parentAnim.speed = speed;
				return;
			}

			for (int i = 0; i < trans.childCount; i++)
			{
				if (trans.GetChild(i).TryGetComponent(out Animator childAnim) && transitionSettings.TransitionSpeed != 0)
					childAnim.speed = speed;
			}
		}
	}
}
