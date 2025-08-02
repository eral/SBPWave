using UnityEngine;

namespace SBPWaveTests {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1IntValue")]
	public class Test1IntValue : ScriptableObject {
		public int Value => m_Value;

		[SerializeField]
		private int m_Value;
	}

}