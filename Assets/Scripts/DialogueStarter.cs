using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Press space to start the intro dialogue scene.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DialogueManager.Instance.StartDialogue("scene_intro");
        }

        // Left mouse click advances the dialogue
        if (Input.GetMouseButtonDown(0))
        {
            DialogueManager.Instance.OnPlayerClick();
        }
    }
}
