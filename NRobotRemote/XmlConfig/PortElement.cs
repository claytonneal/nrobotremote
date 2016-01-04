﻿using System.Configuration;

namespace NRobotRemote.XmlConfig
{
	/// <summary>
	/// Port number configuration element
	/// </summary>
	public class PortElement : ConfigurationElement
	{
		
		[ConfigurationProperty("number",IsRequired=true)]
		public string Number
		{
			get
			{
				return this["number"] as string;
			}
		}
		
	}
}
