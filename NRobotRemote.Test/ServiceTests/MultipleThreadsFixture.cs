﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CookComputing.XmlRpc;
using NRobotRemote.Config;
using NRobotRemote.Services;
using NUnit.Framework;

namespace NRobotRemote.Test.ServiceTests
{

#pragma warning disable 1591

    /// <summary>
    /// Tests to call the service xml-rpc methods with multiple threads
    /// </summary>
    [TestFixture]
    public class MultipleThreadsFixture
    {

        private NRobotRemoteService _service;
        private const int numthreads = 5;
        private const int numloops = 10;
        private const int loopwait = 250;
        private int threaderrorcount = 0;

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
            var threads = new List<Task<List<string[]>>>();
            threaderrorcount = 0;
            //start all threads
            for (int counter = 0; counter < numthreads; counter++)
            {
                threads.Add(Task<List<string[]>>.Factory.StartNew(get_keyword_names_worker));
            }
            //get thread results
            foreach (var task in threads)
            {
                var result = task.Result;
                Assert.IsTrue(result.Count == numloops);
                foreach (var names in result)
                {
                    Assert.IsTrue(names.Length > 0);
                    Assert.Contains("WRITESTRACEOUTPUT", names);
                }
            }
            Assert.IsTrue(threaderrorcount == 0);
        }

        private List<string[]> get_keyword_names_worker()
        {
            //setup client
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270";
            var output = new List<string[]>();
            //loop calling method
            for (int counter = 0; counter < numloops; counter++)
            {
                try
                {
                    var result = client.get_keyword_names();
                    output.Add(result);
                    Thread.Sleep(loopwait);
                }
                catch (Exception)
                {
                    threaderrorcount += 1;
                }
            }
            return output;
        }

#endregion

#region get_keyword_arguments

        [Test]
        public void get_keyword_arguments()
        {
            var threads = new List<Task<List<string[]>>>();
            threaderrorcount = 0;
            //start all threads
            for (int counter = 0; counter < numthreads; counter++)
            {
                threads.Add(Task<List<string[]>>.Factory.StartNew(get_keyword_arguments_worker));
            }
            //get thread results
            foreach (var task in threads)
            {
                var result = task.Result;
                Assert.IsTrue(result.Count == numloops);
                foreach (var names in result)
                {
                    Assert.IsTrue(names.Length == 2);
                    Assert.Contains("arg1", names);
                    Assert.Contains("arg2", names);
                }
            }
            Assert.IsTrue(threaderrorcount == 0);
        }

        private List<string[]> get_keyword_arguments_worker()
        {
            //setup client
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270";
            var output = new List<string[]>();
            //loop calling method
            for (int counter = 0; counter < numloops; counter++)
            {
                try
                {
                    var result = client.get_keyword_arguments("STRING PARAMETERTYPE");
                    output.Add(result);
                    Thread.Sleep(loopwait);
                }
                catch (Exception)
                {
                    threaderrorcount += 1;
                }
            }
            return output;
        }

#endregion

#region get_keyword_documentation

        [Test]
        public void get_keyword_documentation()
        {
            var threads = new List<Task<List<string>>>();
            threaderrorcount = 0;
            //start all threads
            for (int counter = 0; counter < numthreads; counter++)
            {
                threads.Add(Task<List<string>>.Factory.StartNew(get_keyword_documentation_worker));
            }
            //get thread results
            foreach (var task in threads)
            {
                var result = task.Result;
                Assert.IsTrue(result.Count == numloops);
                foreach (var doc in result)
                {
                    Assert.IsFalse(String.IsNullOrEmpty(doc));
                    Assert.AreEqual("This is a method with a comment", doc);
                }
            }
            Assert.IsTrue(threaderrorcount == 0);
        }

        private List<string> get_keyword_documentation_worker()
        {
            //setup client
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270";
            var output = new List<string>();
            //loop calling method
            for (int counter = 0; counter < numloops; counter++)
            {
                try
                {
                    var result = client.get_keyword_documentation("METHODWITHCOMMENTS");
                    output.Add(result);
                    Thread.Sleep(loopwait);
                }
                catch (Exception)
                {
                    threaderrorcount += 1;
                }
            }
            return output;
        }

#endregion

#region run_keyword

        [Test]
        public void run_keyword()
        {
            var threads = new List<Task<List<XmlRpcStruct>>>();
            threaderrorcount = 0;
            //start all threads
            for (int counter = 0; counter < numthreads; counter++)
            {
                threads.Add(Task<List<XmlRpcStruct>>.Factory.StartNew(run_keyword_worker));
            }
            //get thread results
            foreach (var task in threads)
            {
                var results = task.Result;
                Assert.IsTrue(results.Count == numloops);
                foreach (var result in results)
                {
                    Assert.AreEqual("PASS", (string)result["status"], (string)result["error"]);
                    Assert.AreEqual("OK", (string)result["return"], (string)result["return"]);
                }
            }
            Assert.IsTrue(threaderrorcount == 0);
        }

        private List<XmlRpcStruct> run_keyword_worker()
        {
            //setup client
            var client = (IRemoteClient)XmlRpcProxyGen.Create(typeof(IRemoteClient));
            client.Url = "http://127.0.0.1:8270";
            var output = new List<XmlRpcStruct>();
            //loop calling method
            for (int counter = 0; counter < numloops; counter++)
            {
                try
                {
                    var result = client.run_keyword("MULTITHREADKEYWORD", new object[] { "2000" });
                    output.Add(result);
                    Thread.Sleep(loopwait);
                }
                catch (Exception)
                {
                    threaderrorcount += 1;
                }
            }
            return output;
        }

#endregion

    }

#pragma warning restore 1591

}
