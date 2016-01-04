using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.DomainTests
{
    
    /// <summary>
    /// Tests to check different keyword class access levels
    /// </summary>
    [TestFixture]
    class ClassAccessFixture
    {

        [Test]
        public void ClassAccess_PublicClass()
        {
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.TestKeywords";
            kwmanager.AddLibrary(config);
            Assert.True(kwmanager.GetAllKeywordNames().Length > 0);
        }

        [Test]
        public void ClassAccess_InternalClass()
        {
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.InternalClass";
            kwmanager.AddLibrary(config);
            Assert.True(kwmanager.GetAllKeywordNames().Length == 0);
        }

        [Test]
        [ExpectedException(typeof(KeywordLoadingException))]
        public void ClassAccess_StaticClass()
        {
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.StaticClass";
            kwmanager.AddLibrary(config);
        }


    }
}
