using eral.SBPWave.Interfaces;
using eral.SBPWave.Utilities;
using eral.SBPWave.WriteTypes;
using System.Linq;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.WriteTypes;

namespace eral.SBPWave.Tasks {

	public class VariantlizeLinkDestination : IBuildTask {
		public int Version => 1;

#pragma warning disable 649
		[InjectContext(ContextUsage.In)]
		IBuildVariantMap m_VariantMap;

		[InjectContext]
		IWriteData m_WriteData;
#pragma warning restore 649

		public ReturnCode Run() {
			for (var i = 0; i < m_WriteData.WriteOperations.Count; i++) {
				if (m_WriteData.WriteOperations[i] is AssetBundleWriteOperation abwo) {
					m_WriteData.WriteOperations[i] = new VariantAssetBundleWriteOperation(abwo, m_VariantMap);
				}
			}

			return ReturnCode.Success;
		}
	}

}