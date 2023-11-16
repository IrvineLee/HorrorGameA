

namespace Personal.InputProcessing
{
	/// <summary>
	/// Add additional controls here.
	/// </summary>
	public interface IControlInput
	{
		void Submit() { }              // Submit and Cancel are interchangable(Button South and Button East)
		void Cancel() { }              // Submit and Cancel are interchangable(Button South and Button East)
		void ButtonNorth() { }
		void ButtonWest() { }
		void L3() { }
		void R3() { }
	}
}
