using System.Windows.Data;
using NUnit.Framework;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public abstract class ConverterTestBase
	{
		protected IValueConverter _converter;

		protected abstract IValueConverter CreateConverter();

		[SetUp]
		public void SetUp()
		{
			_converter = CreateConverter();
		}

		protected object DoConvert(object value)
		{
			return _converter.Convert(value, typeof(object), null, null);
		}
	}
}