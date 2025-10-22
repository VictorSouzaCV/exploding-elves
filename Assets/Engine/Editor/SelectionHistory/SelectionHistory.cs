#if UNITY_EDITOR
using UnityEditor;

namespace ExplodingElves.Editor
{
    [InitializeOnLoad]
    static class SelectionHistory
    {
        public static bool HasPrevious => selections.HasPrevious;
        public static bool HasNext => selections.HasNext;
        const int maxHistorySize = 50; 
        static bool ignoreNextSelectionChangedEvent;

        static readonly SelectionList selections;
        
        static SelectionHistory()
        {
            selections = new SelectionList(maxHistorySize);
            Selection.selectionChanged += HandleSelectionChanged;
        }

        static void HandleSelectionChanged()
        {
            if (ignoreNextSelectionChangedEvent)
            {
                ignoreNextSelectionChangedEvent = false;
                return;
            }
            
            selections.Insert(Selection.activeObject);
        }

        [MenuItem("Edit/Selection/Select Previous &,")]
        public static void SelectPrevious()
        {
            var previous = selections.SelectPrevious();
            if (previous == null)
            {
                return;
            }

            Selection.activeObject = previous;
            ignoreNextSelectionChangedEvent = true;
        }
        
        [MenuItem("Edit/Selection/Select Next &.")]
        public static void SelectNext()
        {
            var next = selections.SelectNext();
            if (next == null)
            {
                return;
            }

            Selection.activeObject = next;
            ignoreNextSelectionChangedEvent = true;
        }
    }
}
#endif
