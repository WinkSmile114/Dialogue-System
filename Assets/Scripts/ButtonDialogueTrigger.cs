using UnityEngine;

public class ButtonDialogueTrigger : MonoBehaviour
{
    [SerializeField] private string sceneId = "scene_intro";

    public void OnButtonPressed() {
        DialogueManager.Instance.StartDialogue(sceneId);
    }
}
