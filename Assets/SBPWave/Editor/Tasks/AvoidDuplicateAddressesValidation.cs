using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Injector;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Tasks {

	public class AvoidDuplicateAddressesValidation : IBuildTask {
		public int Version => 1;

#if SBPWAVE_AVOID_DUPLICATE_ADDRESSES_VALIDATION
#pragma warning disable 649
		[InjectContext(ContextUsage.In)]
		private IBundleBuildContent m_Content;
#pragma warning restore 649
#endif

		public ReturnCode Run() {
#if SBPWAVE_AVOID_DUPLICATE_ADDRESSES_VALIDATION
			if (m_Content is BundleBuildContent) {
				((BundleBuildContent)m_Content).AvoidDuplicateAddressesValidation();
			}
			return ReturnCode.Success;
#else
			return ReturnCode.SuccessNotRun;
#endif
		}
	}

}