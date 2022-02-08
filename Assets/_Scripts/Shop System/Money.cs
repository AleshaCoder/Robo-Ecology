using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Money
{
    public static event UnityAction<int> CountChanged;

    public static int Count { get; private set; }

    public static void AddMoney(int count)
    {
        Count += count;
        CountChanged?.Invoke(Count);
    }

    public static bool TryGetMoney(int count)
    {
        bool enoughMoney = Count >= count;
        if (enoughMoney)
        {
            Count -= count;
            CountChanged?.Invoke(Count);
        }
        return enoughMoney;
    }
}
