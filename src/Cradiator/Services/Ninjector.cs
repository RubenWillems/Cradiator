using Ninject;

namespace Cradiator.Services
{
	/// <summary>
	/// service locator, which I tried to avoid, but required to get DI into the xaml converter classes
	/// </summary>
	public class Ninjector
	{
		static IKernel _kernel;

		public static IKernel Kernel
		{
			set { _kernel = value; }
			get { return _kernel; }
		}

		public static T Get<T>()
		{
			return _kernel.Get<T>();
		}
	}
}