using eral.SBPWave.Interfaces;
using System.IO;
using UnityEditor;
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
				//�o���A���g�Ώۂ̃A�Z�b�g�o���h���Ȃ�
				//�o���A���g�p��ID�Z�o���s��
				var objectObj = ObjectIdentifier.ToObject(objectID);
				if (objectObj is GameObject objectGo) {
					//�v���t�@�u�͓����ɂ���SObject���ʂɓ����Ă���̂ŁA�S�Ă��o���A���g�pID�ɂ���ƏՓ˂���
					//���̈׃v���t�@�u�̏ꍇ�̓��[�g��GameObject�ȊO�̓o���A���g�p��ID�Z�o���s��Ȃ��悤�ɂ���
					if (!objectGo.transform.parent) {//Missing�ł����Ă��e�����邩������̂Łu==null�v�ɂ͂��Ȃ�����
						//GameObject�Ń��[�g�Ȃ�
						var addressableName = m_BundleBuildContent.Addresses[objectID.guid];
						hashSource = addressableName;
					} else {
						//GameObject�Ń��[�g�ȊO�Ȃ�
						//�ʏ�ID�Z�o���g��
						//empty.
					}
				} else if (objectObj is Component) {
					//(�v���t�@�u����)MonoBehaviour��Transform�Ȃ�
					//�ʏ�ID�Z�o���g��
					//empty.
				} else if (objectObj is Texture) {
					//�e�N�X�`���͊g���q�Ⴂ�œ����t�@�C��������ꍇ������̂ŁA�S�Ă��o���A���g�pID�ɂ���ƏՓ˂���
					//���̈�ID�Z�o�ɂ͊g���q���l������
					var addressableName = m_BundleBuildContent.Addresses[objectID.guid];
					var assetPath = AssetDatabase.GUIDToAssetPath(objectID.guid);
					var assetExtWithoutDot = Path.GetExtension(assetPath).Substring(1);
					hashSource = $"{addressableName}:{assetExtWithoutDot}";
				} else {
					//����ȊO�Ȃ�
					//�T�u�A�Z�b�g�����̏ꍇ�̓T�u�A�Z�b�g���ʂɓ����Ă���̂ŁA�T�u�A�Z�b�g�����l�����ăo���A���g�pID�ɂ��Ȃ��ƏՓ˂���
					var addressableName = m_BundleBuildContent.Addresses[objectID.guid];
					if (AssetDatabase.IsSubAsset(objectObj)) {
						hashSource = $"{addressableName}:{objectObj.name}";
					} else {
						hashSource = addressableName;
					}
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