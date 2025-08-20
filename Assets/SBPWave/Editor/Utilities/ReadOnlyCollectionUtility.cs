using System.Collections.ObjectModel;
using System.Reflection;

namespace eral.SBPWave.Utilities {

	public static class ReadOnlyCollectionUtility<T> where T : struct {
		#region Public methods

		public static T[] GetInternalItems(ReadOnlyCollection<T> src) {
			var result = kPropertyInfo.GetValue(src);
			return result as T[];
		}

		#endregion
		#region Private const fields

		private const BindingFlags kBindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
		private static readonly PropertyInfo kPropertyInfo = typeof(ReadOnlyCollection<T>).GetProperty("Items", kBindingFlags);

		#endregion
	}

}
