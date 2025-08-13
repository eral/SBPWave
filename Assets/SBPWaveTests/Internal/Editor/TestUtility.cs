using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace eral.SBPWave.Test.Internal.Editor {

	public class TestUtility {
		public const BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression
		                                                             | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension
#if UNITY_2022_1_OR_NEWER
		                                                             | BuildAssetBundleOptions.StripUnatlasedSpriteCopies
#endif
		                                                             | BuildAssetBundleOptions.AssetBundleStripUnityVersion;

		public static void CreateFolder(string path) {
			if (!Directory.Exists(path)) {
				var parent = Path.GetDirectoryName(path);
#if UNITY_EDITOR_WIN
				parent = parent.Replace("\\", "/");
#endif
				CreateFolder(parent);
				var crnt = Path.GetFileName(path);
				var guid = AssetDatabase.CreateFolder(parent, crnt);
				if (string.IsNullOrEmpty(guid)) {
					Directory.CreateDirectory(path);
				}
			}
		}

		public static void CreateFolderWhenEndIsFile(string path) {
			path = Path.GetDirectoryName(path);
			CreateFolder(path);
		}

		public static void ClearFolder(string path) {
			if (Directory.Exists(path)) {
				var outFailedPaths = new List<string>();
				var paths = AssetDatabase.FindAssets("t:Object", new[]{path}).Select(x=>AssetDatabase.GUIDToAssetPath(x)).ToArray();
				AssetDatabase.DeleteAssets(paths, outFailedPaths);

				foreach (var directory in Directory.EnumerateDirectories(path)) {
					Directory.Delete(directory, true);
				}
				foreach (var file in Directory.EnumerateFiles(path)) {
					File.Delete(file);
				}
			}
		}

		public enum Style {
			SBPWave,
			Builtin,
		}

		public static void CallAllStyles(System.Action<Style> action) {
			foreach (Style i in System.Enum.GetValues(typeof(Style))) {
				action(i);
			}
		}

		public static string AddStyleStringToEnd(Style style, string src) {
			return $"{src}{style}";
		}

		public static CompatibilityAssetBundleManifest BuildAssetBundles(Style style, string outputPath, AssetBundleBuild[] builds) {
			return BuildAssetBundles(style, outputPath, builds, buildAssetBundleOptions, EditorUserBuildSettings.activeBuildTarget);
		}
		public static CompatibilityAssetBundleManifest BuildAssetBundles(Style style, string outputPath, AssetBundleBuild[] builds, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform) {
			var buildAssetBundles = new System.Func<string, AssetBundleBuild[], BuildAssetBundleOptions, BuildTarget, CompatibilityAssetBundleManifest>[]{
				CompatibilityBuildPipeline.BuildAssetBundles,
				BuildAssetBundlesBuiltin,
			};
			return buildAssetBundles[(int)style](outputPath, builds, assetBundleOptions, targetPlatform);
		}

		private static CompatibilityAssetBundleManifest BuildAssetBundlesBuiltin(string outputPath, AssetBundleBuild[] builds, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform) {
			var manifestBuiltin = BuildPipeline.BuildAssetBundles(outputPath, builds, assetBundleOptions, targetPlatform);
			var results = manifestBuiltin.GetAllAssetBundles().ToDictionary(x=>x, x=>{
				BuildPipeline.GetCRCForAssetBundle($"{outputPath}/{x}", out uint crc);
				return new BundleDetails{
					FileName = x,
					Crc = crc,
					Hash = manifestBuiltin.GetAssetBundleHash(x),
					Dependencies = manifestBuiltin.GetDirectDependencies(x),
				};
			});
				
			var manifest = ScriptableObject.CreateInstance<CompatibilityAssetBundleManifest>();
			manifest.SetResults(results);
			return manifest;
		}
	}

}