using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{
  World world;

  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    DrawSettingsEditor(world.noiseSettings, world.DebugRegenerate, ref world.noiseSettingsFoldout);
  }

  private void DrawSettingsEditor(Object settings, System.Action action, ref bool foldout)
  {
    using (var check = new EditorGUI.ChangeCheckScope())
    {
      foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

      if (foldout)
      {
        Editor editor = CreateEditor(settings);
        editor.OnInspectorGUI();

        if (check.changed && action != null)
        {
          action();
        }
      }
    }
  }

  private void OnEnable()
  {
    world = (World)target;
  }
}