using UnityEngine;

namespace eral.SBPWave.Test.Test1 {

	[CreateAssetMenu(menuName="SBPWaveTests/Test1/TexturesValue")]
	public class TexturesValue : ScriptableObject {
		public Texture2D[] Values => m_Values;
		public Texture2D Value => m_Values[0];


		[SerializeField]
		private Texture2D[] m_Values;
	}

}