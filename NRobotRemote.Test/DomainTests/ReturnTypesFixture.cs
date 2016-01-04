using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.DomainTests
{
    /// <summary>
    /// Tests that check methods with valid return types are keywords
    /// </summary>
    [TestFixture]
    class ReturnTypesFixture
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
        public void Int_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("INT RETURNTYPE", result);
        }

        [Test]
        public void Int32_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("INT32 RETURNTYPE", result);
        }

        [Test]
        public void Int64_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("INT64 RETURNTYPE", result);
        }

        [Test]
        public void StringAlias_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("STRINGALIAS RETURNTYPE", result);
        }

        [Test]
        public void String_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("STRING RETURNTYPE", result);
        }

        [Test]
        public void DoubleAlias_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("DOUBLEALIAS RETURNTYPE", result);
        }

        [Test]
        public void Double_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("DOUBLE RETURNTYPE", result);
        }

        [Test]
        public void Single_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("SINGLE RETURNTYPE"));
        }

        [Test]
        public void Decimal_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("DECIMAL RETURNTYPE"));
        }

        [Test]
        public void BooleanAlias_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("BOOLEANALIAS RETURNTYPE", result);
        }

        [Test]
        public void Boolean_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("BOOLEAN RETURNTYPE", result);
        }

        [Test]
        public void StringArray_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("STRINGARRAY RETURNTYPE", result);
        }

        [Test]
        public void IntegerArray_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("INTEGERARRAY RETURNTYPE"));
        }

        [Test]
        public void DoubleArray_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("DOUBLEARRAY RETURNTYPE"));
        }

        [Test]
        public void BooleanArray_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.That(result, Has.No.Member("BOOLEANARRAY RETURNTYPE"));
        }

        [Test]
        public void Void_ReturnType()
        {
            var result = _keywordManager.GetAllKeywordNames();
            Assert.Contains("VOID RETURNTYPE", result);
        }

    }
}
