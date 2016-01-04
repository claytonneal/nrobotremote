using System;
using System.Configuration;

namespace NRobotRemote.Config
{

	public class AssemblyElementCollection : ConfigurationElementCollection
	{
		
		public AssemblyElement this[int index]
		{
			get
			{
				return base.BaseGet(index) as AssemblyElement;
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				base.BaseAdd(index,value);
			}
		}
		
		protected override ConfigurationElement CreateNewElement()
		{
			return new AssemblyElement();
		}
		
		
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AssemblyElement)element).Type;
		}
		
	}
}
