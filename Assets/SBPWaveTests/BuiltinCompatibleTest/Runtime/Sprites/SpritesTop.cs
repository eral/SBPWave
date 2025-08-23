using UnityEngine;

namespace eral.SBPWave.Test.BuiltinCompatibleTest {

	[CreateAssetMenu(menuName="SBPWaveTests/BuiltinCompatibleTest/SpritesTop")]
	public class SpritesTop : ScriptableObject {
		#region Public fields and properties

		public Sprite Value => m_Value;

		#endregion
		#region Private fields and properties

		[SerializeField]
		private Sprite m_Value;

		#endregion
	}

}