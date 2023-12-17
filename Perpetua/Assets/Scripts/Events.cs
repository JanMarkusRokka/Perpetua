using System.Collections;
using System.Collections.Generic;
using System;

public static class Events
{
    public static event Action<ItemData> OnItemReceived;
    public static void ReceiveItem(ItemData data) => OnItemReceived?.Invoke(data);
}
