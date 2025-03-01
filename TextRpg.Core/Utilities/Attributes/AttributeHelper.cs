using System.Reflection;

namespace TextRpg.Core.Utilities.Attributes
{
    public static class AttributeHelper
    {
        public static string GetDescription(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Text ?? "No description available.";
        }

        public static object GetDefaultValue(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<DefaultValueAttribute>();

            if (attribute != null)
            {
                if (property.PropertyType == typeof(DateTime) && attribute.Value is string str && str == "NOW")
                {
                    return DateTime.Now;
                }

                return attribute.Value;
            }

            var type = property.PropertyType;
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Cannot create default instance for {type}");
            }

            return string.Empty;
        }
    }
}
