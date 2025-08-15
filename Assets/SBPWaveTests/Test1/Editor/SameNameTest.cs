using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace eral.SBPWave.Test.Test1 {

	public class SameNameTest {
		[UnityTest]
		public IEnumerator DifferentClassScript() {
			CreateAssetBundles(TestUtility.Style.SBPWave, Pattern.DifferentClassScript);
			return Load(TestUtility.Style.SBPWave, Pattern.DifferentClassScript);
		}

		[UnityTest]
		public IEnumerator DifferentNamespaceScript() {
			CreateAssetBundles(TestUtility.Style.SBPWave, Pattern.DifferentNamespaceScript);
			return Load(TestUtility.Style.SBPWave, Pattern.DifferentNamespaceScript);
		}

		[UnityTest]
		public IEnumerator ScriptAndPrefab() {
			CreateAssetBundles(TestUtility.Style.SBPWave, Pattern.ScriptAndPrefab);
			return Load(TestUtility.Style.SBPWave, Pattern.ScriptAndPrefab);
		}

		[UnityTest]
		public IEnumerator DifferentClassScriptBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin, Pattern.DifferentClassScript);
			return Load(TestUtility.Style.Builtin, Pattern.DifferentClassScript);
		}

		[UnityTest]
		public IEnumerator DifferentNamespaceScriptBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin, Pattern.DifferentNamespaceScript);
			return Load(TestUtility.Style.Builtin, Pattern.DifferentNamespaceScript);
		}

		[UnityTest]
		public IEnumerator ScriptAndPrefabBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin, Pattern.ScriptAndPrefab);
			return Load(TestUtility.Style.Builtin, Pattern.ScriptAndPrefab);
		}

		[OneTimeSetUp]
		public void OneTimeSetUp() {
			TestUtility.CallAllStyles(x=>{
				var assetBundlesPath = TestUtility.AddStyleStringToEnd(x, kAssetBundlesPath);
				TestUtility.ClearFolder(assetBundlesPath);
			});
		}

		[TearDown]
		public void TearDown() {
			AssetBundle.GetAllLoadedAssetBundles().ToList().ForEach(x=>x.Unload(true));
		}

		private enum Pattern {
			DifferentClassScript,
			DifferentNamespaceScript,
			ScriptAndPrefab,
		}
		private const string kAssetsBasePath = "Assets/SBPWaveTests/Test1/Runtime/SameName";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/SameName_";
		private readonly string kAssetName = "Asset";
		private readonly int[][] kAssetPickupIndices = new[]{new[]{0, 1}, new[]{0, 2}, new[]{0, 3}};
		private readonly string kAssetBundleName = "samename";
		private readonly string kAssetBundleVariant = "variant";

		private void CreateAssetBundles(TestUtility.Style style, Pattern pattern) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			TestUtility.CreateFolder(assetBundlesPath);
			var assetNameDatabases = new[]{
				$"{kAssetsBasePath}/script/{kAssetName}.asset",
				$"{kAssetsBasePath}/script2/{kAssetName}.asset",
				$"{kAssetsBasePath}/script3/{kAssetName}.asset",
				$"{kAssetsBasePath}/prefab/{kAssetName}.prefab",
			};
			var builds = new[]{
				new AssetBundleBuild{
					assetBundleName=$"{kAssetBundleName}_{pattern.ToString().ToLower()}",
					assetBundleVariant=kAssetBundleVariant,
					assetNames=kAssetPickupIndices[(int)pattern].Select(x=>assetNameDatabases[x]).ToArray(),
					addressableNames=null
				},
			};
			TestUtility.BuildAssetBundles(style, assetBundlesPath, builds);
		}

		private IEnumerator Load(TestUtility.Style style, Pattern pattern) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleName}_{pattern.ToString().ToLower()}.{kAssetBundleVariant}");
			var ab = abcReq.assetBundle;
			var abReq = ab.LoadAllAssetsAsync();
			while (!abReq.isDone) yield return null;
			var allAssets = abReq.allAssets;
			{
				Assert.AreEqual(2, allAssets.Length);
				foreach (var asset in allAssets) {
					Assert.AreEqual(kAssetName, asset.name);
				}
			}
			ab.Unload(true);
		}
	}

}