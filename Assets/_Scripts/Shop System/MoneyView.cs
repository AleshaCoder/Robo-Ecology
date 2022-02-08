using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyView : ResourseView
{
    protected override void SubscribeToEvent()
    {
        Money.CountChanged += UpdateInfo;
    }

    protected override void UnsubscribeFromEvent()
    {
        Money.CountChanged -= UpdateInfo;
    }
}
