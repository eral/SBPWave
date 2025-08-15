using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace eral.SBPWave.Test.Test1 {

	public class SubAssetsTest {
		[UnityTest]
		public IEnumerator LoadNormal() {
			CreateAssetBundles(TestUtility.Style.SBPWave);
			yield return TestNormal(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadVariant() {
			CreateAssetBundles(TestUtility.Style.SBPWave);
			yield return TestVariant(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator NoLoad() {
			CreateAssetBundles(TestUtility.Style.SBPWave);
			yield return TestNoLoad(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadAssetDirectFromVariant() {
			CreateAssetBundles(TestUtility.Style.SBPWave);
			yield return TestLoadAssetDirectFromVariant(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadNormalBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin);
			yield return TestNormal(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator LoadVariantBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin);
			yield return TestVariant(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator NoLoadBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin);
			yield return TestNoLoad(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator LoadAssetDirectFromVariantBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin);
			yield return TestLoadAssetDirectFromVariant(TestUtility.Style.Builtin);
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

		private const string kAssetsBasePath = "Assets/SBPWaveTests/Test1/Runtime/SubAssets";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/SubAssets_";
		private readonly string[] kAssetNames = new[]{"Top", "Value"};
		private readonly string[] kAssetBundleNames = new[]{"subassets_top", "subassets_value"};
		private readonly string[] kAssetBundleVariants = new[]{"int10001", "int10002"};
		private readonly int[] kAssetBundleVariantValues = new[]{10001, 10002};
		private readonly string[] kSubAssetNames = new[]{"SubValue", "SubValue2"};

		private void CreateAssetBundles(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			TestUtility.CreateFolder(assetBundlesPath);
			var builds = new[]{
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[0],
					assetBundleVariant=null,
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetNames[0]}.asset"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[0],
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetBundleVariants[0]}/{kAssetNames[1]}.asset"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[1],
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetBundleVariants[1]}/{kAssetNames[1]}.asset"
					},
					addressableNames=null
				},
			};
			TestUtility.BuildAssetBundles(style, assetBundlesPath, builds);
		}

		private IEnumerator TestNormal(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[0], (asset, ab)=>{
				Assert.True(asset != null);
				Assert.AreEqual(kSubAssetNames[0], asset.name);
				Assert.AreEqual(kAssetBundleVariantValues[0], asset.Value);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[0]}", ab.name);
			});
		}

		private IEnumerator TestVariant(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[1], (asset, ab)=>{
				if (asset != null) {
					Assert.True(asset != null);
					Assert.AreEqual(kSubAssetNames[0], asset.name);
					Assert.AreEqual(kAssetBundleVariantValues[1], asset.Value);
				} else if ((object)asset != null) {
					Assert.Ignore("Undefined Behavior");
				} else {
					Assert.Fail();
				}
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[1]}", ab.name);
			});
		}

		private IEnumerator TestNoLoad(TestUtility.Style style) {
			yield return LoadAndTest(style, null, (asset, ab)=>{
				Assert.True(asset == null);
				Assert.AreNotEqual(null, asset);
				Assert.AreEqual(null, ab);
			});
		}

		private IEnumerator TestLoadAssetDirectFromVariant(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			for (var i = 0; i < kAssetBundleVariants.Length; i++) {
				var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[1]}.{kAssetBundleVariants[i]}");
				while (!abcReq.isDone) yield return null;
				var ab = abcReq.assetBundle;
				var abReq = ab.LoadAssetAsync<SubAssetsSubValue>(kAssetNames[1]);
				while (!abReq.isDone) yield return null;
				var asset = (SubAssetsSubValue)abReq.asset;
				{
					if (kSubAssetNames[0] == asset.name) {
						Assert.AreEqual(kSubAssetNames[0], asset.name);
					} else if (kSubAssetNames.Contains(asset.name)) {
						Assert.Ignore("Undefined Behavior");
					} else {
						Assert.Fail();
					}
					if (kAssetBundleVariantValues[i] == asset.Value) {
						Assert.AreEqual(kAssetBundleVariantValues[i], asset.Value);
					} else if (kAssetBundleVariantValues.Contains(asset.Value)) {
						Assert.Ignore("Undefined Behavior");
					} else {
						Assert.Fail();
					}
				}
				ab.Unload(true);
			}
		}

		private IEnumerator LoadAndTest(TestUtility.Style style, string variant, System.Action<SubAssetsSubValue, AssetBundle> test) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[0]}");
			AssetBundleCreateRequest abcReq2 = null;
			if (variant != null) {
				abcReq2 = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[1]}.{variant}");
				while (!abcReq2.isDone) yield return null;
			}
			while (!abcReq.isDone) yield return null;
			var ab2 = abcReq2?.assetBundle;
			var ab = abcReq.assetBundle;
			var abReq = ab.LoadAssetAsync<SubAssetsTop>(kAssetNames[0]);
			while (!abReq.isDone) yield return null;
			var asset = (SubAssetsTop)abReq.asset;
			{
				Assert.AreEqual(kAssetNames[0], asset.name);
				test(asset.Value, ab2);
			}
			ab.Unload(true);
			ab2?.Unload(true);
		}
	}

}