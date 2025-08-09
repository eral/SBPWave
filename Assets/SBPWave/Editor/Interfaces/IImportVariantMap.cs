using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave.Interfaces {

	public interface IImportVariantMap : IContextObject {
		IBundleBuildContent BundleBuildContent {set;}
		IBuildVariantMap BuildVariantMap {set;}
	}

}