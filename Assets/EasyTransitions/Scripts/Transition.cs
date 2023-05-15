using UnityEngine;
using UnityEngine.UI;

using Personal.GameState;
using Helper;
using Cysharp.Threading.Tasks;

namespace EasyTransition
{
	public class Transition : GameInitialize
	{
		[SerializeField] Transform transitionPanelIN;
		[SerializeField] Transform transitionPanelOUT;

		TransitionSettings transitionSettings;

		Material multiplyColorMaterial;
		Material additiveColorMaterial;

		AnimationClip animationIn;
		AnimationClip animationOut;

		protected override async UniTask Awake()
		{
			await base.Awake();

			// There will only be 1 clip within the state.
			animationIn = transitionPanelIN.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips[0];
			animationOut = transitionPanelOUT.GetComponentInChildren<Animator>().runtimeAnimatorController.animationClips[0];
		}

		public async UniTask Begin(TransitionSettings transitionSettings, TransitionManagerSettings fullSettings)
		{
			this.transitionSettings = transitionSettings;

			// Setting the materials
			multiplyColorMaterial = fullSettings.MultiplyColorMaterial;
			additiveColorMaterial = fullSettings.AddColorMaterial;

			// Checking if the materials were correctly set
			if (multiplyColorMaterial == null || additiveColorMaterial == null)
				Debug.LogWarning("There are no color tint materials set for the transition. Changing the color tint will not affect the transition anymore!");

			await TransitionIn();
			await TransitionOut();
		}

		async UniTask TransitionIn()
		{
			// Setting up the transition objects
			transitionPanelOUT.gameObject.SetActive(false);
			transitionPanelIN.gameObject.SetActive(true);

			await HandleTransition(transitionSettings.TransitionIn.transform, animationIn);
		}

		async UniTask TransitionOut()
		{
			// Setting up the transition
			transitionPanelIN.gameObject.SetActive(false);
			transitionPanelOUT.gameObject.SetActive(true);

			await HandleTransition(transitionSettings.TransitionOut.transform, animationOut);

			transitionPanelOUT.gameObject.SetActive(false);

			////Adjusting the destroy time if needed
			//float destroyTime = transitionSettings.DestroyTime;
			//if (transitionSettings.AutoAdjustTransitionTime)
			//	destroyTime = destroyTime / transitionSettings.TransitionSpeed;

			////Destroying the transition
			//Destroy(gameObject, destroyTime);
		}

		async UniTask HandleTransition(Transform transition, AnimationClip animationClip)
		{
			HandleTransitionColor(transition.transform);
			HandleFlipping(transition.transform);
			HandleAnimatorSpeed(transition.transform);

			await UniTask.Delay((int)animationClip.length.SecondsToMilliseconds());
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
		void HandleAnimatorSpeed(Transform trans)
		{
			if (trans.TryGetComponent(out Animator parentAnim) && transitionSettings.TransitionSpeed != 0)
			{
				parentAnim.speed = transitionSettings.TransitionSpeed;
				return;
			}

			for (int i = 0; i < trans.childCount; i++)
			{
				if (trans.GetChild(i).TryGetComponent(out Animator childAnim) && transitionSettings.TransitionSpeed != 0)
					childAnim.speed = transitionSettings.TransitionSpeed;
			}
		}
	}
}