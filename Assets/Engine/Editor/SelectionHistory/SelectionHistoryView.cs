#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace ExplodingElves.Editor
{
    [InitializeOnLoad]
    public class SelectionHistoryView
    {
        static SelectionHistoryView()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            GUI.enabled = SelectionHistory.HasPrevious;
            if (GUILayout.Button(new GUIContent("<", "Previous Selection")))
            {
                SelectionHistory.SelectPrevious();
            }

            GUI.enabled = SelectionHistory.HasNext;
            if (GUILayout.Button(new GUIContent(">", "Next Selection")))
            {
                SelectionHistory.SelectNext();
            }
        }
    }
}
#endif
