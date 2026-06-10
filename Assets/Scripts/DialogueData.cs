using System;
using System.Collections.Generic;

[Serializable]
public class DialogueScene
{
    public string sceneId;
    public List<DialogueLine> lines = new List<DialogueLine>();
    public List<DialogueChoice> choices = new List<DialogueChoice>();
}

[Serializable]
public class DialogueLine
{
    public string characterName;
    public string portrait;
    public string text;
}

[Serializable]
public class DialogueChoice
{
    public string text;
    public List<DialogueCondition> conditions = new List<DialogueCondition>();
    public List<DialogueEffect> effects = new List<DialogueEffect>();
    public string nextScene;
}

[Serializable]
public class DialogueCondition
{
    public string stat;
    public string comparison;
    public int valueA;
    public int valueB;
}

[Serializable]
public class DialogueEffect
{
    public string stat;
    public int value;
}

[Serializable]
public class DialogueSceneWrapper
{
    public List<DialogueScene> scenes = new List<DialogueScene>();
}

