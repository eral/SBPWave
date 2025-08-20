using eral.SBPWave.Interfaces;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Tasks {

	public class ExportVariantMap : IBuildTask {
		#region Public fields and properties

		public int Version => 1;

		#endregion
		#region Public methods

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

		#endregion
		#region Private fields and properties

#pragma warning disable 649
		[InjectContext(ContextUsage.In)]
		private IBundleBuildContent m_Content;

		[InjectContext(ContextUsage.In, true)]
		private IBuildVariantMap m_VariantMap;

		[InjectContext(ContextUsage.In, true)]
		private IImportVariantMap m_ImportVariantMap; //VariantMap���g���ꍇ�^�X�N�Ȃ�InjectContext(ContextUsage.In)���邾�������AVariantPackedIdentifiers(IDeterministicIdentifiers)�̓^�X�N���ł͂Ȃ��̂ł���Ŏ󂯓n��
#pragma warning restore 649

		#endregion
	}

}