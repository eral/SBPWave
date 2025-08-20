using eral.SBPWave.Interfaces;
using eral.SBPWave.Utilities;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.WriteTypes;
using UnityEngine;

namespace eral.SBPWave.WriteTypes {

	public class VariantAssetBundleWriteOperation : IWriteOperation {
		#region Public fields and properties

		public WriteCommand Command {
			get => m_AssetBundleWriteOperation.Command;
			set => m_AssetBundleWriteOperation.Command = value;
		}
		public BuildUsageTagSet UsageSet {
			get => m_AssetBundleWriteOperation.UsageSet;
			set => m_AssetBundleWriteOperation.UsageSet = value;
		}
		public BuildReferenceMap ReferenceMap {
			get => m_AssetBundleWriteOperation.ReferenceMap;
			set => m_AssetBundleWriteOperation.ReferenceMap = value;
		}
		public Hash128 DependencyHash {
			get => m_AssetBundleWriteOperation.DependencyHash;
			set => m_AssetBundleWriteOperation.DependencyHash = value;
		}

		#endregion
		#region Public methods

		public Hash128 GetHash128() => m_AssetBundleWriteOperation.GetHash128();

		public Hash128 GetHash128(IBuildLogger log) => m_AssetBundleWriteOperation.GetHash128(log);

		public WriteResult Write(string outputFolder, BuildSettings settings, BuildUsageTagGlobal globalUsage) {
			var linkerFileNames = m_BuildVariantMap.LinkerFileNames;
			var linkerNames = m_BuildVariantMap.LinkerNames;

			var internalNameOrigin = m_AssetBundleWriteOperation.Command.internalName;
			{
				if (linkerNames.TryGetValue(m_AssetBundleWriteOperation.Command.internalName, out var linkerName)) {
					m_AssetBundleWriteOperation.Command.internalName = linkerName;
				}
			}

			{
				var isChange = false;
				var buildReferenceMap = BuildReferenceMapUtility.LoadFrom(m_AssetBundleWriteOperation.ReferenceMap);
				foreach (var obj in buildReferenceMap.Objects) {
					if (linkerNames.TryGetValue(obj.internalFileName, out var linkerName)) {
						obj.internalFileName = linkerName;
						isChange = true;
					}
				}
				if (isChange) {
					buildReferenceMap.SaveTo(m_AssetBundleWriteOperation.ReferenceMap);
				}
			}

			var result = m_AssetBundleWriteOperation.Write(outputFolder, settings, globalUsage);

			if (m_AssetBundleWriteOperation.Command.internalName != internalNameOrigin) {
				m_AssetBundleWriteOperation.Command.internalName = internalNameOrigin;

				var internalFileName = m_AssetBundleWriteOperation.Command.fileName;
				if (linkerFileNames.TryGetValue(internalFileName, out var linkerFileName)) {
					var resourceFiles = ReadOnlyCollectionUtility<ResourceFile>.GetInternalItems(result.resourceFiles);
					for (var i = 0; i < resourceFiles.Length; ++i) {
						var fileAlias = resourceFiles[i].fileAlias;
						if (fileAlias.StartsWith(linkerFileName)) {
							if (fileAlias.Length == internalFileName.Length) {
								resourceFiles[i].fileAlias = internalFileName;
							} else {
								var fileAliasRess = $"{internalFileName}{fileAlias.Substring(linkerFileName.Length)}";
								resourceFiles[i].fileAlias = fileAliasRess;
								resourceFiles[i].fileName = $"{outputFolder}/{fileAliasRess}";
							}
						}
					}
				}
			}

			return result;
		}

		public VariantAssetBundleWriteOperation(AssetBundleWriteOperation assetBundleWriteOperation, IBuildVariantMap BuildVariantMap) {
			m_AssetBundleWriteOperation = assetBundleWriteOperation;
			m_BuildVariantMap = BuildVariantMap;
		}

		#endregion
		#region Private fields and properties

		private AssetBundleWriteOperation m_AssetBundleWriteOperation;
		private IBuildVariantMap m_BuildVariantMap;

		#endregion
	}

}