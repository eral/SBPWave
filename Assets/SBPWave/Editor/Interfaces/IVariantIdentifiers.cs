using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Interfaces {

	public interface IVariantIdentifiers : IDeterministicIdentifiers {
		string GenerateLinkerFileName(string name);
	}

}