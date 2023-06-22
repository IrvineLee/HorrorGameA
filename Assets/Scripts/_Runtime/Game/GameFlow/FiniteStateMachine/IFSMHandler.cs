
using System;

namespace Personal.FSM
{
	public interface IFSMHandler
	{
		void OnBegin(Type type);
		void OnExit();
	}
}