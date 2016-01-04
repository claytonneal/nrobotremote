using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.DomainTests
{
    /// <summary>
    /// Tests that checks methods with valid parameter types are considered keywords
    /// </summary>
    [TestFixture]
    class ParameterTypesFixture
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
        public void String_ParameterType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("STRING PARAMETERTYPE", result);
        }

        [Test]
        public void No_Parameters()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("NO PARAMETERS", result);
        }

        [Test]
        public void Integer_ParameterType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("INTEGER PARAMETERTYPE"));
        }

        [Test]
        public void Boolean_ParameterType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("BOOLEAN PARAMETERTYPE"));
        }

        [Test]
        public void Double_ParameterType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("DOUBLE PARAMETERTYPE"));
        }

        [Test]
        public void Mixed_ParameterType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("MIXED PARAMETERTYPE"));
        }

        [Test]
        public void StringArray_ParameterType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("STRINGARRAY PARAMETERTYPE"));
        }


    }
}
