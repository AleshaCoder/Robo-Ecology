using UnityEngine;

public class RubbishView : ResourseView
{
    [SerializeField] private RubbishMagnet _magnet;
    protected override void SubscribeToEvent()
    {
        _magnet.RubbishCountChanged += UpdateInfo;
    }

    protected override void UnsubscribeFromEvent()
    {
        _magnet.RubbishCountChanged -= UpdateInfo;
    }
}
