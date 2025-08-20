using eral.SBPWave.Interfaces;
using eral.SBPWave.WriteTypes;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.WriteTypes;

namespace eral.SBPWave.Tasks {

	public class VariantlizeLinkDestination : IBuildTask {
		#region Public fields and properties

		public int Version => 1;

		#endregion
		#region Public methods

		public ReturnCode Run() {
			for (var i = 0; i < m_WriteData.WriteOperations.Count; i++) {
				if (m_WriteData.WriteOperations[i] is AssetBundleWriteOperation abwo) {
					m_WriteData.WriteOperations[i] = new VariantAssetBundleWriteOperation(abwo, m_VariantMap);
				}
			}

			return ReturnCode.Success;
		}

		#endregion
		#region Private fields and properties

#pragma warning disable 649
		[InjectContext(ContextUsage.In)]
		private IBuildVariantMap m_VariantMap;

		[InjectContext]
		private IWriteData m_WriteData;
#pragma warning restore 649

		#endregion
	}

}