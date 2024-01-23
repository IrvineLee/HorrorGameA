
using System;

namespace Personal.InteractiveObject
{
	[Flags]
	public enum InteractableAnimatorType
	{
		BeforeInteract_BeforeTalk = 1 << 0,
		BeforeInteract_AfterTalk = 1 << 1,
		AfterInteract_BeforeTalk = 1 << 2,
		AfterInteract_AfterTalk = 1 << 3,

		All = BeforeInteract_BeforeTalk | BeforeInteract_AfterTalk | AfterInteract_BeforeTalk | AfterInteract_AfterTalk,
	}
}