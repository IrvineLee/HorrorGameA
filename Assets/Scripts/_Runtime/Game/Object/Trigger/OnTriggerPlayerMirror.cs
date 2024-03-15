using UnityEngine;

using Personal.Manager;
using Personal.System.Handler;
using Personal.Character.Player;

namespace Personal
{
	public class OnTriggerPlayerMirror : MonoBehaviour
	{
		PlayerModel modelController;

		void Start()
		{
			modelController = StageManager.Instance.PlayerController.ModelController;
		}

		void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer != (int)LayerType._Player) return;

			modelController.MirrorAnimatorController.gameObject.SetActive(true);
		}

		void OnTriggerExit(Collider other)
		{
			if (other.gameObject.layer != (int)LayerType._Player) return;

			modelController.MirrorAnimatorController.gameObject.SetActive(false);
		}
	}
}
