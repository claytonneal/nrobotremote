using System;
using CookComputing.XmlRpc;
using NRobotRemote.Services;
using NUnit.Framework;

namespace NRobotRemote.Test.ServiceTests
{

#pragma warning disable 1591

    /// <summary>
    /// Tests to call the individual xml-rpc methods and assert the returned values
    /// </summary>
    [TestFixture]
    public class XmlRpcFixture
    {

        private static IRemoteClient _client;
        private NRobotRemoteService _service;

        [SetUp]
        public void Setup()
        {
            //start service
            var config = new NRobotRemoteServiceConfig();
            config.Port = 8270;
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.TestKeywords",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.Test",
                    TypeName = "NRobotRemote.Test.Keywords.TestKeywords",
                    Documentation = "NRobotRemote.Test.XML"
                });
            config.AssemblyConfigs.Add("NRobotRemote.Test.Keywords.WithDocumentationClass",
                new LibraryConfig()
                {
                    Assembly = "NRobotRemote.Test",
                    TypeName = "NRobotRemote.Test.Keywords.WithDocumentationClass",
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
            //setup client
            _client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            _client.Url = "http://127.0.0.1:8270";
        }

        [TearDown]
        public void TearDown()
        {
            if (_service != null)
            {
                _service.Stop();
            }
        }


#region get_keyword_names

        [Test]
        public void get_keyword_names()
        {
            string[] result = _client.get_keyword_names();
            Assert.IsTrue(result.Length > 0);
            Assert.Contains("INT RETURNTYPE",result);
        }

#endregion

#region get_keyword_arguments

        [Test]
        public void get_keyword_arguments()
        {
            string[] result = _client.get_keyword_arguments("STRING PARAMETERTYPE");
            Assert.IsTrue(result.Length > 0);
            Assert.Contains("arg1", result);
            Assert.Contains("arg2", result);
        }

#endregion

#region get_keyword_documentation

        [Test]
        public void get_keyword_documentation()
        {
            string result = _client.get_keyword_documentation("MethodWithComments");
            Assert.IsFalse(String.IsNullOrEmpty(result));
            Assert.IsTrue(result == "This is a method with a comment");
        }

#endregion

#region run_keyword

        // NOTE: These are the same as the domain tests
        // However here we are asserting on the xml-rpc structure returned


        [Test]
        public void RunKeyword_NoArgs_VoidReturn_EmptyArgs()
        {
            var result = _client.run_keyword("NoInputNoOutput", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(String.IsNullOrEmpty(result["error"].ToString()));
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
        }

        [Test]
        public void RunKeyword_ThrowsException()
        {
            var result = _client.run_keyword("ThrowsException", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "FAIL");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsFalse(result.ContainsKey("fatal"));
            Assert.IsFalse(result.ContainsKey("continuable"));
            Assert.IsTrue(result["error"].ToString() == "A regular exception");
            Assert.IsFalse(String.IsNullOrEmpty(result["traceback"].ToString()));
        }

        [Test]
        public void RunKeyword_ThrowsFatalException()
        {
            var result = _client.run_keyword("ThrowsFatalException", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "FAIL");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsTrue(result.ContainsKey("fatal"));
            Assert.IsTrue(result["error"].ToString() == "A fatal exception");
            Assert.IsFalse(String.IsNullOrEmpty(result["traceback"].ToString()));
        }

        [Test]
        public void RunKeyword_ThrowsContinuableException()
        {
            var result = _client.run_keyword("ThrowsContinuableException", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "FAIL");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsTrue(result.ContainsKey("continuable"));
            Assert.IsTrue(result["error"].ToString() == "A continuable exception");
            Assert.IsFalse(String.IsNullOrEmpty(result["traceback"].ToString()));
        }

        [Test]
        public void RunKeyword_TraceOutput()
        {
            var result = _client.run_keyword("WritesTraceOutput", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(String.IsNullOrEmpty(result["return"].ToString()));
            Assert.IsTrue(result["output"].ToString().Contains("First line"));
            Assert.IsTrue(result["output"].ToString().Contains("Second line"));
        }

        [Test]
        public void RunKeyword_IntReturnType()
        {
            var result = _client.run_keyword("Int ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToInt32(result["return"]) == 1);
        }

        [Test]
        public void RunKeyword_Int64ReturnType()
        {
            var result = _client.run_keyword("Int64 ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToInt32(result["return"]) == 1);
        }

        [Test]
        public void RunKeyword_StringReturnType()
        {
            var result = _client.run_keyword("String ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToString(result["return"]) == "1");
        }

        [Test]
        public void RunKeyword_DoubleReturnType()
        {
            var result = _client.run_keyword("Double ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToDouble(result["return"]).Equals(1));
        }

        [Test]
        public void RunKeyword_BooleanReturnType()
        {
            var result = _client.run_keyword("Boolean ReturnType", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(Convert.ToBoolean(result["return"]));
        }

        [Test]
        public void RunKeyword_StringArrayReturnType()
        {
            var result = _client.run_keyword("StringArray ReturnType", new object[0]);
            var returnval = (string[]) result["return"];
            Assert.IsTrue(result["status"].ToString() == "PASS");
            Assert.IsTrue(returnval.Length == 3);
        }

        [Test]
        public void RunKeyword_LessThanRequiredArgs()
        {
            var result = _client.run_keyword("String ParameterType", new object[] {"1"});
            Assert.IsTrue(result["status"].ToString() == "FAIL");
        }

        [Test]
        public void RunKeyword_MoreThanRequiredArgs()
        {
            var result = _client.run_keyword("String ParameterType", new object[] {"1", "2", "3"});
            Assert.IsTrue(result["status"].ToString() == "FAIL");
        }

        [Test]
        public void RunKeyword_StaticMethod()
        {
            var result = _client.run_keyword("PublicStatic Method", new object[0]);
            Assert.IsTrue(result["status"].ToString() == "PASS");
        }


#endregion

    }

#pragma warning restore 1591

}
