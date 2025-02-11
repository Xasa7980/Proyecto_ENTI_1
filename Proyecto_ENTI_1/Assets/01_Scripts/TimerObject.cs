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
public class AnimationObject
{
    const string ATTACK_ANIMATOR_TAG = "Attack";
    const string CHARGED_ATTACK_ANIMATOR_TAG = "ChargedAttack";
    const string MOVE_ANIMATOR_TAG = "Movement";

    Animator anim;
    public AnimationObject (Animator anim)
    {
        this.anim = anim;
    }
}
