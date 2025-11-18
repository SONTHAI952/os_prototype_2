using System;
using DG.Tweening;

namespace ZeroX.Extensions
{
    public static class Tween_Extension
    {
        public static TTween OnCompleteOrKill<TTween>(this TTween tween, Action callBack) where TTween : Tween
        {
            bool completed = false;
            bool killed = false;
            
            
            tween.OnComplete(() =>
            {
                if(killed)
                    return;
                
                completed = true;
                callBack?.Invoke();
            });

            
            tween.OnKill(() =>
            {
                if (completed)
                    return;

                killed = true;
                callBack?.Invoke();
            });

            
            return tween;
        }
    }
}