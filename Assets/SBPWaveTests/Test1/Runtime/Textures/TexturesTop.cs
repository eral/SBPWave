using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1/TexturesTop")]
	public class TexturesTop : ScriptableObject {
		public TexturesValue Value => m_Value;

		[SerializeField]
		private TexturesValue m_Value;
	}

}