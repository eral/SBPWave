using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class TexturesTest {
		#region Public methods

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
		public IEnumerator LoadDifferentExtension() {
			CreateAssetBundles(TestUtility.Style.SBPWave);
			yield return TestDifferentExtension(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator LoadShare() {
			CreateAssetBundles(TestUtility.Style.SBPWave);
			yield return TestShare(TestUtility.Style.SBPWave);
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
		public IEnumerator LoadDifferentExtensionBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin);
			yield return TestDifferentExtension(TestUtility.Style.Builtin);
		}

		[UnityTest]
		public IEnumerator LoadShareBuiltin() {
			CreateAssetBundles(TestUtility.Style.Builtin);
			yield return TestShare(TestUtility.Style.Builtin);
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

		#endregion
		#region Private const fields

		private const string kAssetsBasePath = "Assets/SBPWaveTests/BuiltinCompatibleTest/Runtime/Textures";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/Textures_";
		private readonly string[] kAssetNames = new[]{"Top", "Tex"};
		private readonly string[] kAssetBundleNames = new[]{"textures_top", "textures_value"};
		private readonly string[] kAssetBundleVariants = new[]{"tex_red", "tex_green", "tex_jpeg", "tex_share"};
		private readonly Color32[][] kAssetBundleVariantPixels = new[]{new[]{new Color32(0xFF, 0x00, 0x00, 0xFF)}
		                                                             , new[]{new Color32(0x00, 0xFF, 0x00, 0xFF)}
		                                                             , new[]{new Color32(0xFF, 0x00, 0xFF, 0xFF)}
		                                                             , new[]{new Color32(0x00, 0x00, 0xFF, 0xFF),new Color32(0x00, 0xFF, 0xFF, 0xFF)}
		                                                             };
		
		#endregion
		#region Private methods

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
						$"{kAssetsBasePath}/{kAssetBundleVariants[0]}/{kAssetNames[1]}.png"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[1],
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetBundleVariants[1]}/{kAssetNames[1]}.png"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[2],
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetBundleVariants[2]}/{kAssetNames[1]}.jpg"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[3],
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetBundleVariants[3]}/{kAssetNames[1]}.png",
						$"{kAssetsBasePath}/{kAssetBundleVariants[3]}/{kAssetNames[1]}.jpg"
					},
					addressableNames=null
				},
			};
			TestUtility.BuildAssetBundles(style, assetBundlesPath, builds);
		}

		private IEnumerator TestNormal(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[0], (tex, ab)=>{
				Assert.True(tex != null);
				var pixels = tex.GetPixels32();
				var pixel = pixels[pixels.Length / 2];
				Assert.AreEqual(kAssetBundleVariantPixels[0][0], pixel);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[0]}", ab.name);
			});
		}

		private IEnumerator TestVariant(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[1], (tex, ab)=>{
				Assert.True(tex != null);
				var pixels = tex.GetPixels32();
				var pixel = pixels[pixels.Length / 2];
				Assert.AreEqual(kAssetBundleVariantPixels[1][0], pixel);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[1]}", ab.name);
			});
		}

		private IEnumerator TestNoLoad(TestUtility.Style style) {
			yield return LoadAndTest(style, null, (tex, ab)=>{
				Assert.True(tex == null);
				Assert.AreNotEqual(null, tex);
				Assert.AreEqual(null, ab);
			});
		}

		private IEnumerator TestDifferentExtension(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[2], (tex, ab)=>{
				Assert.True(tex == null);
				Assert.AreNotEqual(null, tex);
				Assert.AreNotEqual(null, ab);
			});
		}

		public IEnumerator TestShare(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[3], (tex, ab)=>{
				Assert.True(tex != null);
				var pixels = tex.GetPixels32();
				var pixel = pixels[pixels.Length / 2];
				Assert.AreEqual(kAssetBundleVariantPixels[3][0], pixel);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[3]}", ab.name);
			});
		}

		private IEnumerator TestLoadAssetDirectFromVariant(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			for (var i = 0; i < kAssetBundleVariants.Length; i++) {
				var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[1]}.{kAssetBundleVariants[i]}");
				while (!abcReq.isDone) yield return null;
				var ab = abcReq.assetBundle;
				var abReq = ab.LoadAssetAsync<Texture2D>(kAssetNames[1]);
				while (!abReq.isDone) yield return null;
				var tex = (Texture2D)abReq.asset;
				{
					Assert.AreEqual(kAssetNames[1], tex.name);
					var pixels = tex.GetPixels32();
					var pixel = pixels[pixels.Length / 2];
					Assert.That(kAssetBundleVariantPixels[i].Contains(pixel));
				}
				ab.Unload(true);
			}
		}

		private IEnumerator LoadAndTest(TestUtility.Style style, string variant, System.Action<Texture2D, AssetBundle> test) {
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

		#endregion
	}

}