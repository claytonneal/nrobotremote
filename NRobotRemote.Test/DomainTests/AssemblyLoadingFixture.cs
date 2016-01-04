using NRobotRemote.Config;
using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.DomainTests
{

#pragma warning disable 1591

    /// <summary>
    /// Tests that check loading of keyword assembly
    /// </summary>
    [TestFixture]
    public class AssemblyLoadingFixture
    {


        [Test]
        public void LoadAssembly_WithTestKeywords()
        {
            //setup
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            //config
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.TestKeywords";
            config.Documentation = "NRobotRemote.Test.XML";
            //load
            kwmanager.AddLibrary(config);
        }

        [Test]
        [ExpectedException(typeof(KeywordLoadingException))]
        public void LoadAssembly_WithUnknownAssembly()
        {
            //setup
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            //config
            config.Assembly = "NRobotRemote.Test.UnknownAssembly";
            config.TypeName = "NRobotRemote.Test.Keywords.TestKeywords";
            //load
            kwmanager.AddLibrary(config);
        }

        [Test]
        [ExpectedException(typeof(KeywordLoadingException))]
        public void LoadAssembly_WithUnknownType()
        {
            //setup
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            //config
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.UnknownType";
            //load
            kwmanager.AddLibrary(config);
        }

        [Test]
        [ExpectedException(typeof(KeywordLoadingException))]
        public void LoadAssembly_WithUnknownXMLDocumentation()
        {
            //setup
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            //config
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.TestKeywords";
            config.Documentation = "NRobotRemote.Test.Unknown.xml";
            //load
            kwmanager.AddLibrary(config);
        }

        [Test]
        [ExpectedException(typeof(KeywordLoadingException))]
        public void LoadAssembly_WithNullConfig()
        {
            //setup
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            //load
            kwmanager.AddLibrary(config);
        }

        [Test]
        public void LoadAssembly_GACAssembly()
        {
            //setup
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            //config
            config.Assembly = "system, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            config.TypeName = "System.Timers.Timer";
            //load
            kwmanager.AddLibrary(config);
        }

        [Test]
        public void LoadAssembly_LoadMultiple()
        {
            //setup
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            //config
            config.Assembly = "system, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            config.TypeName = "System.Timers.Timer";
            //load first
            kwmanager.AddLibrary(config);
            //config
            config.Assembly = "NRobotRemote.Test";
            config.TypeName = "NRobotRemote.Test.Keywords.TestKeywords";
            //load second
            kwmanager.AddLibrary(config);
            var result = kwmanager.GetAllKeywordNames();
            Assert.Contains("PUBLIC METHOD", result);
        }

    }


#pragma warning restore 1591

}
