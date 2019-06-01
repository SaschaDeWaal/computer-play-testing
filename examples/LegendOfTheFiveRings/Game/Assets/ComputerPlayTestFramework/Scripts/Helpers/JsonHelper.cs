using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine.UI;

namespace ComputerPlayTesting {

	public static class JsonHelper {

		/**
		 * Returns a json string of the object
		 * @param obj is the object that will be used to create the json
		 */
		public static string FromObjectToJson<T>(this T obj) {
			using (MemoryStream msObj = new MemoryStream()) {
				DataContractJsonSerializer js = new DataContractJsonSerializer(obj.GetType());
				js.WriteObject(msObj, obj);
				msObj.Position = 0;

				using (StreamReader sr = new StreamReader(msObj)) {
					string json = sr.ReadToEnd();
					return json;
				}
			}
		}

		/**
		 * This creates a object out of a json string
		 * @param json is the json string that will be used to create the object
		 */
		public static T FromJsonToObject<T>(string json) {
			using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json))) {
				DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
				T newObj = (T) deserializer.ReadObject(ms);

				return newObj;
			}
		}

		public static T CloneObject<T>(this T obj) {
			using (MemoryStream msObj = new MemoryStream()) {
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
				serializer.WriteObject(msObj, obj);
				msObj.Position = 0;

				T newObj = (T) serializer.ReadObject(msObj);
				
				return newObj;
			}
		}
		

	}
}
