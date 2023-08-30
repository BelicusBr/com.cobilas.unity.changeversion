using UnityEditor;
using UnityEngine;

namespace Packages.com.cobilas.unity.changeversion.Test.Editor {
    public class ChangeVersionWinTest : EditorWindow {

        [MenuItem("Window/Change version test")]
        private static void Init() {
            ChangeVersionWinTest test = GetWindow<ChangeVersionWinTest>();
            test.Show();
        }
    }
}