using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/TexturesTop")]
	public class TexturesTop : ScriptableObject {
		public Texture2D Value => m_Value;

		[SerializeField]
		private Texture2D m_Value;
	}

}