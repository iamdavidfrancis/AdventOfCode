using System.ComponentModel;

namespace AdventOfCode.Helpers;

public static class TConverter
{
    public static T ChangeType<T>(this object value)
    {
        return (T)value.ChangeType(typeof(T));
    }

    public static object ChangeType(this object value, Type t)
    {
        if (value.GetType() == t)
        {
            return value;
        }

        TypeConverter tc = TypeDescriptor.GetConverter(t) ?? throw new InvalidOperationException();
        return tc.ConvertFrom(value) ?? throw new InvalidOperationException();
    }

    public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
    {
        TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
    }
}
