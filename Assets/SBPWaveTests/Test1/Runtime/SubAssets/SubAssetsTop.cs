using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1/SubAssetsTop")]
	public class SubAssetsTop : ScriptableObject {
		public SubAssetsSubValue Value => m_Value;

		[SerializeField]
		private SubAssetsSubValue m_Value;
	}

}