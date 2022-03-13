using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// Get an instance of all derived interfaces
/// https://stackoverflow.com/questions/26733/getting-all-types-that-implement-an-interface
/// </summary>
class InterfaceGetter
{
	public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class
	{
		var type = typeof(T);
		var types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

		return types;
	}
}