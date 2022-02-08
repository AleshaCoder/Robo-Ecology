using TMPro;
using UnityEngine;

public abstract class ResourseView : MonoBehaviour
{
    [SerializeField] protected TMP_Text _info;

    protected virtual void UpdateInfo(int count)
    {
        if (count < 1000)
        {
            _info.text = count.ToString();
        }
        else if (count < 1000000)
        {
            _info.text = (count / 1000).ToString() + "," + (count % 1000 / 100).ToString() + "k";
        }
        else if (count > 1000000)
        {
            _info.text = (count / 1000000).ToString() + "," + (count % 1000000 / 100000).ToString() + "m";
        }
    }

    protected abstract void SubscribeToEvent();

    protected abstract void UnsubscribeFromEvent();

    private void OnEnable()
    {
        SubscribeToEvent();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvent();
    }
}
