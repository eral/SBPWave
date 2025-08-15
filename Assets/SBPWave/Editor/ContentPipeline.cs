using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave {
	using super = UnityEditor.Build.Pipeline.ContentPipeline;

	public static class ContentPipeline {
		public static ReturnCode BuildAssetBundles(IBundleBuildParameters parameters, IBundleBuildContent content, out IBundleBuildResults result) {
			var taskList = DefaultBuildTasks.Create(DefaultBuildTasks.Preset.AssetBundleCompatible);
			return BuildAssetBundles_Internal(parameters, content, out result, taskList);
		}

		public static ReturnCode BuildAssetBundles(IBundleBuildParameters parameters, IBundleBuildContent content, out IBundleBuildResults result, IList<IBuildTask> taskList, params IContextObject[] contextObjects) {
			DefaultBuildTasks.SupportVariant(taskList);
			return BuildAssetBundles_Internal(parameters, content, out result, taskList, contextObjects);
		}

		private static ReturnCode BuildAssetBundles_Internal(IBundleBuildParameters parameters, IBundleBuildContent content, out IBundleBuildResults result, IList<IBuildTask> taskList, params IContextObject[] contextObjects) {
			SupportVariant(ref contextObjects);
			return super.BuildAssetBundles(parameters, content, out result, taskList, contextObjects);
		}

		private static void SupportVariant(ref IContextObject[] contextObjects) {
			for (int i = 0; i < contextObjects.Length; i++) {
				var contextObject = contextObjects[i];
				if (contextObject is IDeterministicIdentifiers deterministicIdentifiers) {
					if (!(contextObject is VariantPackedIdentifiers)) {
						contextObjects[i] = new CachedVariantPackedIdentifiers(deterministicIdentifiers);
					}
					return;
				}
			}
			System.Array.Resize(ref contextObjects, contextObjects.Length + 1);
			contextObjects[contextObjects.Length - 1] = new CachedVariantPackedIdentifiers();
		}

	}

}