using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave {
	using super = UnityEditor.Build.Pipeline.ContentPipeline;

	public static class ContentPipeline {
		public static ReturnCode BuildAssetBundles(IBundleBuildParameters parameters, IBundleBuildContent content, out IBundleBuildResults result) {
			var taskList = DefaultBuildTasks.Create(DefaultBuildTasks.Preset.AssetBundleCompatible);
			return BuildAssetBundles(parameters, content, out result, taskList);
		}

		public static ReturnCode BuildAssetBundles(IBundleBuildParameters parameters, IBundleBuildContent content, out IBundleBuildResults result, IList<IBuildTask> taskList, params IContextObject[] contextObjects) {
			return super.BuildAssetBundles(parameters, content, out result, taskList, contextObjects);
		}
	}

}