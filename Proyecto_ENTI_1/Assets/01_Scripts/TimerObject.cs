
using System.Collections;
using UnityEngine;

public class TimerObject
{
    bool isWaiting;
    MonoBehaviour owner;

    public TimerObject(MonoBehaviour owner ) 
    {  
        this.owner = owner;
        isWaiting = false;
    }
    public void StartTimer(float time)
    {
        owner.StartCoroutine(TimerCoroutine(time));
    }

    public bool IsTimerActive ( )
    {
        return isWaiting;
    }
    IEnumerator TimerCoroutine ( float time )
    {
        isWaiting = true;
        yield return new WaitForSeconds(time);
        isWaiting = false;
    }
}
