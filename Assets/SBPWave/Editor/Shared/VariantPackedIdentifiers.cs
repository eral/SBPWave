using eral.SBPWave.Interfaces;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.Utilities;

namespace eral.SBPWave {

	public class VariantPackedIdentifiers : IVariantIdentifiers, IImportVariantMap {
		public IBundleBuildContent BundleBuildContent {set => m_BundleBuildContent = value;}
		public IBuildVariantMap BuildVariantMap {set => m_BuildVariantMap = value;}

		public virtual string GenerateInternalFileName(string name) {
			return m_DeterministicIdentifiers.GenerateInternalFileName(name);
		}

		public virtual long SerializationIndexFromObjectIdentifier(ObjectIdentifier objectID) {
			long result;
			if ((m_BuildVariantMap != null) && m_BuildVariantMap.BundleLayoutInverse.ContainsKey(objectID.guid)) {
				//バリアント対象のアセットバンドルなら
				//バリアント用のID算出を行う
				var addressableName = m_BundleBuildContent.Addresses[objectID.guid];
				var hash = HashingMethods.Calculate(addressableName);
				result = System.BitConverter.ToInt64(hash.ToBytes(), 0);
			} else {
				result = m_DeterministicIdentifiers.SerializationIndexFromObjectIdentifier(objectID);
			}
			return result;
		}

		public virtual string GenerateLinkerFileName(string name) {
			var dotIndex = name.IndexOf('.');
			if (0 <= dotIndex) {
				return GenerateInternalFileName(name.Substring(0, dotIndex));
			} else {
				return GenerateInternalFileName(name);
			}
		}

		public VariantPackedIdentifiers(IDeterministicIdentifiers deterministicIdentifiers = null) {
			m_DeterministicIdentifiers = deterministicIdentifiers ?? new PrefabPackedIdentifiers();
		}

		private IDeterministicIdentifiers m_DeterministicIdentifiers;
		private IBundleBuildContent m_BundleBuildContent = default;
		private IBuildVariantMap m_BuildVariantMap = default;
	}

}