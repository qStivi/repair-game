using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///     Base Interface for Stats
/// </summary>
public interface IStat
{
    object GetValueAsObject();
}

/// <summary>
///     Interface für alle Objekte, die in Modifiern vorkommen können und somit Auswirkungen auf die
///     Berechung der Stat-Werte haben
/// </summary>
public interface IStatOperable<T>
{
    T AddAbsolute(T other);
    T ApplyPercent(float pct);
}

public struct FloatValue : IStatOperable<FloatValue>
{
    public float Value;

    public FloatValue AddAbsolute(FloatValue other)
    {
        return new FloatValue { Value = Value + other.Value };
    }

    public FloatValue ApplyPercent(float pct)
    {
        return new FloatValue { Value = Value * (1 + pct) };
    }

    // implizite Konvertierungen, damit du leichter mit float mischen kannst
//1.Zuweisung ohne Cast
//Du kannst jetzt in deinem Code schreiben:
//FloatValue fv = 3.5f;   // float wird automatisch zu FloatValue
//float f = fv;    // FloatValue wird automatisch zu float
//ohne jemals(FloatValue)3.5f oder(float)fv schreiben zu müssen.

//2.Automatische Typanpassung
//Wenn eine Methode void Foo(FloatValue v) erwartet, darfst du ihr problemlos einen float übergeben.
//Umgekehrt, wenn void Bar(float x) aufgerufen wird, kannst du einen FloatValue übergeben.

    public static implicit operator FloatValue(float f)
    {
        return new FloatValue { Value = f };
    }

    public static implicit operator float(FloatValue fv)
    {
        return fv.Value;
    }
}

public class Stat<T> : IStat where T : IStatOperable<T>
{
    private readonly T baseValue;
    private readonly IModifierTarget owner;
    private readonly int statId;

    //Caching mittels dirty-Flag 
    private T _chacheValue;
    private bool _dirty = true;

    public Stat(T value, IModifierTarget owner, int statId)
    {
        baseValue = value;
        _chacheValue = value;
        this.owner = owner;
        this.statId = statId;
    }

    object IStat.GetValueAsObject()
    {
        return GetValue();
    }

    public event Action OnStatChanged;

    private void Invalidate()
    {
        _dirty = true;
        OnStatChanged?.Invoke();
    }

   

        public T ComputeValue(ModifierSystem mods, float now)
        {
            
          

            var apps = mods.QueryForTargetAndStat(TargetId, statKey, now);
            return StatCalculator.Calculate(BaseHealth, apps);
        }
    

    public T GetValue()
    {
        if (_dirty)
        {
            var now = Time.time;
            modifiers.RemoveAll(m => m.IsExpired(now));

            //Absolute-Modifiers 
            var result = modifiers
                .Where(m => m.Type == ModificationType.Absolute)
                .Aggregate(baseValue, (acc, m) => acc.AddAbsolute(m.Value));

            //Additive Prozent
            var totalPct = pctModifiers
                .Where(m => m.Type == ModificationType.Relativ_Additive)
                .Sum(m => m.Value);
            result = result.ApplyPercent(totalPct);

            //// 3) (optional) Multiplikative Prozente analog
            //float mulFactor = modifiers
            //  .Where(m => m.Type == ModificationType.Relativ_Mulitplicative)
            //  .Aggregate(1f, (acc, m) => acc * (1 + m.Value));
            //result = result.ApplyPercent(mulFactor - 1f);
            _chacheValue = result;
            _dirty = false;
        }

        return _chacheValue;
    }
}