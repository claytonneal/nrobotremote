using System;
using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.DomainTests
{
    
    /// <summary>
    /// Test cases for execution keywords via keyword manager
    /// </summary>
    [TestFixture]
    class RunKeywordFixture
    {

        private KeywordManager _keywordManager;

        [TestFixtureSetUp]
        public void Setup()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.RunKeyword";
            _keywordManager = new KeywordManager();
            _keywordManager.AddLibrary(config);
            config.TypeName = "NRobotRemote.Test.Keywords.TestKeywords";
            _keywordManager.AddLibrary(config);
        }

        [Test]
        public void RunKeyword_NoArgs_VoidReturn_NullArgs()
        {
            var result = _keywordManager.RunKeyword("NoInputNoOutput", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.NoError);
            Assert.IsTrue(result.KeywordReturn == null);
        }

        [Test]
        public void RunKeyword_NoArgs_VoidReturn_EmptyArgs()
        {
            var result = _keywordManager.RunKeyword("NoInputNoOutput", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.NoError);
            Assert.IsTrue(result.KeywordReturn == null);
        }

        [Test]
        public void RunKeyword_ThrowsException()
        {
            var result = _keywordManager.RunKeyword("ThrowsException", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.Normal);
            Assert.IsTrue(result.KeywordError.Equals("A regular exception"));
            Assert.IsFalse(String.IsNullOrEmpty(result.KeywordTraceback));
        }

        [Test]
        public void RunKeyword_ThrowsFatalException()
        {
            var result = _keywordManager.RunKeyword("ThrowsFatalException", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.Fatal);
            Assert.IsTrue(result.KeywordError.Equals("A fatal exception"));
            Assert.IsFalse(String.IsNullOrEmpty(result.KeywordTraceback));
        }

        [Test]
        public void RunKeyword_ThrowsContinuableException()
        {
            var result = _keywordManager.RunKeyword("ThrowsContinuableException", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.Continuable);
            Assert.IsTrue(result.KeywordError.Equals("A continuable exception"));
            Assert.IsFalse(String.IsNullOrEmpty(result.KeywordTraceback));
        }

        [Test]
        public void RunKeyword_TraceOutput()
        {
            var result = _keywordManager.RunKeyword("WritesTraceOutput", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordOutput.Contains("First line"));
            Assert.IsTrue(result.KeywordOutput.Contains("Second line"));
        }

        [Test]
        public void RunKeyword_IntReturnType()
        {
            var result = _keywordManager.RunKeyword("Int ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToInt32(result.KeywordReturn) == 1);
        }

        [Test]
        public void RunKeyword_Int64ReturnType()
        {
            var result = _keywordManager.RunKeyword("Int64 ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToInt32(result.KeywordReturn) == 1);
        }

        [Test]
        public void RunKeyword_StringReturnType()
        {
            var result = _keywordManager.RunKeyword("String ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToString(result.KeywordReturn) == "1");
        }

        [Test]
        public void RunKeyword_DoubleReturnType()
        {
            var result = _keywordManager.RunKeyword("Double ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue((Convert.ToDouble(result.KeywordReturn)).Equals(1));
        }

        [Test]
        public void RunKeyword_BooleanReturnType()
        {
            var result = _keywordManager.RunKeyword("Boolean ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToBoolean(result.KeywordReturn));
        }

        [Test]
        public void RunKeyword_StringArrayReturnType()
        {
            var result = _keywordManager.RunKeyword("StringArray ReturnType", null);
            var returnval = (string[]) result.KeywordReturn;
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(returnval.Length == 3);
        }

        [Test]
        public void RunKeyword_LessThanRequiredArgs()
        {
            var result = _keywordManager.RunKeyword("String ParameterType", new object[] {"1"});
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
        }

        [Test]
        public void RunKeyword_MoreThanRequiredArgs()
        {
            var result = _keywordManager.RunKeyword("String ParameterType", new object[] { "1", "2", "3" });
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
        }

        [Test]
        public void RunKeyword_StaticMethod()
        {
            var result = _keywordManager.RunKeyword("PublicStatic Method", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
        }

        [Test]
        public void RunKeyword_KeywordDuration()
        {
            var result = _keywordManager.RunKeyword("PublicStatic Method", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordDuration > 0);
        }


    }
}
