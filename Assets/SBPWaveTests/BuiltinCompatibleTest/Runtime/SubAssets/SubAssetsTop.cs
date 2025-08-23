using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/SubAssetsTop")]
	public class SubAssetsTop : ScriptableObject {
		#region Public fields and properties

		public SubAssetsSubValue Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private SubAssetsSubValue m_Value;

		#endregion
	}

}