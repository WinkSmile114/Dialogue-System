using UnityEngine;

public class DialogueTriggerZone : MonoBehaviour
{
    [SerializeField] private string sceneId = "scene_intro";
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            DialogueManager.Instance.StartDialogue(sceneId);
            hasTriggered = true;
        }
    }
}