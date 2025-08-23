using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/ScriptableObjectsIntValue")]
	public class ScriptableObjectsIntValue : ScriptableObject {
		#region Public fields and properties

		public int Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private int m_Value;

		#endregion
	}

}