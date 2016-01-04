using NRobotRemote.Config;
using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.DomainTests
{
    
    /// <summary>
    /// Tests for get keyword arguments
    /// </summary>
    [TestFixture]
    class KeywordArgumentsFixture
    {

        private KeywordManager _keywordManager;

        [TestFixtureSetUp]
        public void Setup()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.TestKeywords";
            _keywordManager = new KeywordManager();
            _keywordManager.AddLibrary(config);
        }

        [Test]
        public void GetKeywordArguments_StringArguments()
        {
            var keyword = _keywordManager.GetKeyword("String ParameterType");
            Assert.IsTrue(keyword.ArgumentCount == 2);
            Assert.Contains("arg1", keyword.ArgumentNames);
            Assert.Contains("arg2", keyword.ArgumentNames);
        }

        [Test]
        public void GetKeywordArguments_NoArguments()
        {
            var keyword = _keywordManager.GetKeyword("No Parameters");
            Assert.IsTrue(keyword.ArgumentCount == 0);
        }


    }
}
