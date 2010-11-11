using System.Linq;
using Cradiator.Config;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Config
{
    [TestFixture]
    public class ViewConfigReader_Tests
    {
        ViewSettingsReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = new ViewSettingsReader(Create.Stub<IConfigLocation>())
                          {
                              Xml = "<configuration>" +
                                        "<views>" +
                                            @"<view id=""1"" url=""http://url1"" " +
                                                @"skin=""Grid"" " +
                                                @"project-regex=""v5.*"" " +
                                                @"category-regex="".*""/>"" " +
                                        "</views>" +
                                    "</configuration>"
                          };
        }

        [Test]
        public void can_read_view_from_xml()
        {
            var views = _reader.Read();
            views.Count().ShouldBe(1);

            var view1 = views.First();

            view1.URL.ShouldBe("http://url1");
            view1.SkinName.ShouldBe("Grid");
            view1.ProjectNameRegEx.ShouldBe("v5.*");
            view1.CategoryRegEx.ShouldBe(".*");
        }
    }
}