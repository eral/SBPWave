using eral.SBPWave.Interfaces;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Tasks {

	public class ExportVariantMap : IBuildTask {
		public int Version => 1;

#pragma warning disable 649
		[InjectContext(ContextUsage.In)]
		private IBundleBuildContent m_Content;

		[InjectContext(ContextUsage.In, true)]
		private IBuildVariantMap m_VariantMap;

		[InjectContext(ContextUsage.In, true)]
		private IImportVariantMap m_ImportVariantMap; //VariantMapを使う場合タスクならInjectContext(ContextUsage.In)するだけだが、VariantPackedIdentifiers(IDeterministicIdentifiers)はタスク側ではないのでこれで受け渡す
#pragma warning restore 649

		public ReturnCode Run() {
			if (m_ImportVariantMap != null) {
				if (m_VariantMap == null) {
					return ReturnCode.MissingRequiredObjects;
				}
				m_ImportVariantMap.BundleBuildContent = m_Content;
				m_ImportVariantMap.BuildVariantMap = m_VariantMap;
			}

			return ReturnCode.Success;
		}
	}

}