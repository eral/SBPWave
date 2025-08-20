using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEditor.Build.Pipeline.Interfaces;

namespace eral.SBPWave {

	public class CachedVariantPackedIdentifiers : VariantPackedIdentifiers {
		#region Public methods

		public override long SerializationIndexFromObjectIdentifier(ObjectIdentifier objectID) {
			if (m_VariantSerializationIndexFromObjectIdentifierCache.TryGetValue(objectID, out var cache)) {
				return cache;
			}

			var result = VariantSerializationIndexFromObjectIdentifier(objectID);
			if (result == 0) {
				result = base.SerializationIndexFromObjectIdentifier(objectID);
			} else {
				m_VariantSerializationIndexFromObjectIdentifierCache.Add(objectID, result);
			}
			return result;
		}

		public CachedVariantPackedIdentifiers(IDeterministicIdentifiers deterministicIdentifiers = null) : base(deterministicIdentifiers) {
		}

		#endregion
		#region Private fields and properties

		private Dictionary<ObjectIdentifier, long> m_VariantSerializationIndexFromObjectIdentifierCache = new Dictionary<ObjectIdentifier, long>();

		#endregion
	}

}