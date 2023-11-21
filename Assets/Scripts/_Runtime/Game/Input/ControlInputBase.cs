using UnityEngine;

using Personal.GameState;

namespace Personal.InputProcessing
{
	public abstract class ControlInputBase : GameInitialize, IControlInput
	{
		public static ControlInputBase ActiveControlInput { get; protected set; }

		public IControlInput IControlInput { get => this; }

		protected virtual void OnEnable()
		{
			ActiveControlInput = this;
		}

		void IControlInput.Move(Vector2 direction) { Move(direction); }
		void IControlInput.Next(bool isFlag) { Next(isFlag); }

		void IControlInput.DPad(Vector2 direction) { DPad(direction); }
		void IControlInput.LeftAnalog_Move(Vector2 direction) { LeftAnalog_Move(direction); }
		void IControlInput.RightAnalog_Look(Vector2 direction) { RightAnalog_Look(direction); }

		void IControlInput.ButtonSouth_Submit() { ButtonSouth_Submit(); }                       // Submit and Cancel are interchangable(This will always be submit)
		void IControlInput.ButtonEast_Cancel() { ButtonEast_Cancel(); }                         // Submit and Cancel are interchangable(This will always be cancel)
		void IControlInput.ButtonNorth() { ButtonNorth(); }
		void IControlInput.ButtonWest() { ButtonWest(); }

		void IControlInput.ButtonSouth_Submit_Released() { ButtonSouth_Submit_Released(); }     // Submit and Cancel are interchangable(This will always be submit)
		void IControlInput.ButtonEast_Cancel_Released() { ButtonEast_Cancel_Released(); }       // Submit and Cancel are interchangable(This will always be cancel)
		void IControlInput.ButtonNorth_Released() { ButtonNorth_Released(); }
		void IControlInput.ButtonWest_Released() { ButtonWest_Released(); }

		void IControlInput.L1() { L1(); }
		void IControlInput.L2() { L2(); }
		void IControlInput.L3() { L3(); }

		void IControlInput.R1() { R1(); }
		void IControlInput.R2() { R2(); }
		void IControlInput.R3() { R3(); }

		void IControlInput.Share() { Share(); }
		void IControlInput.Option() { Option(); }

		protected virtual void Move(Vector2 direction) { }
		protected virtual void Next(bool isFlag) { }

		protected virtual void DPad(Vector2 direction) { }
		protected virtual void LeftAnalog_Move(Vector2 direction) { }
		protected virtual void RightAnalog_Look(Vector2 direction) { }

		protected virtual void ButtonSouth_Submit() { }
		protected virtual void ButtonEast_Cancel() { }
		protected virtual void ButtonNorth() { }
		protected virtual void ButtonWest() { }

		protected virtual void ButtonSouth_Submit_Released() { }
		protected virtual void ButtonEast_Cancel_Released() { }
		protected virtual void ButtonNorth_Released() { }
		protected virtual void ButtonWest_Released() { }

		protected virtual void L1() { }
		protected virtual void L2() { }
		protected virtual void L3() { }

		protected virtual void R1() { }
		protected virtual void R2() { }
		protected virtual void R3() { }

		protected virtual void Share() { }
		protected virtual void Option() { }
	}
}