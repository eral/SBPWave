using eral.SBPWave.Test.Internal.Editor;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using static System.Net.Mime.MediaTypeNames;
using static UnityEditor.UIElements.ToolbarMenu;

namespace eral.SBPWave.Test.Test1 {

	public class SpritesTest {
		[UnityTest]
		public IEnumerator BuildAndLoad() {
			yield return Test(TestUtility.Style.SBPWave);
		}

		[UnityTest]
		public IEnumerator BuildAndLoadBuiltin() {
			yield return Test(TestUtility.Style.Builtin);
		}

		[TearDown]
		public void TearDown() {
			AssetBundle.GetAllLoadedAssetBundles().ToList().ForEach(x=>x.Unload(true));
		}

		private const string kAssetsBasePath = "Assets/SBPWaveTests/Test1/Runtime/Sprites";
		private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/Sprites_";
		private readonly string kAssetName = "Tex";
		private readonly string kAssetBundleName = "sprites";
		private readonly string kAssetBundleVariant = "variant";

		private IEnumerator Test(TestUtility.Style style) {
			CreateAssetBundles(style);
			return Load(style);
		}

		private void CreateAssetBundles(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			TestUtility.CreateFolder(assetBundlesPath);
			var builds = new[]{
				new AssetBundleBuild{
					assetBundleName=kAssetBundleName,
					assetBundleVariant=kAssetBundleVariant,
					assetNames=new[]{
						$"{kAssetsBasePath}/{kAssetName}.png",
					},
					addressableNames=null
				},
			};
			TestUtility.BuildAssetBundles(style, assetBundlesPath, builds);
		}

		private IEnumerator Load(TestUtility.Style style) {
			var assetBundlesPath = TestUtility.AddStyleStringToEnd(style, kAssetBundlesPath);
			var abcReq = AssetBundle.LoadFromFileAsync($"{assetBundlesPath}/{kAssetBundleName}.{kAssetBundleVariant}");
			var ab = abcReq.assetBundle;
			var abReq = ab.LoadAllAssetsAsync<Sprite>();
			while (!abReq.isDone) yield return null;
			var allAssets = abReq.allAssets;
			{
				Assert.AreEqual(4, allAssets.Length);
				var names = Enumerable.Range(0, 4).Select(x=>$"{kAssetName}_{x}").ToList();
				for (var i = 0; i < allAssets.Length; i++) {
					Assert.True(names.Remove(allAssets[i].name));
				}
				Assert.AreEqual(0, names.Count);
			}
			ab.Unload(true);
		}
	}

}