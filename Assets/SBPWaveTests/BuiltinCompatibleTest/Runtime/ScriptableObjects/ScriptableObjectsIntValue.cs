using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/ScriptableObjectsIntValue")]
	public class ScriptableObjectsIntValue : ScriptableObject {
		public int Value => m_Value;

		[SerializeField]
		private int m_Value;
	}

}