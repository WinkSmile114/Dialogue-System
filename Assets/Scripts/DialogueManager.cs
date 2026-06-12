using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Singleton Setup
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // References
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private DialogueUI dialogueUI;

    // STATE
    private DialogueScene currentScene;
    private int currentLineIndex = 0;
    private bool isShowingChoices = false;
    public bool IsDialogueActive { get; private set; } = false;

    // Public Methods
    public void StartDialogue(string sceneId)
    {
        string path = "DialogueScenes/" + sceneId;

        TextAsset jsonFile = Resources.Load<TextAsset>(path);

        if (jsonFile == null)
        {
            Debug.LogError("[DialogueManger] Could not find dialogue file at: Resources/" + path + ".json");
            return;
        }

        currentScene = JsonUtility.FromJson<DialogueScene>(jsonFile.text);

        if (currentScene == null)
        {
            Debug.LogError("[DialogueManger] Failed to parse JSON for scene: " + sceneId);
            return;
        }

        currentLineIndex = 0;
        isShowingChoices = false;
        IsDialogueActive = true;

        dialogueUI.ShowDialoguePanel(true);
        dialogueUI.ShowChoicePanel(false);

        DisplayCurrentLine();
    }

    public void OnPlayerClick()
    {
        if (isShowingChoices) return;
        if (!IsDialogueActive) return;

        currentLineIndex++;
        
        if (currentLineIndex >= currentScene.lines.Count)
        {
            ShowChoices();
        }
        else{
            DisplayCurrentLine();
        }
    }

    public void HandleChoiceSelected(DialogueChoice choice)
    {
        foreach (DialogueEffect effect in choice.effects)
        {
            ApplyEffect(effect);
        }

        if (!string.IsNullOrEmpty(choice.nextScene))
        {
            StartDialogue(choice.nextScene);
        }
        else
        {
            EndDialogue();
        }        
    }

    // Private Methods
    private void DisplayCurrentLine()
    {
        DialogueLine line = currentScene.lines[currentLineIndex];
        Sprite portrait = LoadPortrait(line.portrait);
        dialogueUI.DisplayLine(line.characterName, portrait, line.text);
    }

    private void ShowChoices()
    {
         isShowingChoices = true;

        // Build a list of choices that pass their conditions.
        List<DialogueChoice> validChoices = new List<DialogueChoice>();
    
        foreach (DialogueChoice choice in currentScene.choices)
        {
            if (CheckConditions(choice.conditions))
            {
                validChoices.Add(choice);
            }
        }

        if (validChoices.Count == 0)
        {
            Debug.LogWarning("[DialogueManager] No valid choices in scene: " + currentScene.sceneId + ".Ending dialogue.");
            EndDialogue();
            return;
        }

        dialogueUI.ShowChoices(validChoices);
        dialogueUI.ShowChoicePanel(true);
    }

    private bool CheckConditions(List<DialogueCondition> conditions)
    {
        foreach (DialogueCondition condition in conditions)
        {
            if (!CheckSingleCondition(condition))
            {
                // One condition failed — the whole set fails.
                return false;
            }
        }
        return true;
    }

    private bool CheckSingleCondition(DialogueCondition condition)
    {
        // Get the current value of the stat named in condition.stat
        int statValue = GetStatValue(condition.stat);

        // Evaluate based on the comparison operator.
        switch (condition.comparison)
        {
            case "greaterThan":
                return statValue > condition.valueA;
            case "lessThan":
                return statValue < condition.valueA;
            case "greaterThanOrEqual":
                return statValue >= condition.valueA;
            case "lessThanOrEqual":
                return statValue <= condition.valueA;
            case "equalTo":
                return statValue == condition.valueA;
            case "between":
                // True if statValue is >= valueA AND <= valueB
                return statValue >= condition.valueA && statValue <= condition.valueB;
            default:
                Debug.LogWarning("[DialogueManager] Unknown comparison operator: " + condition.comparison);
                return false;
        }
    }

    private int GetStatValue(string statName)
    {
        switch (statName)
        {
            // ── Country Stats ────────────────────────────────────────────────
            case "EconomyAlignment":        return playerStats.EconomyAlignment;
            case "EconomyHealth":           return playerStats.EconomyHealth;
            case "EconomySize":             return playerStats.EconomySize;
            case "SocialStats":             return playerStats.SocialStats;
            case "TradeVolume":             return playerStats.TradeVolume;

            // ── Political Stats ──────────────────────────────────────────────
            case "PartyRelation":           return playerStats.PartyRelation;
            case "LeftRelation":            return playerStats.LeftRelation;
            case "RightRelation":           return playerStats.RightRelation;
            case "CommunistRelation":       return playerStats.CommunistRelation;
            case "AltRightRelation":        return playerStats.AltRightRelation;

            // ── Public Stats ─────────────────────────────────────────────────
            case "LeftSupport":             return playerStats.LeftSupport;
            case "RightSupport":            return playerStats.RightSupport;
            case "SasanSupport":            return playerStats.SasanSupport;
            case "AsterlundieSupport":      return playerStats.AsterlundieSupport;
            case "RegionalSupport":         return playerStats.RegionalSupport;
            case "CitySupport":             return playerStats.CitySupport;
            case "PoorSupport":             return playerStats.PoorSupport;
            case "RichSupport":             return playerStats.RichSupport;

            // ── International Relation Stats ─────────────────────────────────
            case "WelticaRelation":         return playerStats.WelticaRelation;
            case "DiestlundRelation":       return playerStats.DiestlundRelation;
            case "AsterlundRelation":       return playerStats.AsterlundRelation;
            case "CriviaRelation":          return playerStats.CriviaRelation;
            case "ValmorRelation":          return playerStats.ValmorRelation;
            case "ErebraRelation":          return playerStats.ErebraRelation;
            case "SasanRelation":           return playerStats.SasanRelation;
            case "KarsovaRelation":         return playerStats.KarsovaRelation;
            case "AmariaRelation":          return playerStats.AmariaRelation;

            // ── Character Relations ──────────────────────────────────────────
            case "Family":                  return playerStats.Family;

            default:
                Debug.LogWarning("[DialogueManager] Unknown stat name in GetStatValue: " + statName);
                return 0;
        }
    }

    private void ApplyEffect(DialogueEffect effect)
    {
        switch (effect.stat)
        {
            // ── Country Stats ────────────────────────────────────────────────
            case "EconomyAlignment":
                playerStats.EconomyAlignment = Clamp(playerStats.EconomyAlignment + effect.value);
                break;
            case "EconomyHealth":
                playerStats.EconomyHealth = Clamp(playerStats.EconomyHealth + effect.value);
                break;
            case "EconomySize":
                playerStats.EconomySize = Clamp(playerStats.EconomySize + effect.value);
                break;
            case "SocialStats":
                playerStats.SocialStats = Clamp(playerStats.SocialStats + effect.value);
                break;
            case "TradeVolume":
                playerStats.TradeVolume = Clamp(playerStats.TradeVolume + effect.value);
                break;

            // ── Political Stats ──────────────────────────────────────────────
            case "PartyRelation":
                playerStats.PartyRelation = Clamp(playerStats.PartyRelation + effect.value);
                break;
            case "LeftRelation":
                playerStats.LeftRelation = Clamp(playerStats.LeftRelation + effect.value);
                break;
            case "RightRelation":
                playerStats.RightRelation = Clamp(playerStats.RightRelation + effect.value);
                break;
            case "CommunistRelation":
                playerStats.CommunistRelation = Clamp(playerStats.CommunistRelation + effect.value);
                break;
            case "AltRightRelation":
                playerStats.AltRightRelation = Clamp(playerStats.AltRightRelation + effect.value);
                break;

            // ── Public Stats ─────────────────────────────────────────────────
            case "LeftSupport":
                playerStats.LeftSupport = Clamp(playerStats.LeftSupport + effect.value);
                break;
            case "RightSupport":
                playerStats.RightSupport = Clamp(playerStats.RightSupport + effect.value);
                break;
            case "SasanSupport":
                playerStats.SasanSupport = Clamp(playerStats.SasanSupport + effect.value);
                break;
            case "AsterlundieSupport":
                playerStats.AsterlundieSupport = Clamp(playerStats.AsterlundieSupport + effect.value);
                break;
            case "RegionalSupport":
                playerStats.RegionalSupport = Clamp(playerStats.RegionalSupport + effect.value);
                break;
            case "CitySupport":
                playerStats.CitySupport = Clamp(playerStats.CitySupport + effect.value);
                break;
            case "PoorSupport":
                playerStats.PoorSupport = Clamp(playerStats.PoorSupport + effect.value);
                break;
            case "RichSupport":
                playerStats.RichSupport = Clamp(playerStats.RichSupport + effect.value);
                break;

            // ── International Relation Stats ─────────────────────────────────
            case "WelticaRelation":
                playerStats.WelticaRelation = Clamp(playerStats.WelticaRelation + effect.value);
                break;
            case "DiestlundRelation":
                playerStats.DiestlundRelation = Clamp(playerStats.DiestlundRelation + effect.value);
                break;
            case "AsterlundRelation":
                playerStats.AsterlundRelation = Clamp(playerStats.AsterlundRelation + effect.value);
                break;
            case "CriviaRelation":
                playerStats.CriviaRelation = Clamp(playerStats.CriviaRelation + effect.value);
                break;
            case "ValmorRelation":
                playerStats.ValmorRelation = Clamp(playerStats.ValmorRelation + effect.value);
                break;
            case "ErebraRelation":
                playerStats.ErebraRelation = Clamp(playerStats.ErebraRelation + effect.value);
                break;
            case "SasanRelation":
                playerStats.SasanRelation = Clamp(playerStats.SasanRelation + effect.value);
                break;
            case "KarsovaRelation":
                playerStats.KarsovaRelation = Clamp(playerStats.KarsovaRelation + effect.value);
                break;
            case "AmariaRelation":
                playerStats.AmariaRelation = Clamp(playerStats.AmariaRelation + effect.value);
                break;

            // ── Character Relations ──────────────────────────────────────────
            case "Family":
                playerStats.Family = Clamp(playerStats.Family + effect.value);
                break;

            default:
                Debug.LogWarning("[DialogueManager] Unknown stat name in ApplyEffect: " + effect.stat);
                break;
        }
    }

    private int Clamp(int value)
    {
        return Mathf.Clamp(value, -100, 100);
    }

    private Sprite LoadPortrait(string portraitName)
    {
        if (string.IsNullOrEmpty(portraitName))
        {
            return null;
        }

        // Resources.Load looks inside Assets/Resources/
        Sprite portrait = Resources.Load<Sprite>("Portraits/" + portraitName);

        if (portrait == null)
        {
            // Portrait not found — warn but don't crash. The UI will hide the image.
            Debug.LogWarning("[DialogueManager] Portrait not found: Resources/Portraits/" + portraitName);
        }

        return portrait;
    }

    private void EndDialogue()
    {
        IsDialogueActive = false;
        isShowingChoices = false;
        currentScene = null;
        dialogueUI.ShowDialoguePanel(false);
        dialogueUI.ShowChoicePanel(false);
    }
}