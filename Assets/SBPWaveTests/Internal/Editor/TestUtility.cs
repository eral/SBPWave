using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace eral.SBPWave.Test.Internal.Editor {

	public class TestUtility {
		public const BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.AssetBundleStripUnityVersion
		                                                             | BuildAssetBundleOptions.ChunkBasedCompression
		                                                             | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension
#if UNITY_2022_1_OR_NEWER
		                                                             | BuildAssetBundleOptions.StripUnatlasedSpriteCopies
#endif
		                                                             | BuildAssetBundleOptions.ForceRebuildAssetBundle;

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