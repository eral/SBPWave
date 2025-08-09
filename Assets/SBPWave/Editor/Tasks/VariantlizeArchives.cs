using eral.SBPWave.Interfaces;
using eral.SBPWave.Utilities;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Tasks {

	public class VariantlizeArchives : IBuildTask {
		public int Version => 1;

#pragma warning disable 649
		[InjectContext(ContextUsage.In)]
		IBuildVariantMap m_VariantMap;

		[InjectContext(ContextUsage.In)]
		IBuildResults m_Results;
#pragma warning restore 649

		public ReturnCode Run() {
			var linkerFileNames = m_VariantMap.LinkerFileNames;

			foreach(var writeResult in m_Results.WriteResults.Values) {
				var resourceFiles = ReadOnlyCollectionUtility<ResourceFile>.GetInternalItems(writeResult.resourceFiles);
				for (var i = 0; i < resourceFiles.Length; ++i) {
					var fileAlias = resourceFiles[i].fileAlias;
					if (fileAlias.Length == NameUtility.kInternalFileNameLength) {
						if (linkerFileNames.TryGetValue(fileAlias, out var linkerFileName)) {
							resourceFiles[i].fileAlias = linkerFileName;
						}
					} else if (fileAlias.StartsWith(NameUtility.kInternalFileNamePrefix)) {
						fileAlias = fileAlias.Substring(0, NameUtility.kInternalFileNameLength);
						if (linkerFileNames.TryGetValue(fileAlias, out var linkerFileName)) {
							resourceFiles[i].fileAlias = $"{linkerFileName}{resourceFiles[i].fileAlias.Substring(NameUtility.kInternalFileNameLength)}";
						}
					}
				}
			}

			return ReturnCode.Success;
		}
	}

}