using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ExportAssetBundles
{
  public static void BuildAssetBundles(BuildTarget target, string platform_name, string[] bundles_to_build)
  {
    string path = EditorUtility.SaveFolderPanel("Where should we place the " + platform_name + " AssetBundles?", "", "");
    if (string.IsNullOrEmpty(path)) return;
    foreach (string s in bundles_to_build) {
      string dest_fn = s + ".unity3d";
      string dest_path = path + "/" + dest_fn;

      BuildPipeline.BuildPlayer(new string[]{"Assets/AssetBundles/" + s + ".unity"}, dest_path, target, BuildOptions.BuildAdditionalStreamedScenes);
    }
  }

  public static string[] ListAllScenes()
  {
    string[] files = Directory.GetFiles("Assets/AssetBundles");
    List<string> filtered = new List<string>();
    foreach (string s in files) {
      if (s.EndsWith(".unity")) {
        filtered.Add(s);
      }
    }
    return filtered.ToArray();
  }

  [MenuItem ("Assets/Build All AssetBundles For Android")]
  public static void BuildAllAndroid()
  {
    BuildAssetBundles(BuildTarget.Android, "Android", ListAllScenes());
  }

  [MenuItem ("Assets/Build All AssetBundles For Mac")]
  public static void BuildAllMac()
  {
    BuildAssetBundles(BuildTarget.StandaloneOSXIntel64, "Mac", ListAllScenes());
  }

  [MenuItem ("Assets/Build All AssetBundles For iOS")]
  public static void BuildAlliPhone()
  {
    BuildAssetBundles(BuildTarget.iPhone, "iOS", ListAllScenes());
  }

  [MenuItem ("Assets/Build All AssetBundles For Windows")]
  public static void BuildAllStandaloneWindows()
  {
    BuildAssetBundles(BuildTarget.StandaloneWindows, "Windows", ListAllScenes());
  }

  public static string[] ListSelectedScenes()
  {
    List<string> scenes = new List<string>();
    foreach (Object o in Selection.objects) {
      string path = AssetDatabase.GetAssetPath(o);

      if (path.EndsWith(".unity")) {
        scenes.Add(Path.GetFileNameWithoutExtension(path));
      }
    }

    if (scenes.Count == 0) {
        UnityEngine.Debug.LogWarning("No scenes are selected");
    }

    return scenes.ToArray();
  }

  [MenuItem ("Assets/Build Selected AssetBundles For Android")]
  public static void BuildSingleAndroid()
  {
    string[] input = ListSelectedScenes();
    if (input.Length > 0) {
      BuildAssetBundles(BuildTarget.Android, "Android", input);
    }
  }

  [MenuItem ("Assets/Build Selected AssetBundles For Mac")]
  public static void BuildSingleMac()
  {
    string[] input = ListSelectedScenes();
    if (input.Length > 0) {
      BuildAssetBundles(BuildTarget.StandaloneOSXIntel64, "Mac", input);
    }
  }

  [MenuItem ("Assets/Build Selected AssetBundles For iOS")]
  public static void BuildSingleiPhone()
  {
    string[] input = ListSelectedScenes();
    if (input.Length > 0) {
      BuildAssetBundles(BuildTarget.iPhone, "iOS", input);
    }
  }

  [MenuItem ("Assets/Build Selected AssetBundles For Windows")]
  public static void BuildSingleWindows()
  {
    string[] input = ListSelectedScenes();
    if (input.Length > 0) {
      BuildAssetBundles(BuildTarget.StandaloneWindows, "Windows", input);
    }
  }
}
