using System.Collections.Generic;
using System.Linq;

public static class StatCalculator
{
    public static T Calculate<T>(T baseValue, IEnumerable<ModifierApplication<T>> modifierApplications) where T : IStatOperable<T>
    {

        var result = baseValue;
        T totalPct = .PctDefault;

        foreach (var app in modifierApplications){
            switch (app.Prototype.ModificationType)
            {
                case ModificationType.Absolute:
                    result = result.AddAbsolute(app.Value);
                    break;
                case ModificationType.Relativ_Additive:
                    totalPct = totalPct.AddAbsolute(app.Value);
                    break;
                default:
                    throw new System.NotImplementedException($"ModificationType {app.Prototype.ModificationType} not implemented.");
            }
        }
          
            result = result.ApplyPercent(totalPct);

            return result;
    }
}