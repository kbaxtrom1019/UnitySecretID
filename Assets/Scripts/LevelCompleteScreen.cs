using UnityEngine;
using System.Collections;

public class LevelCompleteScreen : BaseMenu
{
    public delegate void OnLevelCompleteAnimationDone();

    private OnLevelCompleteAnimationDone callback;

    public void SetAnimDoneCallback(OnLevelCompleteAnimationDone func)
    {
        callback = func;
    }

    public void AnimationCompleted()
    {
        if(callback != null)
        {
            callback();
        }
    }
}
