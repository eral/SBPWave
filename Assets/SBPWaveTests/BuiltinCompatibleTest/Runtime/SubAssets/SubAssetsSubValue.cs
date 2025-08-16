using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	public class SubAssetsSubValue : ScriptableObject {
		public int Value => m_Value;

		[SerializeField]
		private int m_Value;
	}

}