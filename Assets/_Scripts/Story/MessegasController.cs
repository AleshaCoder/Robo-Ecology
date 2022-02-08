using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessegasController : MonoBehaviour
{
    [SerializeField] private StoryManager StoryManager;
    [SerializeField] private Messege _messege;
    [SerializeField] private AudioSource _audioSource;

    private IEnumerator _hidingMessageEnumerator;

    private void ShowMessage(Story story)
    {
        _messege.Show(story.Text);
        if (story.AudioClip != null)
        {
            _audioSource.clip = story.AudioClip;
            _audioSource.Play();
        }
        if (story.AutoEnd == false)
            return;
        if (story.AudioClip == null)
            _hidingMessageEnumerator = HideMessage();
        else
            _hidingMessageEnumerator = HideMessageWithAudio();
        StartCoroutine(_hidingMessageEnumerator);
    }

    private IEnumerator HideMessageWithAudio()
    {
        yield return new WaitWhile(() => _audioSource.isPlaying == true);
        _messege.Hide();
        StoryManager.CheckNextStory();
    }

    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(4);
        _messege.Hide();
        StoryManager.CheckNextStory();
    }

    public void HideMessageImmediatly()
    {
        if (_hidingMessageEnumerator != null)
            StopCoroutine(_hidingMessageEnumerator);
        _messege.Hide();
        StoryManager.CheckNextStory();
    }

    private void OnEnable()
    {
        StoryManager.CurrentStoryChanged += ShowMessage;
    }

    private void OnDisable()
    {
        StoryManager.CurrentStoryChanged -= ShowMessage;
    }
}
