using System;
using NRobotRemote.Config;
using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.DomainTests
{
    
    /// <summary>
    /// Tests that check the xml documentation on a keyword
    /// </summary>
    [TestFixture]
    class KeywordDocumentationFixture
    {

        private KeywordManager _keywordManager;

        [TestFixtureSetUp]
        public void Setup()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.WithDocumentationClass";
            config.Documentation = "NRobotRemote.Test.xml";
            _keywordManager = new KeywordManager();
            _keywordManager.AddLibrary(config);
        }

        [Test]
        public void GetKeywordDocumentation_HasComments()
        {
            var keyword = _keywordManager.GetKeyword("MethodWithComments");
            Assert.IsFalse(String.IsNullOrEmpty(keyword.KeywordDocumentation));
            Assert.IsTrue(keyword.KeywordDocumentation == "This is a method with a comment");
        }

        [Test]
        public void GetKeywordDocumentation_NoComments()
        {
            var keyword = _keywordManager.GetKeyword("MethodWithNoComment");
            Assert.IsTrue(String.IsNullOrEmpty(keyword.KeywordDocumentation));
        }

        [Test]
        public void GetKeywordDocumentation_NoXMLFileLoaded()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.WithDocumentationClass";
            var keywordManager = new KeywordManager();
            keywordManager.AddLibrary(config);
            var keyword = keywordManager.GetKeyword("MethodWithComments");
            Assert.IsTrue(String.IsNullOrEmpty(keyword.KeywordDocumentation));
        }

    }
}
