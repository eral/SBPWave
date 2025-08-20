using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace eral.SBPWave {
	using super = UnityEditor.Build.Pipeline.BundleBuildContent;

	public class BundleBuildContent : super {
		#region Public methods

		public BundleBuildContent(IEnumerable<AssetBundleBuild> bundleBuilds) : base(bundleBuilds.Select(SupportVariant)) {
#if SBPWAVE_AVOID_DUPLICATE_ADDRESSES_VALIDATION
			m_AddressesOfAvoidDuplicateAddressesValidation = bundleBuilds.Where(x=>x.addressableNames == null)
				                                                         .SelectMany(x=>x.assetNames.Select(x=>KeyValuePair.Create(AssetDatabase.GUIDFromAssetPath(x), Path.GetFileNameWithoutExtension(x))))
				                                                         .ToArray();
#endif
		}

#if SBPWAVE_AVOID_DUPLICATE_ADDRESSES_VALIDATION
		public void AvoidDuplicateAddressesValidation() {
			if (m_AddressesOfAvoidDuplicateAddressesValidation != null) {
				foreach (var pair in m_AddressesOfAvoidDuplicateAddressesValidation) {
					Addresses[pair.Key] = pair.Value;
				}
				m_AddressesOfAvoidDuplicateAddressesValidation = null;
			}
		}
#endif

		#endregion
		#region Private fields and properties

#if SBPWAVE_AVOID_DUPLICATE_ADDRESSES_VALIDATION
		private KeyValuePair<GUID, string>[] m_AddressesOfAvoidDuplicateAddressesValidation = default;
#endif

		#endregion
		#region Private methods

		private static AssetBundleBuild SupportVariant(AssetBundleBuild bundleBuild) {
			var isNotVariant = string.IsNullOrEmpty(bundleBuild.assetBundleVariant);
			return new AssetBundleBuild{
				assetBundleName = ((isNotVariant)? bundleBuild.assetBundleName: $"{bundleBuild.assetBundleName}.{bundleBuild.assetBundleVariant}"),
				assetBundleVariant = null ,
				assetNames = bundleBuild.assetNames,
#if SBPWAVE_AVOID_DUPLICATE_ADDRESSES_VALIDATION
				addressableNames = bundleBuild.addressableNames ?? bundleBuild.assetNames,
#else
				addressableNames = bundleBuild.addressableNames ?? bundleBuild.assetNames.Select(x=>Path.GetFileNameWithoutExtension(x)).ToArray(),
#endif
			};
		}

		#endregion
	}

}