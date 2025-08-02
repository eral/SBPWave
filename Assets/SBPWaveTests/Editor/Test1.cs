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
		yield return LoadAndTest(kAssetBundleVariants[0], (asset, ab)=>{
			Assert.True(asset != null);
			Assert.AreEqual(kAssetNames[1], asset.name);
			Assert.AreEqual(10001, asset.Value);
			Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[0]}", ab.name);
		});
	}

	[UnityTest]
	public IEnumerator LoadVariant() {
		yield return LoadAndTest(kAssetBundleVariants[1], (asset, ab)=>{
			Assert.True(asset != null);
			Assert.AreEqual(kAssetNames[1], asset.name);
			Assert.AreEqual(10002, asset.Value);
			Assert.AreEqual($"{kAssetBundleNames[1]}.{kAssetBundleVariants[1]}", ab.name);
		});
	}

	[UnityTest]
	public IEnumerator NoLoad() {
		yield return LoadAndTest(null, (asset, ab)=>{
			Assert.True(asset == null);
			Assert.AreNotEqual(null, asset);
			Assert.AreEqual(null, ab);
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
						$"Assets/SBPWaveTests/ScriptableObject/{kAssetNames[0]}.asset"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[0],
					assetNames=new[]{
						$"Assets/SBPWaveTests/ScriptableObject/{kAssetBundleVariants[0]}/{kAssetNames[1]}.asset"
					},
					addressableNames=null
				},
				new AssetBundleBuild{
					assetBundleName=kAssetBundleNames[1],
					assetBundleVariant=kAssetBundleVariants[1],
					assetNames=new[]{
						$"Assets/SBPWaveTests/ScriptableObject/{kAssetBundleVariants[1]}/{kAssetNames[1]}.asset"
					},
					addressableNames=null
				},
			},
			options=BuildAssetBundleOptions.AssetBundleStripUnityVersion | BuildAssetBundleOptions.StripUnatlasedSpriteCopies | BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension,
			//targetPlatform=BuildTarget.StandaloneWindows,
		});
	}

	private const string kAssetBundlesPath = "Assets/SBPWaveTests/AssetBundles~/Test1";
	private readonly string[] kAssetNames = new[]{"Test1Top", "Test1Value" };
	private readonly string[] kAssetBundleNames = new[]{"assetbundle", "assetbundle2" };
	private readonly string[] kAssetBundleVariants = new[]{"int10001", "int10002" };

	private IEnumerator LoadAndTest(string variant, Action<Test1IntValue, AssetBundle> test) {
		var abcReq = AssetBundle.LoadFromFileAsync($"{kAssetBundlesPath}/{kAssetBundleNames[0]}");
		AssetBundleCreateRequest abcReq2 = null;
		if (variant != null) {
			abcReq2 = AssetBundle.LoadFromFileAsync($"{kAssetBundlesPath}/{kAssetBundleNames[1]}.{variant}");
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
