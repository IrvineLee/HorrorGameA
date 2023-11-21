using UnityEngine;

namespace Personal.InputProcessing
{
	/// <summary>
	/// Add additional controls here.
	/// </summary>
	public interface IControlInput
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

		void DPad(Vector2 direction) { }
		void LeftAnalog_Move(Vector2 direction) { }
		void RightAnalog_Look(Vector2 direction) { }

		void ButtonSouth_Submit() { }                       // Submit and Cancel are interchangable(Button South and Button East)
		void ButtonEast_Cancel() { }                        // Submit and Cancel are interchangable(Button South and Button East)
		void ButtonNorth() { }
		void ButtonWest() { }

		void ButtonSouth_Submit_Released() { }              // Submit and Cancel are interchangable(Button South and Button East)
		void ButtonEast_Cancel_Released() { }               // Submit and Cancel are interchangable(Button South and Button East)
		void ButtonNorth_Released() { }
		void ButtonWest_Released() { }

		void L1() { }
		void L2() { }
		void L3() { }

		void R1() { }
		void R2() { }
		void R3() { }

		void Share() { }
		void Option() { }
	}
}
