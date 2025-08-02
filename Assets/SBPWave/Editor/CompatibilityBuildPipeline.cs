using System.IO;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace eral.SBPWave {
#if UNITY_2018_3_OR_NEWER
    using BuildCompression = UnityEngine.BuildCompression;
#else
    using BuildCompression = UnityEditor.Build.Content.BuildCompression;
#endif

	public static class CompatibilityBuildPipeline {
		public static CompatibilityAssetBundleManifest BuildAssetBundles(string outputPath, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform) {
			var buildInput = ContentBuildInterface.GenerateAssetBundleBuilds();
			if (targetPlatform == 0) {
				targetPlatform = EditorUserBuildSettings.activeBuildTarget;
			}
			return BuildAssetBundles_Internal(outputPath, new BundleBuildContent(buildInput), assetBundleOptions, targetPlatform);
		}
		public static CompatibilityAssetBundleManifest BuildAssetBundles(string outputPath, AssetBundleBuild[] builds, BuildAssetBundleOptions assetBundleOptions, BuildTarget targetPlatform) {
			if (targetPlatform == 0) {
				targetPlatform = EditorUserBuildSettings.activeBuildTarget;
			}
			return BuildAssetBundles_Internal(outputPath, new BundleBuildContent(builds), assetBundleOptions, targetPlatform);
		}

		public static CompatibilityAssetBundleManifest BuildAssetBundles(BuildAssetBundlesParameters buildParameters) {
			if (buildParameters.targetPlatform == 0) {
				buildParameters.targetPlatform = EditorUserBuildSettings.activeBuildTarget;
			}
			return BuildAssetBundles_Internal(buildParameters.outputPath, new BundleBuildContent(buildParameters.bundleDefinitions), buildParameters.options, buildParameters.targetPlatform);
		}

        internal static CompatibilityAssetBundleManifest BuildAssetBundles_Internal(string outputPath, IBundleBuildContent content, BuildAssetBundleOptions options, BuildTarget targetPlatform)
        {
            var group = BuildPipeline.GetBuildTargetGroup(targetPlatform);
            var parameters = new BundleBuildParameters(targetPlatform, group, outputPath);
            if ((options & BuildAssetBundleOptions.ForceRebuildAssetBundle) != 0)
                parameters.UseCache = false;

            if ((options & BuildAssetBundleOptions.AppendHashToAssetBundleName) != 0)
                parameters.AppendHash = true;

#if UNITY_2018_3_OR_NEWER
            if ((options & BuildAssetBundleOptions.ChunkBasedCompression) != 0)
                parameters.BundleCompression = BuildCompression.LZ4;
            else if ((options & BuildAssetBundleOptions.UncompressedAssetBundle) != 0)
                parameters.BundleCompression = BuildCompression.Uncompressed;
            else
                parameters.BundleCompression = BuildCompression.LZMA;
#else
            if ((options & BuildAssetBundleOptions.ChunkBasedCompression) != 0)
                parameters.BundleCompression = BuildCompression.DefaultLZ4;
            else if ((options & BuildAssetBundleOptions.UncompressedAssetBundle) != 0)
                parameters.BundleCompression = BuildCompression.DefaultUncompressed;
            else
                parameters.BundleCompression = BuildCompression.DefaultLZMA;
#endif

            if ((options & BuildAssetBundleOptions.DisableWriteTypeTree) != 0)
                parameters.ContentBuildFlags |= ContentBuildFlags.DisableWriteTypeTree;

#if BUILD_OPTIONS_RECURSE_DEPENDENCIES_2022_3 || BUILD_OPTIONS_RECURSE_DEPENDENCIES_2023_3 || UNITY_6000_0_OR_NEWER
            if ((options & BuildAssetBundleOptions.RecurseDependencies) != 0)
                parameters.NonRecursiveDependencies = false;
#endif

            IBundleBuildResults results;
            ReturnCode exitCode = ContentPipeline.BuildAssetBundles(parameters, content, out results);
            if (exitCode < ReturnCode.Success)
                return null;

            var manifest = ScriptableObject.CreateInstance<CompatibilityAssetBundleManifest>();
            manifest.SetResults(results.BundleInfos);
            File.WriteAllText(parameters.GetOutputFilePathForIdentifier(Path.GetFileName(outputPath) + ".manifest"), manifest.ToString());
            return manifest;
        }

	}

}