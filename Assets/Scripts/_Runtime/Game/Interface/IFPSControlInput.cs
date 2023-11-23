using UnityEngine;

namespace Personal.InputProcessing
{
	/// <summary>
	/// Add additional controls here.
	/// </summary>
	public interface IFPSControlInput
	{
		/// <summary>
		/// This handles both the DPad and left analog stick.
		/// </summary>
		void Move(Vector2 direction) { }

		/// <summary>
		/// This handles left and right. True is right.
		/// </summary>
		/// <param name="isFlag"></param>
		void Next(bool isFlag) { }

		void Submit() { }                                   // Submit and Cancel are interchangable(Button South and Button East)
		void Cancel() { }                                   // Submit and Cancel are interchangable(Button South and Button East)

		void SubmitReleased() { }                           // Submit and Cancel are interchangable(Button South and Button East)
		void CancelReleased() { }                           // Submit and Cancel are interchangable(Button South and Button East)

		void Sprint(bool isFlag) { }
		void Jump(bool isFlag) { }

		/// <summary>
		/// Pause Menu
		/// </summary>
		void OpenPauseMenu() { }

		/// <summary>
		/// Inventory
		/// </summary>
		void OpenInventory() { }
		void InventoryIndexSelect(int number) { }
	}
}
