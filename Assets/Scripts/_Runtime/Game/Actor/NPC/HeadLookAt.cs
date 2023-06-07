using UnityEngine;

using Cysharp.Threading.Tasks;
using Personal.Manager;
using Personal.GameState;

namespace Personal.Character
{
	public class HeadLookAt : GameInitialize
	{
		[SerializeField] bool isLookAtPlayer = false;
		[SerializeField] float lookAtSpeed = 1f;
		[SerializeField] float maxLookAtWeight = 1f;

		Animator animator;
		Transform playerTrans;

		float lookWeight;

		protected override async UniTask Awake()
		{
			await base.Awake();

			animator = GetComponentInChildren<Animator>();
			playerTrans = StageManager.Instance.PlayerController.transform;
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			Vector3 direction = Vector3.Normalize(playerTrans.position - transform.position);
			float dotProduct = Vector3.Dot(transform.forward, direction);

			float lastFrameValue = Time.deltaTime * lookAtSpeed;
			if (dotProduct > 0 && isLookAtPlayer)
			{
				lookWeight += lastFrameValue;
				if (lookWeight > maxLookAtWeight) lookWeight = maxLookAtWeight;

				return;
			}

			lookWeight -= lastFrameValue;
			if (lookWeight <= 0)
			{
				lookWeight = 0;

				if (!isLookAtPlayer) enabled = false;
			}
		}

		public void SetLookAtPlayer(bool isFlag)
		{
			enabled = true;
			isLookAtPlayer = isFlag;
		}

		void OnAnimatorIK()
		{
			if (!animator) return;

			animator.SetLookAtWeight(lookWeight);
			animator.SetLookAtPosition(playerTrans.position);
		}
	}
}