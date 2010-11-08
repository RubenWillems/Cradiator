using System.Linq;
using System.Xml.Linq;
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
                                            @"<view url=""http://url1"" " +
                                                @"skin=""Grid"" " +
                                                @"project-regex=""v5.*"" " +
                                                @"category-regex="".*""/>"" " +
                                        "</views>" +
                                    "</configuration>"
                          };
        }

        [Test]
        public void can_parse_views_from_xml()
        {
            var views = _reader.Read();
            views.Count().ShouldBe(1);

            var view1 = views.First();

            view1.URL.ShouldBe("http://url1");
            view1.SkinName.ShouldBe("Grid");
            view1.ProjectNameRegEx.ShouldBe("v5.*");
            view1.CategoryRegEx.ShouldBe(".*");
        }

        [Test]
        public void can_writer_1st_view_from_xml()
        {
            var views = _reader.Read();
            var xml = _reader.Write(new ViewSettings
                              {
                                URL = "http://new",
                                ProjectNameRegEx = "[a-z]",  
                                CategoryRegEx = "[1-9]",  
                                SkinName = "StackPhoto",  
                              });

            _reader.Xml = xml;
            views = _reader.Read();
            var view1 = views.First();

            view1.URL.ShouldBe("http://new");
            view1.SkinName.ShouldBe("StackPhoto");
            view1.ProjectNameRegEx.ShouldBe("[a-z]");
            view1.CategoryRegEx.ShouldBe("[1-9]");
        }
    }
}