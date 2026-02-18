using System;
using System.Collections.Generic;
using UnityEngine;

public interface IModifierService<TargetType>
{
    void Register(TargetType target);
    void Unregister(TargetType target);
    void AddModifier(IModificationInstance mod);
    void RemoveModifier(IModificationInstance mod);
}

public class ModifierService<TargetType> : MonoBehaviour, IModifierService<TargetType>
{
    private readonly List<IModificationInstance> _activeModifiers;
    private readonly List<TargetType> _registered;

    public void AddModifier(IModificationInstance mod)
    {
        /*   _activeModifiers.Add(mod);
           foreach (var target in _registered.Where())
           {

           }*/
    }

    public void Register(TargetType target)
    {
        _registered.Add(target);
        //Alle Buffs anwenden, die schon vorhandn sind 
    }

    public void RemoveModifier(IModificationInstance mod)
    {
        throw new NotImplementedException();
    }

    public void Unregister(TargetType target)
    {
        _registered.Remove(target);
    }

    protected virtual bool AppliesTo(TargetType target, IModificationInstance mod)
    {
        return mod.AppliesTo(target);
    }
}