using System.Linq;

public static class DmgTypeExtensions
{
    public static DmgTypeAttribute GetAttribute(this DmgType dmgType)
    {
        var type = typeof(DmgType);
        var memberInfo = type.GetMember(dmgType.ToString());
        if (memberInfo.Length == 0) return null;
        var attribute =
            memberInfo[0].GetCustomAttributes(typeof(DmgTypeAttribute), false).FirstOrDefault() as
                DmgTypeAttribute;
        return attribute;
    }
}