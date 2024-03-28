using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using TMPro;
using Cysharp.Threading.Tasks;
using Helper;
using Personal.Quest;
using Personal.Manager;
using Personal.Localization;
using Personal.GameState;
using Personal.UI.Option;

namespace Personal.UI.Quest
{
	public class QuestContainerUI : GameInitialize
	{
		[SerializeField] TextMeshProUGUI questTitleTMP = null;
		[SerializeField] Transform descriptionTMPParent = null;

		public bool IsMainQuest { get; private set; }

		Animator animator;
		string parameter = "IsEnable";
		int animFade;

		PaddingAnimation paddingAnimation;
		List<TextMeshProUGUI> descriptionTMPList = new();

		QuestInfo questInfo;
		QuestEntity questEntity;

		protected override void Initialize()
		{
			CacheAnimator();
			OptionGameUI.OnLanguageChangedEvent += OnLanguageChanged;
		}

		public void ShowQuest(QuestInfo questInfo)
		{
			if (string.IsNullOrEmpty(questTitleTMP.text)) animator.SetBool(animFade, true);

			this.questInfo = questInfo;
			IsMainQuest = questInfo.QuestEntity.isMainQuest;

			questEntity = MasterDataManager.Instance.Quest.Get(questInfo.QuestEntity.id);
			DisplayQuestAndDescription();
		}

		public void UpdateTasks()
		{
			UpdateTaskProgress();
		}

		public async UniTask FadeAwayResetText()
		{
			animator.SetBool(animFade, false);
			paddingAnimation.MoveOut();

			await UniTask.NextFrame();

			bool isDone = false;
			CoroutineHelper.WaitUntilCurrentAnimationEnds(animator, () => isDone = true);

			await UniTask.WaitUntil(() => isDone, cancellationToken: this.GetCancellationTokenOnDestroy());

			questTitleTMP.text = "";
			foreach (var descriptionTMP in descriptionTMPList)
			{
				descriptionTMP.text = "";
			}
		}

		void CacheAnimator()
		{
			if (animator) return;

			animator = GetComponentInChildren<Animator>();
			animFade = Animator.StringToHash(parameter);

			descriptionTMPList = descriptionTMPParent.GetComponentsInChildren<TextMeshProUGUI>().ToList();
			paddingAnimation = GetComponentInChildren<PaddingAnimation>();
		}

		void DisplayQuestAndDescription()
		{
			if (questEntity == null) return;

			var questName = MasterLocalization.Get(MasterLocalization.TableNameType.QuestName, questEntity.id);
			questTitleTMP.text = questName;

			UpdateTaskProgress();
		}

		void UpdateTaskProgress()
		{
			var descriptionList = MasterLocalization.GetList(MasterLocalization.TableNameType.QuestDescriptionText, questEntity.id);

			for (int i = 0; i < descriptionTMPList.Count; i++)
			{
				var taskInfo = questInfo.TaskInfoList[i];
				string value = taskInfo.GetProgressOverRequiredAmount();

				descriptionTMPList[i].text = descriptionList[i] + value;
			}
		}

		void OnLanguageChanged(SupportedLanguageType supportedLanguageType)
		{
			DisplayQuestAndDescription();
		}

		void OnDestroy()
		{
			OptionGameUI.OnLanguageChangedEvent -= OnLanguageChanged;
		}
	}
}