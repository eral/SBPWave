using eral.SBPWave.Interfaces;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline;
using UnityEditor.Build.Pipeline.Interfaces;
using UnityEditor.Build.Pipeline.Utilities;
using UnityEngine;

namespace eral.SBPWave {

	public class VariantPackedIdentifiers : IVariantIdentifiers, IImportVariantMap {
		public IBundleBuildContent BundleBuildContent {set => m_BundleBuildContent = value;}
		public IBuildVariantMap BuildVariantMap {set => m_BuildVariantMap = value;}

		public virtual string GenerateInternalFileName(string name) {
			return m_DeterministicIdentifiers.GenerateInternalFileName(name);
		}

		public virtual long SerializationIndexFromObjectIdentifier(ObjectIdentifier objectID) {
			var hashSource = default(object);
			if ((m_BuildVariantMap != null) && m_BuildVariantMap.BundleLayoutInverse.ContainsKey(objectID.guid)) {
				//バリアント対象のアセットバンドルなら
				//バリアント用のID算出を行う
				var objectObj = ObjectIdentifier.ToObject(objectID);
				if (objectObj is GameObject objectGo) {
					//プレファブは内部にある全Objectが個別に入ってくるので、全てをバリアント用IDにすると衝突する
					//その為プレファブの場合はルートのGameObject以外はバリアント用のID算出を行わないようにする
					if (!objectGo.transform.parent) {//Missingであっても親が居るかを見るので「==null」にはしないこと
						//GameObjectでルートなら
						var addressableName = m_BundleBuildContent.Addresses[objectID.guid];
						hashSource = addressableName;
					} else {
						//GameObjectでルート以外なら
						//通常ID算出を使う
						//empty.
					}
				} else if (objectObj is Component) {
					//(プレファブ内の)MonoBehaviourやTransformなら
					//通常ID算出を使う
					//empty.
				} else {
					//それ以外なら
					var addressableName = m_BundleBuildContent.Addresses[objectID.guid];
					hashSource = addressableName;
				}
			}
			long result;
			if (hashSource != null) {
				var hash = HashingMethods.Calculate(hashSource);
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