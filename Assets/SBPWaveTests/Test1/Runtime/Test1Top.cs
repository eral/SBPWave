using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1Top")]
	public class Test1Top : ScriptableObject {
		public Test1IntValue Value => m_Value;

		[SerializeField]
		private Test1IntValue m_Value;
	}

}