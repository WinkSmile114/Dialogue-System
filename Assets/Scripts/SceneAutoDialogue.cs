using UnityEngine;

public class SceneAutoDialogue : MonoBehaviour
{
    [SerializeField] private string sceneId = "scene_intro";
    
    private void Start()
    {
        DialogueManager.Instance.StartDialogue(sceneId);
    }
}