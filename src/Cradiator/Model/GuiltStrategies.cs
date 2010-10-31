using System.Text.RegularExpressions;

namespace Cradiator.Model
{
	public class FirstGuiltStrategy : BuildBusterStrategy
	{
		public FirstGuiltStrategy()
            : base(new Regex(@"(\w+)(,|$)")) { }
	}
    
	public class LastGuiltStrategy : BuildBusterStrategy
	{
		public LastGuiltStrategy()
            : base(new Regex(@"[: ].*,* (.*)")) { }
	}

	public class FixerStrategy : BuildBusterStrategy
	{
		public FixerStrategy()
			: base(new Regex(@"(.*).* is fixing")) { }
	}

	public class BuildBusterStrategy
	{
		readonly Regex _regex;

		protected BuildBusterStrategy(Regex regex)
		{
			_regex = regex;
		}

		public bool IsMatch(string text)
		{
			return _regex.Match(text).Success;
		}

		public string Extract(string text)
		{
		    var match = _regex.Match(text);
		    return match.Groups[1].ToString().Trim(new[]{',',':',' '});
		}
	}
}