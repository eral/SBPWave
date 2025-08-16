using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/ScriptableObjectsTop")]
	public class ScriptableObjectsTop : ScriptableObject {
		public ScriptableObjectsIntValue Value => m_Value;

		[SerializeField]
		private ScriptableObjectsIntValue m_Value;
	}

}