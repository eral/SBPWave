using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace eral.SBPWave.Test.Test1.Editor {

	public class Test1 {
		[UnityTest]
		public IEnumerator LoadNormal() {
			yield return TestNormal(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadVariant() {
			yield return TestVariant(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator NoLoad() {
			yield return TestNoLoad(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadAssetDirectFromVariant() {
			yield return TestLoadAssetDirectFromVariant(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadNormalBuiltin() {
			yield return TestNormal(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator LoadVariantBuiltin() {
			yield return TestVariant(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator NoLoadBuiltin() {
			yield return TestNoLoad(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator LoadAssetDirectFromVariantBuiltin() {
			yield return TestLoadAssetDirectFromVariant(TestUtility.Style.Builtin);
		}

		[SetUp]
		public void Test1Setup() {
			TestUtility.CallAllStyles(CreateAssetBundles);
		}

		private const string kAssetsBasePath = "Assets/SBPWaveTests/Test1";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/Test1";
		private readonly string[] kAssetNames = new[]{"Test1Top", "Test1Value"};
		private readonly string[] kAssetBundleNames = new[]{"assetbundle", "assetbundle2"};
		private readonly string[] kAssetBundleVariants = new[]{"int10001", "int10002"};
		private readonly int[] kAssetBundleVariantValues = new[]{10001, 10002};

		private void CreateAssetBundles(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			TestUtility.CreateFolder(assetBundlesPath);
			TestUtility.BuildAssetBundles(style, new BuildAssetBundlesParameters{
				outputPath=assetBundlesPath,
				bundleDefinitions=new[]{
					new AssetBundleBuild{
						assetBundleName=kAssetBundleNames[0],
						assetBundleVariant=null,
						assetNames=new[]{
							$"{kAssetsBasePath}/ScriptableObjects/{kAssetNames[0]}.asset"
						},
						addressableNames=null
					},
					new AssetBundleBuild{
						assetBundleName=kAssetBundleNames[1],
						assetBundleVariant=kAssetBundleVariants[0],
						assetNames=new[]{
							$"{kAssetsBasePath}/ScriptableObjects/{kAssetBundleVariants[0]}/{kAssetNames[1]}.asset"
						},
						addressableNames=null
					},
					new AssetBundleBuild{
						assetBundleName=kAssetBundleNames[1],
						assetBundleVariant=kAssetBundleVariants[1],
						assetNames=new[]{
							$"{kAssetsBasePath}/ScriptableObjects/{kAssetBundleVariants[1]}/{kAssetNames[1]}.asset"
						},
						addressableNames=null
					},
				},
				options=BuildAssetBundleOptions.AssetBundleStripUnityVersion | BuildAssetBundleOptions.StripUnatlasedSpriteCopies | BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension,
				//targetPlatform=BuildTarget.StandaloneWindows,
			});
		}

		private IEnumerator TestNormal(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[0], (asset, ab)=>{
				Assert.True(asset != null);
				Assert.AreEqual(kAssetNames[1], asset.name);
				Assert.AreEqual(kAssetBundleVariantValues[0], asset.Value);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[0]}", ab.name);
			});
		}

		private IEnumerator TestVariant(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[1], (asset, ab)=>{
				Assert.True(asset != null);
				Assert.AreEqual(kAssetNames[1], asset.name);
				Assert.AreEqual(kAssetBundleVariantValues[1], asset.Value);
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
				yield return abcReq;
				var ab = abcReq.assetBundle;
				var abReq = ab.LoadAssetAsync<Test1IntValue>(kAssetNames[1]);
				yield return abReq;
				var asset = (Test1IntValue)abReq.asset;
				{
					Assert.AreEqual(kAssetNames[1], asset.name);
					Assert.AreEqual(kAssetBundleVariantValues[i], asset.Value);
				}
				ab.Unload(true);
			}
		}

		private IEnumerator LoadAndTest(TestUtility.Style style, string variant, Action<Test1IntValue, AssetBundle> test) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[0]}");
			AssetBundleCreateRequest abcReq2 = null;
			if (variant != null) {
				abcReq2 = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[1]}.{variant}");
				yield return abcReq2;
			}
			yield return abcReq;
			var ab2 = abcReq2?.assetBundle;
			var ab = abcReq.assetBundle;
			var abReq = ab.LoadAssetAsync<Test1Top>(kAssetNames[0]);
			yield return abReq;
			var asset = (Test1Top)abReq.asset;
			{
				Assert.AreEqual(kAssetNames[0], asset.name);
				test(asset.Value, ab2);
			}
			ab.Unload(true);
			ab2?.Unload(true);
		}
	}

}