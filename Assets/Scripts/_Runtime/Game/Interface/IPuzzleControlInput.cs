using UnityEngine;

namespace Personal.InputProcessing
{
	public interface IPuzzleControlInput
	{
		void Move(Vector2 direction) { }

		void Submit() { }                       // Submit and Cancel are interchangable(Button South and Button East)
		void Cancel() { }                       // Submit and Cancel are interchangable(Button South and Button East)
		void Reset() { }
		void AutoComplete() { }
	}
}
