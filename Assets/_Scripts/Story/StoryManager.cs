using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Story
{
    [SerializeField] private string _name;
    [SerializeField] private string _text;
    [SerializeField] private string _nextStoryName = "";
    [SerializeField] private AudioClip _audioClip = null;
    [SerializeField] private bool _autoEnd = true;

    public string Name => _name;
    public string Text => _text;
    public string NextStoryName => _nextStoryName;
    public AudioClip AudioClip => _audioClip;

    public bool AutoEnd => _autoEnd;
}

public class StoryManager : MonoBehaviour
{
    [SerializeField] private List<Story> _stories = new List<Story>();

    private static Story _currentStory;

    public delegate void StoryChanged(Story story);
    public static event StoryChanged CurrentStoryChanged;

    private bool HasInList(string name, ref Story story)
    {
        foreach (var item in _stories)
        {
            if (item.Name == name)
            {
                story = item;
                return true;
            }
        }
        return false;
    }

    public void CheckNextStory()
    {
        string nextStoryName = _currentStory.NextStoryName;
        if (nextStoryName != "")
        {
            TrySetCurrentStory(nextStoryName);
        }
    }

    public Story GetCurrentStory()
    {
        if (_currentStory == null)
            Debug.LogError("Current story is null");
        return _currentStory;
    }

    public void TrySetCurrentStory(string name)
    {
        Story story = null;
        if (HasInList(name, ref story))
        {
            _currentStory = story;
            CurrentStoryChanged?.Invoke(story);
            return;
        }
        Debug.LogError($"Story with name {name} is not found");
    }
}
