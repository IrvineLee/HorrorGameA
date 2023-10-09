using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TMPro;
using Cysharp.Threading.Tasks;
using Helper;
using Personal.Quest;

namespace Personal.UI.Quest
{
	public class QuestContainerUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI questTitleTMP = null;
		[SerializeField] Transform descriptionTMPParent = null;

		public bool IsMainQuest { get; private set; }

		Animator animator;
		string parameter = "IsEnable";
		int animFade;

		PaddingAnimation paddingAnimation;

		List<TextMeshProUGUI> descriptionTMPList = new();

		public void ShowQuest(QuestInfo questInfo)
		{
			Cache();
			if (string.IsNullOrEmpty(questTitleTMP.text)) animator.SetBool(animFade, true);

			IsMainQuest = questInfo.QuestEntity.isMainQuest;
			questTitleTMP.text = questInfo.QuestEntity.name;

			for (int i = 0; i < descriptionTMPList.Count; i++)
			{
				descriptionTMPList[i].text = questInfo.TaskInfoList[i].Description;
			}
		}

		public async UniTask FadeAwayResetText()
		{
			animator.SetBool(animFade, false);
			paddingAnimation.MoveOut();

			await UniTask.NextFrame();

			bool isDone = false;
			CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => isDone = true);

			await UniTask.WaitUntil(() => isDone);

			questTitleTMP.text = "";
			foreach (var descriptionTMP in descriptionTMPList)
			{
				descriptionTMP.text = "";
			}
		}

		void Cache()
		{
			if (animator) return;

			animator = GetComponentInChildren<Animator>();
			animFade = Animator.StringToHash(parameter);

			descriptionTMPList = descriptionTMPParent.GetComponentsInChildren<TextMeshProUGUI>().ToList();
			paddingAnimation = GetComponentInChildren<PaddingAnimation>();
		}
	}
}