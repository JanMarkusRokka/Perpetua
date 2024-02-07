using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Inspired by Jim Mischel https://stackoverflow.com/questions/4675535/how-do-i-add-an-item-to-the-front-of-the-queue
public class BattleActionQueue<BattleAction>
{
    private Queue<BattleAction> GuardQueue = new();
    private Queue<BattleAction> OtherActionQueue = new();

    public void Enqueue(BattleAction action) 
    {
        if (action.GetType() == typeof(Guard))
            GuardQueue.Enqueue(action);
        else
            OtherActionQueue.Enqueue(action);
    }

    public BattleAction Dequeue()
    {
        if (GuardQueue.Count > 0)
            return GuardQueue.Dequeue();
        else
            return OtherActionQueue.Dequeue();
    }

    public int Count()
    {
        int count = 0;
        count += GuardQueue.Count;
        count += OtherActionQueue.Count;
        return count;
    }
}
