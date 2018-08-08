using System.Collections.Generic;
using UnityEngine;

namespace KKL
{
    public static class Util
    {
        public static void Table(Dictionary<string, string> data, string separator = ":")
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            foreach (var k in data.Keys) GUILayout.Label(k + separator + " ");
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            foreach (var v in data.Values) GUILayout.Label(v);
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }
    }
}