using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1/SpritesTop")]
	public class SpritesTop : ScriptableObject {
		public Sprite Value => m_Value;

		[SerializeField]
		private Sprite m_Value;
	}

}