using System;
using System.Collections.Generic;

namespace Florine
{
    // Primary game bottleneck/controller
    public class FTrack
    {
        private static List<string> _Tracks = new List<string>();
        public static void Clear() { _Tracks.Clear(); }
        public static void Add(string s) { 
			//_Tracks.Add(DateTime.Now.ToString() + ": " + s); 
		}
        public static void Track(
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            //Add("Track: " + memberName + " :: " + sourceFilePath + ":" + sourceLineNumber.ToString());
        }
        public static List<string> Get() { return _Tracks; }
    }

    
}
