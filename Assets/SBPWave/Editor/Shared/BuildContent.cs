using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace eral.SBPWave {
	using super = UnityEditor.Build.Pipeline.BundleBuildContent;

	public class BundleBuildContent : super {
		public BundleBuildContent(IEnumerable<AssetBundleBuild> bundleBuilds) : base(bundleBuilds.Select(SupportVariant)) {
		}

		private static AssetBundleBuild SupportVariant(AssetBundleBuild bundleBuild) {
			var isNotVariant = string.IsNullOrEmpty(bundleBuild.assetBundleVariant);
			return new AssetBundleBuild{
				assetBundleName = ((isNotVariant)? bundleBuild.assetBundleName: $"{bundleBuild.assetBundleName}.{bundleBuild.assetBundleVariant}"),
				assetBundleVariant = null ,
				assetNames = bundleBuild.assetNames,
				addressableNames = bundleBuild.addressableNames ?? ((isNotVariant)
				                                                   ? bundleBuild.assetNames.Select(x=>Path.GetFileNameWithoutExtension(x))
				                                                   : bundleBuild.assetNames.Select(x=>$"{bundleBuild.assetBundleVariant}/{Path.GetFileNameWithoutExtension(x)}")
				                                                   ).ToArray(),
			};
		}
	}

}