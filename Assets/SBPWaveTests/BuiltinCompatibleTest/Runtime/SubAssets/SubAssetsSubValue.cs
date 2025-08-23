using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class SubAssetsSubValue : ScriptableObject {
		#region Public fields and properties

		public int Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private int m_Value;

		#endregion
	}

}