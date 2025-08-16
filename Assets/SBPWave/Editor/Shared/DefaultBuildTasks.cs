using eral.SBPWave.Tasks;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.Tasks;

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

		public static void SupportVariant(IList<IBuildTask> buildTasks) {
			for (int i = 0; i < buildTasks.Count; i++) {
				var buildTaskType = buildTasks[i].GetType();
				if (buildTaskType == typeof(CalculateSceneDependencyData)) {
					if ((i <= 0) || !(buildTasks[i - 1] is ExportVariantMap)) {
						buildTasks.Insert(i, new ExportVariantMap());
						++i;
					}
					if ((i <= 1) || !(buildTasks[i - 2] is GenerateVariantMap)) {
						buildTasks.Insert(i - 1, new GenerateVariantMap());
						++i;
					}
				} else if (buildTaskType == typeof(WriteSerializedFiles)) {
					if ((i <= 0) || !(buildTasks[i - 1] is VariantlizeLinkDestination)) {
						buildTasks.Insert(i, new VariantlizeLinkDestination());
						++i;
					}
				} else if (buildTaskType == typeof(ArchiveAndCompressBundles)) {
					if ((i <= 0) || !(buildTasks[i - 1] is VariantlizeArchives)) {
						buildTasks.Insert(i, new VariantlizeArchives());
						++i;
					}
				}
			}
#if SBPWAVE_AVOID_DUPLICATE_ADDRESSES_VALIDATION
			if (!(buildTasks[0] is AvoidDuplicateAddressesValidation)) {
				buildTasks.Insert(0, new AvoidDuplicateAddressesValidation());
			}
#endif
		}
	}
}

