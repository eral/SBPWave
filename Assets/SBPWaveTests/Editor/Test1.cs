using System;
using System.Collections;
using System.IO;
using NUnit.Framework;
using SBPWaveTests;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class Test1
{
	[UnityTest]
	public IEnumerator LoadNormal() {
		yield return LoadAndTest(kAssetBundleVariants[0], asset=>{
			Assert.AreEqual("Test1 Int10001", asset.name);
			Assert.AreEqual(10001, asset.Value);
		});
	}

	[UnityTest]
	public IEnumerator LoadVariant() {
		yield return LoadAndTest(kAssetBundleVariants[1], asset=>{
			Assert.AreEqual("Test1 Int10002", asset.name);
			Assert.AreEqual(10002, asset.Value);
		});
	}

	[UnityTest]
	public IEnumerator NoLoad() {
		yield return LoadAndTest(null, asset=>{
			Assert.True(asset == null);
			Assert.AreNotEqual(null, asset);
		});
	}

	[SetUp]
	public void Test1Setup() {
		CreateFolder(kAssetBundlesPath);
		BuildPipeline.BuildAssetBundles(new BuildAssetBundlesParameters{
			outputPath=kAssetBundlesPath,
			bundleDefinitions=new[]{
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[0],
					assetBundleVariant=null,
					assetNames=new[]{
						"Assets/SBPWaveTests/ScriptableObject/Test1Top.asset"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[0],
					assetNames=new[]{
						"Assets/SBPWaveTests/ScriptableObject/Test1 Int10001.asset"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[1],
					assetNames=new[]{
						"Assets/SBPWaveTests/ScriptableObject/Test1 Int10002.asset"
					},
					addressableNames=null
				},
			},
			options=BuildAssetBundleOptions.AssetBundleStripUnityVersion | BuildAssetBundleOptions.StripUnatlasedSpriteCopies,
			//targetPlatform=BuildTarget.StandaloneWindows,
		});
	}

	private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/Test1";
	private readonly string[] kAssetBundleNames = new[]{"assetbundle", "assetbundle2" };
	private readonly string[] kAssetBundleVariants = new[]{"int10001", "int10002" };

	private IEnumerator LoadAndTest(string variant, Action<Test1IntValue> test) {
		var abcReq = AssetBundle.LoadFromFileAsync($"{kAssetBundlesPath}/{kAssetBundleNames[0]}");
		AssetBundleCreateRequest abcReq2 = null;
		if (variant != null) {
			abcReq2 = AssetBundle.LoadFromFileAsync($"{kAssetBundlesPath}/{kAssetBundleNames[1]}.{variant}");
			yield return abcReq2;
		}
		yield return abcReq;
		var ab2 = abcReq2?.assetBundle;
		var ab = abcReq.assetBundle;
		var abReq = ab.LoadAssetAsync<Test1Top>("Test1Top");
		yield return abReq;
		var asset = (Test1Top)abReq.asset;
		{
			Assert.AreEqual("Test1Top", asset.name);
			test(asset.Value);
		}
		ab.Unload(true);
		ab2?.Unload(true);
	}

	private void CreateFolder(string path, bool endIsFile = false) {
		if (endIsFile) {
			path = Path.GetDirectoryName(path);
		}
		if (!Directory.Exists(path)) {
			var parent = Path.GetDirectoryName(path);
			CreateFolder(parent);
			var crnt = Path.GetFileName(path);
			AssetDatabase.CreateFolder(parent, crnt);
		}
	}
}
