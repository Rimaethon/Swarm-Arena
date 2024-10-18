using System.IO;
using System.Reflection;
using Scriptable_Objects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
	[CustomEditor(typeof(AudioLibrarySO))]
	public class AudioEnumGenerator : UnityEditor.Editor
	{
		private const string directory_path = "Assets/_Scripts/Enums";

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			AudioLibrarySO sfxLibrarySO = (AudioLibrarySO) target;

			if (GUILayout.Button("Generate Enums"))
			{
				GenerateEnums(sfxLibrarySO);
			}
		}

		private void GenerateEnums(AudioLibrarySO sfxLibrarySO)
		{
			foreach (FieldInfo fieldInfo in sfxLibrarySO.GetType().GetFields())
			{
				if (fieldInfo.FieldType != typeof(AudioClip[])) continue;
				string arrayName = fieldInfo.Name;
				string enumCode = $"public enum {arrayName}\n{{\n";
				AudioClip[] audioClips = (AudioClip[]) fieldInfo.GetValue(sfxLibrarySO);

				for (int i = 0; i < audioClips.Length; i++)
				{
					enumCode += $"\t{audioClips[i].name} = {i},\n";
				}

				enumCode += "}";
				Debug.Log($"Generated Enum for {arrayName}:\n{enumCode}");
				SaveEnumToFile(arrayName, enumCode);
			}
		}

		private void SaveEnumToFile(string arrayName, string enumCode)
		{
			string filePath = $"{directory_path}/{arrayName}.cs";

			if (!AssetDatabase.IsValidFolder(directory_path))
			{
				AssetDatabase.CreateFolder("Assets/_Scripts/", "Enums");
			}

			File.WriteAllText(filePath, enumCode);
			AssetDatabase.Refresh();
			Debug.Log($"Saved Enum for {arrayName} to {filePath}");
		}
	}
}
