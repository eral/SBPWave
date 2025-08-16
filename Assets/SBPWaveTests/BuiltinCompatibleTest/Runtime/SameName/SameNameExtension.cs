using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class SameNameExtension : ScriptableObject {
		public SameNameExtensionSubAsset Value => m_Value;

		[SerializeField]
		private SameNameExtensionSubAsset m_Value;

#if UNITY_EDITOR
		[MenuItem("Assets/Create/SBPWaveTests/BuiltinCompatibleTest/SameNameExtension")]
		public static void CreateAsset() {
			var asset = CreateInstance<SameNameExtension>();

			var resourceFile = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (string.IsNullOrEmpty(resourceFile)) {
				resourceFile = "Assets/";
			}
			while (!Directory.Exists(resourceFile)) {
				resourceFile = Path.GetDirectoryName(resourceFile);
			}
			var endAction = CreateInstance<EndNameEditAction>();
			var pathName = $"New {ObjectNames.NicifyVariableName(nameof(SameNameExtension))}";
			var names = AssetDatabase.FindAssets(pathName, new[]{resourceFile}).Select(x=>Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(x))).ToArray();
			pathName = ObjectNames.GetUniqueName(names, pathName);
			var icon = EditorGUIUtility.FindTexture("ScriptableObject Icon");
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(asset.GetInstanceID(), endAction, pathName, icon, resourceFile);
		}

		private class EndNameEditAction : UnityEditor.ProjectWindowCallback.EndNameEditAction {
			public override void Action(int instanceId, string pathName, string resourceFile) {
				AssetDatabase.StartAssetEditing();

				var asset = EditorUtility.InstanceIDToObject(instanceId);
				AssetDatabase.CreateAsset(asset, $"{pathName}.asset");

				var subAsset = CreateInstance<SameNameExtensionSubAsset>();
				subAsset.name = "png";
				AssetDatabase.AddObjectToAsset(subAsset, asset);

				AssetDatabase.StopAssetEditing();
			}
		}
#endif
	}

}