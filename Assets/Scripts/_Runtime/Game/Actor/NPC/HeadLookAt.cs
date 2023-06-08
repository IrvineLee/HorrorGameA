using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.GameState;
using Sirenix.OdinInspector;

namespace Personal.Character
{
	public class HeadLookAt : GameInitialize
	{
		[SerializeField] [ReadOnly] Transform defaultTargetTrans = null;
		[SerializeField] bool isLookAtTarget = false;
		[SerializeField] float lookAtSpeed = 1f;
		[SerializeField] float maxLookAtWeight = 1f;

		Animator animator;
		Transform targetTrans;

		float lookWeight;

		protected override async UniTask Awake()
		{
			await base.Awake();

			animator = GetComponentInChildren<Animator>();
			defaultTargetTrans = StageManager.Instance.PlayerController.transform;

			targetTrans = defaultTargetTrans;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			Vector3 direction = Vector3.Normalize(targetTrans.position - transform.position);
			float dotProduct = Vector3.Dot(transform.forward, direction);

			float lastFrameValue = Time.deltaTime * lookAtSpeed;
			if (dotProduct > 0 && isLookAtTarget)
			{
				lookWeight += lastFrameValue;
				if (lookWeight > maxLookAtWeight) lookWeight = maxLookAtWeight;

				return;
			}

			lookWeight -= lastFrameValue;
			if (lookWeight <= 0)
			{
				lookWeight = 0;

				if (isLookAtTarget) return;

				// Auto-disable
				enabled = false;
				targetTrans = defaultTargetTrans;
			}
		}

		/// <summary>
		/// The default target is the player's transform. 
		/// It resets back to default transform after getting disabled.
		/// </summary>
		/// <param name="target"></param>
		public void SetTarget(Transform target)
		{
			targetTrans = target;
		}

		/// <summary>
		/// Animate the head to move towards target/default position.
		/// It will automatically disabled by itself after setting it to false and finishes the animation.
		/// </summary>
		/// <param name="isFlag"></param>
		public void SetLookAtTarget(bool isFlag)
		{
			enabled = true;
			isLookAtTarget = isFlag;
		}

		void OnAnimatorIK()
		{
			if (!animator) return;

			animator.SetLookAtWeight(lookWeight);
			animator.SetLookAtPosition(targetTrans.position);
		}
	}
}