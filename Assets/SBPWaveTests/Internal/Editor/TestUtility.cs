using System.IO;
using UnityEditor;

namespace eral.SBPWave.Test.Internal.Editor {

	public class TestUtility {
		public static void CreateFolder(string path) {
			if (!Directory.Exists(path)) {
				var parent = Path.GetDirectoryName(path);
				CreateFolder(parent);
				var crnt = Path.GetFileName(path);
				AssetDatabase.CreateFolder(parent, crnt);
			}
		}

		public static void CreateFolderWhenEndIsFile(string path) {
			path = Path.GetDirectoryName(path);
			CreateFolder(path);
		}
	}

}