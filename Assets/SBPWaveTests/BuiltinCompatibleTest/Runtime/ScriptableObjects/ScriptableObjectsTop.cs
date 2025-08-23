using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/ScriptableObjectsTop")]
	public class ScriptableObjectsTop : ScriptableObject {
		#region Public fields and properties

		public ScriptableObjectsIntValue Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private ScriptableObjectsIntValue m_Value;

		#endregion
	}

}