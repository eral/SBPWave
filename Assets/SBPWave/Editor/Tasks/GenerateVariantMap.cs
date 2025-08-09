using eral.SBPWave.Interfaces;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Utilities;

namespace eral.SBPWave.Tasks {

	public class GenerateVariantMap : IBuildTask {
		public int Version => 1;

#pragma warning disable 649
		[InjectContext(ContextUsage.In)]
		IBundleBuildContent m_Content;

		[InjectContext(ContextUsage.In)]
		IDeterministicIdentifiers m_PackingMethod;

		[InjectContext(ContextUsage.In, true)]
		IVariantIdentifiers m_VariantIdentifiers;

		[InjectContext(ContextUsage.Out, true)]
		IBuildVariantMap m_VariantMap;
#pragma warning restore 649

		public ReturnCode Run() {
			if (m_VariantIdentifiers == null) {
				m_VariantMap = null;
				return ReturnCode.SuccessNotRun;
			}

			var buildVariantMap = new BuildVariantMap();
			ApplyBundleLayoutInverse(buildVariantMap, m_Content.BundleLayout);
			ApplyGroupInternalNames(buildVariantMap, m_Content.BundleLayout.Keys, m_VariantIdentifiers, m_PackingMethod);
			m_VariantMap = buildVariantMap;

			return ReturnCode.Success;
		}

		private static void ApplyBundleLayoutInverse(BuildVariantMap buildVariantMap, IEnumerable<KeyValuePair<string, List<GUID>>> bundleLayout) {
			var bundleLayoutInverse = buildVariantMap.BundleLayoutInverse;
			foreach (var bundle in bundleLayout) {
				if (0 <= bundle.Key.IndexOf('.')) {
					//Variant
					foreach (var guid in bundle.Value) {
						if (bundleLayoutInverse.TryGetValue(guid, out var bundleNames)) {
							bundleNames.Add(bundle.Key);
						} else {
							bundleLayoutInverse.Add(guid, new List<string>{bundle.Key});
						}
					}
				}
			}
		}

		private static void ApplyGroupInternalNames(BuildVariantMap buildVariantMap, IEnumerable<string> bundleLayoutKeys, IVariantIdentifiers variantIdentifiers, IDeterministicIdentifiers packingMethod) {
			var linkerFileNames = buildVariantMap.LinkerFileNames;
			var linkerNames = buildVariantMap.LinkerNames;
			foreach (var bundleName in bundleLayoutKeys) {
				var linkerFileName = variantIdentifiers.GenerateLinkerFileName(bundleName);
				if (string.IsNullOrEmpty(linkerFileName)) {
					throw new System.InvalidOperationException($"{nameof(IVariantIdentifiers)}.{nameof(IVariantIdentifiers.GenerateLinkerFileName)}(\"{bundleName}\");");
				}
				var internalFileName = packingMethod.GenerateInternalFileName(bundleName);
				if (linkerFileName.Length != internalFileName.Length) {
					throw new System.InvalidOperationException($"{nameof(IVariantIdentifiers)}.{nameof(IVariantIdentifiers.GenerateLinkerFileName)}(\"{bundleName}\");");
				}
				if (linkerFileName != internalFileName) {
					linkerFileNames.Add(internalFileName, linkerFileName);

					var internalName = string.Format(CommonStrings.AssetBundleNameFormat, internalFileName);
					var linkerName = string.Format(CommonStrings.AssetBundleNameFormat, linkerFileName);
					linkerNames.Add(internalName, linkerName);
				}
			}
		}
	}

}