using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace Auto.Utils
{
	public static class UnityObjectEx
	{
		/// <summary>
		/// For instances of classes that are components, but don't have access to .gameObject (i.e. interfaces)
		/// </summary>
		/// <returns>The GameObject.</returns>
		public static GameObject GameObject(this object o)
		{
			Assert.IsTrue(o is Component, "The argument is not a component. Its type is " + o.GetType());
			//Component tmp = o as Component;
			//return (tmp != null) ? tmp.gameObject : null;
			if ((Component)o == null)
			{
				return null;
			}
			else
			{
				return ((Component)o).gameObject;
			}
		}
	}
}

