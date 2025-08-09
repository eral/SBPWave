using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Content;
using UnityEngine;

namespace eral.SBPWave.Utilities {

	public class BuildReferenceMapUtility {
		[System.Serializable]
		public struct ObjectIdentifierJson {
			public string guid;
			public long localIdentifierInFile;
			public FileType fileType;
			public string filePath;
		}

		public class ReferenceMapObject {
			public string internalFileName;
			public long serializationIndex;
			public ObjectIdentifierJson objectId;
		}

		public List<ReferenceMapObject> Objects {get;private set;}

		public void LoadFrom(BuildReferenceMap refMap) {
			var refMapJson = s_BuildReferenceMapSerializeToJson(refMap);
			var jsonReferenceMap = JsonUtility.FromJson<JsonReferenceMap>(refMapJson);
			var objectsEnumerable = jsonReferenceMap.m_ObjectMap.Select(x=>new ReferenceMapObject{internalFileName = jsonReferenceMap.m_Paths[x.second.serializedFileIndex]
																								, serializationIndex = x.second.localIdentifierInFile
																								, objectId = x.first
																								}
																);
			if (Objects == null) {
				Objects = objectsEnumerable.ToList();
			} else {
				Objects.Clear();
				Objects.AddRange(objectsEnumerable);
			}
		}

		public void SaveTo(BuildReferenceMap refMap) {
			var jsonReferenceMap = new JsonReferenceMap();
			jsonReferenceMap.m_Paths = Objects.Select(x=>x.internalFileName).Distinct().OrderBy(x=>x).ToList();
			var indexDict = jsonReferenceMap.m_Paths.Select((x,i)=>new{value=x, index=i}).ToDictionary(x=>x.value, x=>x.index);
			jsonReferenceMap.m_ObjectMap = Objects.Select(x=>new JsonReferenceMap.ObjectMap{first=x.objectId
																							, second=new JsonReferenceMap.Location{serializedFileIndex=indexDict[x.internalFileName]
																																, localIdentifierInFile=x.serializationIndex
																																}
																							}).ToList();
			var json = JsonUtility.ToJson(jsonReferenceMap);
			s_BuildReferenceMapDeserializeFromJson(refMap, json);
		}

		public void Clear() {
			Objects = null;
		}

		static BuildReferenceMapUtility() {
			var kBindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
			var SerializeToJsonMethodInfo = typeof(BuildReferenceMap).GetMethod("SerializeToJson", kBindingFlags);
			s_BuildReferenceMapSerializeToJson = refMap=>SerializeToJsonMethodInfo.Invoke(refMap, null) as string;
			var DeserializeFromJsonMethodInfo = typeof(BuildReferenceMap).GetMethod("DeserializeFromJson", kBindingFlags);
			s_BuildReferenceMapDeserializeFromJson = (refMap, json)=>DeserializeFromJsonMethodInfo.Invoke(refMap, new object[]{json});
		}

		private class JsonReferenceMap {
			[System.Serializable]
			public struct Location {
				public int serializedFileIndex;
				public long localIdentifierInFile;
			}
			[System.Serializable]
			public struct ObjectMap {
				public ObjectIdentifierJson first;
				public Location second;
			}
			public List<ObjectMap> m_ObjectMap;
			public List<string> m_Paths;
		}

		private class PathsInBuildReferenceMap {
			public List<string> m_Paths;
		}

		private static readonly System.Func<BuildReferenceMap, string> s_BuildReferenceMapSerializeToJson;
		private static readonly System.Action<BuildReferenceMap, string> s_BuildReferenceMapDeserializeFromJson;
	}

}