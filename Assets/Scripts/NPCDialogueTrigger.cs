using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private string sceneId = "scene_minister_aldren";
    
    private void OnMouseDown()
    {
        DialogueManager.Instance.StartDialogue(sceneId);
    }
}