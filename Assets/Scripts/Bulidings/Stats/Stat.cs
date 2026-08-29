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
    T PctDefault { get; }
    T AddAbsolute(T other);
    T ApplyPercent(T pct);
}

public struct FloatValue : IStatOperable<FloatValue>
{
    public FloatValue PctDefault => new() { Value = 1f };
    public float Value;

    public FloatValue AddAbsolute(FloatValue other)
    {
        return new FloatValue { Value = Value + other.Value };
    }

    public FloatValue ApplyPercent(FloatValue pct)
    {
        return new FloatValue { Value = Value * (1 + pct.Value) };
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
    //INJECT MODSYSTEM IN ALL
    private readonly T baseValue;
    private readonly IModifierTarget owner;
    private readonly int statId;

    //Caching mittels dirty-Flag 
    private T _chacheValue;
    private long _currentCombinedVersion = 0;

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

    public T GetValue(ModifierSystem mods, float now)
    {
        var combinedVersion = mods.GetCombindedVersion(owner, statId);
        if (combinedVersion != _currentCombinedVersion)
        {
            var apps = mods.Query<T>(owner, statId, now);
            var result = StatCalculator.Calculate(baseValue, apps);
            _currentCombinedVersion = combinedVersion;
            _chacheValue = result;
        }

        return _chacheValue;

    }

}