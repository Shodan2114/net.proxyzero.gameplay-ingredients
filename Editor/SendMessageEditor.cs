using UnityEngine;
using UnityEditor;
namespace ProxyBasics
{
    public class SendMessageEditor : EditorWindow
    {
        private float space = 20f;
        private string stringToSend = "";
        [MenuItem("Proxy Basic/Send Message")]
        public static void CustomEdiorWindow()
        {
            GetWindow<SendMessageEditor>("Send Message");
        }

        private void OnGUI() {

            GUILayout.Label("State Machin Test", EditorStyles.largeLabel);
            GUILayout.Space(space);
            stringToSend = EditorGUILayout.TextField("String to send", stringToSend);
            if(GUILayout.Button("TEST"))
            {
                Messager.Send(stringToSend);
            }

        }

        private void GameObjectRename(GameObject[] objects)
        {
            foreach (var item in objects)
            {
                item.name = stringToSend;
            }
        }
    }
}

