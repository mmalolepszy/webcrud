using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebCRUD.vNext.Infrastructure.Exception;

namespace WebCRUD.vNext.Infrastructure.ExtensionMethods
{
    /// <summary>
    /// Contains extension methods for enum
    /// </summary>
    public static class EnumExtensionMethods
    {
        /// <summary>
        /// Creates a <see cref="SelectList"/> from all enumeration values.
        /// </summary>
        /// <typeparam name="TEnum">type of the enumeration</typeparam>
        /// <param name="enumValue">enumeration value</param>
        /// <returns>SelectList for the enumeration type</returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                         select new { Id = e, Name = e.TranslateEnum() };

            return new SelectList(values, "Id", "Name", enumValue);
        }

        /// <summary>
        /// Translates the enum value according to the <see cref="DisplayAttribute"/> settings.
        /// </summary>
        /// <typeparam name="TEnum">type of the enumeration</typeparam>
        /// <param name="enumValue">enumeration value to translate</param>
        /// <returns>translated value</returns>
        public static string TranslateEnum<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            var stringValue = enumValue.ToString();
            var enumType = typeof (TEnum);
            
            var attribute = GetDisplayAttribute(enumType, enumValue);
            if (attribute == null)
                return stringValue;
            if (attribute.ResourceType == null)
                return attribute.Name;

            var resourceManager = new ResourceManager(attribute.ResourceType);
            return resourceManager.GetString(attribute.Name);
        }

        private static DisplayAttribute GetDisplayAttribute(Type enumType, object enumValue)
        {
            var stringValue = enumValue.ToString();
            var enumMembers = enumType.GetMember(enumValue.ToString());
            if (enumMembers == null || enumMembers.Count() != 1)
                throw new TechnicalException(string.Format("Unable to find member {0} in enum type {1}", stringValue, enumType.Name));

            var displayAttributes = enumMembers[0].GetCustomAttributes(typeof (DisplayAttribute), false);
            if (!displayAttributes.Any())
                return null;

            return (DisplayAttribute) displayAttributes.First();
        }
    }
}