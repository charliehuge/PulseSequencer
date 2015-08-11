using UnityEngine;
using UnityEditor;

namespace DerelictComputer
{
    public static class PulseEditorHelpers
    {
        [MenuItem("GameObject/Pulse/Pulse", false, 10)]
        public static void CreatePulse(MenuCommand menuCommand)
        {
            var pulse = new GameObject("Pulse", typeof(Pulse));
            GameObjectUtility.SetParentAndAlign(pulse, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(pulse, "Create " + pulse.name);
            Selection.activeGameObject = pulse;
        }
	
        [MenuItem("GameObject/Pulse/Pattern", false, 10)]
        public static void CreatePattern(MenuCommand menuCommand)
        {
            var pattern = new GameObject("Pattern", typeof(Pattern));
            GameObjectUtility.SetParentAndAlign(pattern, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(pattern, "Create " + pattern.name);
            Selection.activeGameObject = pattern;
        }
    }
}