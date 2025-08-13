using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace eral.SBPWave.Test.Test1 {

	public class SpritesTest {
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

		private const string kAssetsBasePath = "Assets/SBPWaveTests/Test1/Runtime/Sprites";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/Sprites_";
		private readonly string[] kAssetNames = new[]{"Top", "Tex"};
		private readonly string[] kAssetBundleNames = new[]{"sprites_top", "sprites_tex"};
		private readonly string[] kAssetBundleVariants = new[]{"normal", "variant"};
		private readonly Vector2[] kAssetBundleVariantValues = new[]{new Vector2(1,1), new Vector2(9,9)};

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
			};
			TestUtility.BuildAssetBundles(style, assetBundlesPath, builds);
		}

		private IEnumerator TestNormal(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[0], (sprite, ab)=>{
				Assert.True(sprite != null);
				Assert.AreEqual(kAssetNames[1], sprite.name);
				Assert.AreEqual(kAssetBundleVariantValues[0], sprite.rect.position);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[0]}", ab.name);
			});
		}

		private IEnumerator TestVariant(TestUtility.Style style) {
			yield return LoadAndTest(style, kAssetBundleVariants[1], (sprite, ab)=>{
				Assert.True(sprite != null);
				Assert.AreEqual(kAssetNames[1], sprite.name);
				Assert.AreEqual(kAssetBundleVariantValues[1], sprite.rect.position);
				Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[1]}", ab.name);
			});
		}

		private IEnumerator TestNoLoad(TestUtility.Style style) {
			yield return LoadAndTest(style, null, (sprite, ab)=>{
				Assert.True(sprite == null);
				Assert.AreNotEqual(null, sprite);
				Assert.AreEqual(null, ab);
			});
		}

		private IEnumerator TestLoadAssetDirectFromVariant(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			for (var i = 0; i < kAssetBundleVariants.Length; i++) {
				var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleNames[1]}.{kAssetBundleVariants[i]}");
				while (!abcReq.isDone) yield return null;
				var ab = abcReq.assetBundle;
				var abReq = ab.LoadAssetAsync<Sprite>(kAssetNames[1]);
				while (!abReq.isDone) yield return null;
				var asset = (Sprite)abReq.asset;
				{
					Assert.AreEqual(kAssetNames[1], asset.name);
					Assert.AreEqual(kAssetBundleVariantValues[i], asset.rect.position);
				}
				ab.Unload(true);
			}
		}

		private IEnumerator LoadAndTest(TestUtility.Style style, string variant, System.Action<Sprite, AssetBundle> test) {
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
			var abReq = ab.LoadAssetAsync<SpritesTop>(kAssetNames[0]);
			while (!abReq.isDone) yield return null;
			var asset = (SpritesTop)abReq.asset;
			{
				Assert.AreEqual(kAssetNames[0], asset.name);
				test(asset.Value, ab2);
			}
			ab.Unload(true);
			ab2?.Unload(true);
		}
	}

}