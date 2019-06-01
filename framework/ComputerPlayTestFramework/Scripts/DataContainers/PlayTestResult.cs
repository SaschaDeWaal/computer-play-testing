using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace ComputerPlayTesting {
    
    [DataContract]
    [KnownType("DerivedTypes")]
    public abstract class PlayTestResult {
        
        [DataMember] private string _id;
        [DataMember] public abstract string ResultType { get; set; }

        protected PlayTestResult() {
            _id = Guid.NewGuid().ToString();
        }
        
        private static Type[] DerivedTypes() {
            return Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsSubclassOf(typeof(PlayTestResult))).ToArray();
        }
    }
    
}

