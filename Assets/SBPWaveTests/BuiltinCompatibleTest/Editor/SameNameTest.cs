using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class SameNameTest {
		#region Public methods

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
		public IEnumerator SubAssetAndExtension() {
			CreateAssetBundles(TestUtility.Style.SBPWave, Pattern.SubAssetAndExtension);
			return Load(TestUtility.Style.SBPWave, Pattern.SubAssetAndExtension);
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

		[UnityTest]
		public IEnumerator SubAssetAndExtensionBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin, Pattern.SubAssetAndExtension);
			return Load(TestUtility.Style.Builtin, Pattern.SubAssetAndExtension);
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

		#endregion
		#region Private types

		private enum Pattern {
			DifferentClassScript,
			DifferentNamespaceScript,
			ScriptAndPrefab,
			SubAssetAndExtension,
		}

		#endregion
		#region Private const fields

		private const string kAssetsBasePath = "Assets/SBPWaveTests/BuiltinCompatibleTest/Runtime/SameName";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/SameName_";
		private readonly string kAssetName = "Asset";
		private readonly int[][] kAssetPickupIndices = new[]{new[]{0, 1}, new[]{0, 2}, new[]{0, 3}, new[]{4, 5}};
		private readonly string kAssetBundleName = "samename";
		private readonly string kAssetBundleVariant = "variant";

		#endregion
		#region Private methods

		private void CreateAssetBundles(TestUtility.Style style, Pattern pattern) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			TestUtility.CreateFolder(assetBundlesPath);
			var assetNameDatabases = new[]{
				$"{kAssetsBasePath}/script/{kAssetName}.asset",
				$"{kAssetsBasePath}/script2/{kAssetName}.asset",
				$"{kAssetsBasePath}/script3/{kAssetName}.asset",
				$"{kAssetsBasePath}/prefab/{kAssetName}.prefab",
				$"{kAssetsBasePath}/extsubasset/{kAssetName}.asset",
				$"{kAssetsBasePath}/exttexture/{kAssetName}.png",
			};
			var builds = new[]{
				new AssetBundleBuild{
					assetBundleName=$"{kAssetBundleName}_{pattern.ToString().ToLower()}",
					assetBundleVariant=kAssetBundleVariant,
					assetNames=kAssetPickupIndices[(int)pattern].Select(x=>assetNameDatabases[x]).ToArray(),
					addressableNames=null
				},
			};

			var ignoreFailingMessagesOld = LogAssert.ignoreFailingMessages;
			LogAssert.ignoreFailingMessages = true;
			var conditions = new List<System.Tuple<string, string, LogType>>();
			Application.LogCallback logMessageReceived = (condition, stackTrace, type)=>{
				conditions.Add(System.Tuple.Create(condition, stackTrace, type));
			};
			Application.logMessageReceived += logMessageReceived;
			TestUtility.BuildAssetBundles(style, assetBundlesPath, builds);
			Application.logMessageReceived -= logMessageReceived;
			LogAssert.ignoreFailingMessages = ignoreFailingMessagesOld;
			if (conditions.Count == 0) {
				//empty.
			} else if ((conditions.Count == 1) && (conditions[0].Item3 == LogType.Error) && (0 == conditions[0].Item1.IndexOf("Building AssetBundle failed because hash collision was detected in the deterministic id generation."))) {
				Assert.Ignore("Undefined Behavior");
			} else {
				Assert.Fail();
			}
		}

		private IEnumerator Load(TestUtility.Style style, Pattern pattern) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleName}_{pattern.ToString().ToLower()}.{kAssetBundleVariant}");
			var ab = abcReq.assetBundle;
			var abReq = ab.LoadAllAssetsAsync();
			while (!abReq.isDone) yield return null;
			var allAssets = abReq.allAssets;
			switch (pattern) {
			case Pattern.SubAssetAndExtension:
				Assert.AreEqual(3, allAssets.Length);
				foreach (var asset in allAssets) {
					Assert.True(new[]{kAssetName, "png"}.Contains(asset.name));
				}
				break;
			default:
				Assert.AreEqual(2, allAssets.Length);
				foreach (var asset in allAssets) {
					Assert.AreEqual(kAssetName, asset.name);
				}
				break;
			}
			ab.Unload(true);
		}

		#endregion
	}

}