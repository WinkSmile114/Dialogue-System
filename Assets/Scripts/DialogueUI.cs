using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("Dialogue Panel")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Choice Panel")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Transform choiceButtonContainer;

    // Private State
    private List<GameObject> activeChoiceButtons = new List<GameObject>();

    // Public Methods
    public void ShowDialoguePanel(bool visible)
    {
        dialoguePanel.SetActive(visible);
    }

    public void ShowChoicePanel(bool visible)
    {
        choicePanel.SetActive(visible);
    }

    public void DisplayLine(string characterName, Sprite portrait, string text)
    {
        nameText.text = characterName;
        dialogueText.text = text;
        if (portrait != null)
        {
            portraitImage.sprite = portrait;
            portraitImage.gameObject.SetActive(true);
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
        }
    }

    public void ShowChoices(List<DialogueChoice> choices)
    {
        ClearChoiceButtons();
        foreach (var choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = choice.text;
            }
            DialogueChoice capturedChoice = choice; // Capture the current choice for the lambda
            button.onClick.AddListener(() => 
            {
                DialogueManager.Instance.HandleChoiceSelected(capturedChoice);
            });
            activeChoiceButtons.Add(buttonObj);
        }
    }

    // Private Methods
    private void ClearChoiceButtons()
    {
        foreach (GameObject btn in activeChoiceButtons)
        {
            Destroy(btn);
        }
        activeChoiceButtons.Clear();
    }
}