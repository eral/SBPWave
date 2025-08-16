using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/SubAssetsTop")]
	public class SubAssetsTop : ScriptableObject {
		public SubAssetsSubValue Value => m_Value;

		[SerializeField]
		private SubAssetsSubValue m_Value;
	}

}