using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System.Collections;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

namespace eral.SBPWave.Test.Test1 {

	public class TexturesTest {
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
		public IEnumerator LoadDifferentExtension() {
			yield return TestDifferentExtension(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadAssetDirectFromVariant() {
			yield return TestLoadAssetDirectFromVariant(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadShareAsset() {
			yield return TestLoadShareAsset(TestUtility.Style.SBPWave);
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
		public IEnumerator LoadDifferentExtensionBuiltin() {
			yield return TestDifferentExtension(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator LoadAssetDirectFromVariantBuiltin() {
			yield return TestLoadAssetDirectFromVariant(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator LoadShareAssetBuiltin() {
			yield return TestLoadShareAsset(TestUtility.Style.Builtin);
		}

		[OneTimeSetUp]
		public void OneTimeSetUp() {
			TestUtility.CallAllStyles(CreateAssetBundles);
		}

		private const string kAssetsBasePath = "Assets/SBPWaveTests/Test1/Runtime/Textures";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/Textures_";
		private readonly string[] kAssetNames = new[]{"Top", "Value", "Share"};
		private readonly string[] kAssetBundleNames = new[]{"textures_top", "textures_value", "textures_share"};
		private readonly string[] kAssetBundleVariants = new[]{"tex_red", "tex_green", "tex_jpeg"};
		private readonly Color32[] kAssetBundleVariantPixels = new[]{new Color32(0xFF, 0x00, 0x00, 0xFF), new Color32(0x00, 0xFF, 0x00, 0xFF), new Color32(0xFF, 0x00, 0xFF, 0xFF)};
		
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
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[2],
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetBundleVariants[2]}/{kAssetNames[1]}.asset"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[2],
					assetBundleVariant="share",
					assetNames=new[]{
						$"{kAssetsBasePath}/Assets/blue.png",
						$"{kAssetsBasePath}/Assets/blue.jpg"
					},
					addressableNames=null
				},
			};
			TestUtility.BuildAssetBundles(style, assetBundlesPath, builds);
		}

		private IEnumerator TestNormal(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[0], (tex, ab)=>{
				Assert.True(tex != null);
				var pixels = tex.Value.GetPixels32();
				var pixel = pixels[pixels.Length / 2];
				Assert.AreEqual(kAssetBundleVariantPixels[0], pixel);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[0]}", ab.name);
			});
		}

		private IEnumerator TestVariant(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[1], (tex, ab)=>{
				Assert.True(tex != null);
				var pixels = tex.Value.GetPixels32();
				var pixel = pixels[pixels.Length / 2];
				Assert.AreEqual(kAssetBundleVariantPixels[1], pixel);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[1]}", ab.name);
			});
		}

		private IEnumerator TestDifferentExtension(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[2], (tex, ab)=>{
				Assert.True(tex != null);
				var pixels = tex.Value.GetPixels32();
				var pixel = pixels[pixels.Length / 2];
				Assert.AreEqual(kAssetBundleVariantPixels[2], pixel);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[2]}", ab.name);
			});
		}

		private IEnumerator TestNoLoad(TestUtility.Style style) {
			yield return LoadAndTest(style, null, (script, ab)=>{
				Assert.True(script == null);
				Assert.AreNotEqual(null, script);
				Assert.AreEqual(null, ab);
			});
		}

		private IEnumerator TestLoadAssetDirectFromVariant(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			for (var i = 0; i < kAssetBundleVariants.Length; i++) {
				var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[1]}.{kAssetBundleVariants[i]}");
				while (!abcReq.isDone) yield return null;
				var ab = abcReq.assetBundle;
				var abReq = ab.LoadAssetAsync<TexturesValue>(kAssetNames[1]);
				while (!abReq.isDone) yield return null;
				var asset = (TexturesValue)abReq.asset;
				{
					Assert.AreEqual(kAssetNames[1], asset.name);
					var pixels = asset.Value.GetPixels32();
					var pixel = pixels[pixels.Length / 2];
					Assert.AreEqual(kAssetBundleVariantPixels[i], pixel);
				}
				ab.Unload(true);
			}
		}

		public IEnumerator TestLoadShareAsset(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[2]}.share");
			while (!abcReq.isDone) yield return null;
			var ab = abcReq.assetBundle;
			var abReq = ab.LoadAllAssetsAsync<Texture2D>();
			while (!abReq.isDone) yield return null;
			var allAssets = abReq.allAssets;
			{
				Assert.AreEqual(2, allAssets.Length);
				foreach (Texture2D asset in allAssets) {
					Assert.AreEqual("blue", asset.name);
					var pixels = asset.GetPixels32();
					var pixel = pixels[pixels.Length / 2];
					
					if (Color32.Equals(new Color32(0x00, 0x00, 0xFF, 0xFF), pixel)) {
						//empty.
					}  else if (Color32.Equals(new Color32(0x00, 0xFF, 0xFF, 0xFF), pixel)) {
						//empty.
					} else {
						Assert.Fail();
					}
				}
			}
			ab.Unload(true);
		}

		private IEnumerator LoadAndTest(TestUtility.Style style, string variant, System.Action<TexturesValue, AssetBundle> test) {
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
			var abReq = ab.LoadAssetAsync<TexturesTop>(kAssetNames[0]);
			while (!abReq.isDone) yield return null;
			var asset = (TexturesTop)abReq.asset;
			{
				Assert.AreEqual(kAssetNames[0], asset.name);
				test(asset.Value, ab2);
			}
			ab.Unload(true);
			ab2?.Unload(true);
		}
	}

}