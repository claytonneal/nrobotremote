using NRobotRemote.Config;
using NRobotRemote.Domain;
using NUnit.Framework;

namespace NRobotRemote.Test.ServiceTests
{

#pragma warning disable 1591

    /// <summary>
    /// Tests to start and stop the service with different configurations
    /// </summary>
    [TestFixture]
    public class StartStopServiceFixture
    {

        private NRobotRemoteService _service;

        [TearDown]
        public void TearDown()
        {
            if (_service != null)
            {
                _service.Stop();
            }
        }


        [Test]
        public void StartService_SingleType()
        {
            var config = new NRobotRemoteServiceConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.TestKeywords", 
                new LibraryConfig() 
                { Assembly = "NRobotRemote.Test", 
                    TypeName = "NRobotRemote.Test.Keywords.TestKeywords", 
                    Documentation = "NRobotRemote.Test.XML" });
            _service = new NRobotRemoteService(config);
            _service.StartAsync();
        }

        [Test]
        public void StartService_MultipleTypes()
        {
            var config = new NRobotRemoteServiceConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.Test",
                    TypeName = "NRobotRemote.Test.Keywords.TestKeywords",
                    Documentation = "NRobotRemote.Test.XML"
                });
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.RunKeyword",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.Test",
                    TypeName = "NRobotRemote.Test.Keywords.RunKeyword",
                    Documentation = "NRobotRemote.Test.XML"
                });
            _service = new NRobotRemoteService(config);
            _service.StartAsync();
        }

        [Test]
        public void StartService_NoDocumentation()
        {
            var config = new NRobotRemoteServiceConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.Test",
                    TypeName = "NRobotRemote.Test.Keywords.TestKeywords"
                });
            _service = new NRobotRemoteService(config);
            _service.StartAsync();
        }

        [ExpectedException(typeof(KeywordLoadingException))]
        [Test]
        public void StartService_InvalidAssembly()
        {
            var config = new NRobotRemoteServiceConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.TestUnknown",
                    TypeName = "NRobotRemote.Test.Keywords.TestKeywords",
                    Documentation = "NRobotRemote.Test.XML"
                });
            _service = new NRobotRemoteService(config);
            _service.StartAsync();
        }

        [ExpectedException(typeof(KeywordLoadingException))]
        [Test]
        public void StartService_InvalidType()
        {
            var config = new NRobotRemoteServiceConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.Test",
                    TypeName = "NRobotRemote.Test.Keywords.TestKeywordsUnknown",
                    Documentation = "NRobotRemote.Test.XML"
                });
            _service = new NRobotRemoteService(config);
            _service.StartAsync();
        }

        [ExpectedException(typeof(KeywordLoadingException))]
        [Test]
        public void StartService_InvalidDocumentation()
        {
            var config = new NRobotRemoteServiceConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.Test",
                    TypeName = "NRobotRemote.Test.Keywords.TestKeywords",
                    Documentation = "NRobotRemote.TestUnknown.XML"
                });
            _service = new NRobotRemoteService(config);
            _service.StartAsync();
        }


    }

#pragma warning restore 1591

}
