using System.Collections.Generic;
using UnityEngine;

namespace KKL
{
    public static class Util
    {
        public static void Columns(IEnumerable<string> labels)
        {
            GUILayout.BeginHorizontal();

            foreach (var l in labels)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(l);
                GUILayout.EndVertical();
            }
            
            GUILayout.EndHorizontal();
        }
        
        public static void Table(Dictionary<string, double> data, string separator = ":", int precision = 3)
        {
            var format = "{0:F" + precision + "}";
            var formatted = new Dictionary<string, string>();

            foreach (var p in data) formatted.Add(p.Key, string.Format(format, p.Value));
            
            Table(formatted, separator);
        }
        
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