using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave {
	using super = UnityEditor.Build.Pipeline.DefaultBuildTasks;

	public static class DefaultBuildTasks {
		public enum Preset {
			AssetBundleCompatible                = super.Preset.AssetBundleCompatible,
			AssetBundleBuiltInShaderExtraction   = super.Preset.AssetBundleBuiltInShaderExtraction,
			AssetBundleShaderAndScriptExtraction = super.Preset.AssetBundleShaderAndScriptExtraction,
		}

		public static IList<IBuildTask> Create(Preset preset) {
			return Create((super.Preset)preset);
		}

		public static IList<IBuildTask> Create(super.Preset preset) {
			var buildTasks = super.Create(preset);

			switch (preset) {
			case super.Preset.AssetBundleCompatible:
			case super.Preset.AssetBundleBuiltInShaderExtraction:
			case super.Preset.AssetBundleShaderAndScriptExtraction:
				SupportVariant(buildTasks);
				break;
			case super.Preset.PlayerScriptsOnly:
			default:
				//empty.
				break;
			}

			return buildTasks;
		}

		private static void SupportVariant(IList<IBuildTask> buildTasks) {
		}
	}
}

