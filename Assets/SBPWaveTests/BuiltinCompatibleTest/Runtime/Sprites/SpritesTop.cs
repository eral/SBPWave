using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/SpritesTop")]
	public class SpritesTop : ScriptableObject {
		public Sprite Value => m_Value;

		[SerializeField]
		private Sprite m_Value;
	}

}