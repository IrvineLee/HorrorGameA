using UnityEngine;

namespace Personal.InputProcessing
{
	public interface IUIControlInput
	{
		void Move(Vector2 direction) { }
		void Look(Vector2 direction) { }

		/// <summary>
		/// This handles left and right. True is right.
		/// </summary>
		/// <param name="isFlag"></param>
		void NextShoulder(bool isFlag) { }
		void NextTrigger(bool isFlag) { }

		void Submit() { }                       // Submit and Cancel are interchangable(Button South and Button East)
		void Cancel() { }                       // Submit and Cancel are interchangable(Button South and Button East)

		/// <summary>
		/// This is for resetting the values to default.
		/// </summary>
		void Default() { }

		/// <summary>
		/// These are for dialogue.
		/// </summary>
		void FastForward() { }
		void FastForwardReleased() { }
	}
}
