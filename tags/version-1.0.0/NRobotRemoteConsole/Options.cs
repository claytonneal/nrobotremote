using CommandLine;
using CommandLine.Text;
using System;

namespace NRobotRemoteConsole
{
	/// <summary>
	/// Command line options class
	/// </summary>
	public class Options
	{
		[Option('l', "library", Required = true, HelpText = "Keyword assembly")]
		public string library {get; set; }
		
		[Option('t', "type", Required = true, HelpText = "Keyword class name")]
		public string type {get; set;}
		
		[Option('p', "port", Required = true, HelpText = "Server port number")]
		public string port {get; set; }
		
		[Option('d', "doc", Required = false, HelpText = "Keyword assembly documentation")]
		public string docfile {get; set; }
		
		[ParserState]
  		public IParserState LastParserState { get; set; }
  		
  		[HelpOption]
  		public string GetUsage() 
  		{
    		return HelpText.AutoBuild(this,(HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
  		}
		
	}
}
