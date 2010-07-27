using System;
using Cradiator.Config;

namespace Cradiator.Model
{
	// ReSharper disable MemberCanBeMadeStatic.Global
	/// <summary>
	/// GuiltFactory determines the BuildBusterStrategy given the GuiltStrategyType 
	/// Seems overkill, but still reads better than the code it replaced
	/// There is no interface IGuiltFactory - really, just to avoid even worse overkill
	/// </summary>
	public class GuiltFactory
	{
		public BuildBusterStrategy Get(GuiltStrategyType guiltType)
		{
			switch(guiltType)
			{
				case GuiltStrategyType.First:
					return new FirstGuiltStrategy();

				case GuiltStrategyType.Last:
					return new LastGuiltStrategy();
			}

			throw new UnknownStrategyTypeException();
		}
	}
	// ReSharper restore MemberCanBeMadeStatic.Global

	public class UnknownStrategyTypeException : Exception {}
}