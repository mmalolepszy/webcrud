using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebCRUD.vNext.Infrastructure.Exception;

namespace WebCRUD.vNext.Infrastructure.ExtensionMethods
{
    /// <summary>
    /// Contains helper methods for enumerations.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Creates <see cref="SelectListItem"/> from each enumeration value.
        /// </summary>
        /// <typeparam name="TEnum">type of the enumeration</typeparam>
        /// <returns>SelectListItem for each corresponding enumeration value</returns>
        public static IEnumerable<SelectListItem> EnumToSelectListItems<TEnum>() where TEnum : struct
        {
            return from TEnum e in Enum.GetValues(typeof(TEnum))
                   select new SelectListItem { Value = e.ToString(), Text = e.TranslateEnum() };
        }

        /// <summary>
        /// Creates <see cref="SelectListItem"/> from each enumeration value and inserts empty element at the begining.
        /// </summary>
        /// <typeparam name="TEnum">type of the enumeration</typeparam>
        /// <returns>SelectListItem for each corresponding enumeration value and empty item at the beginning</returns>
        public static IEnumerable<SelectListItem> EnumToSelectListItemsWithEmptyElement<TEnum>() where TEnum : struct
        {
            yield return new SelectListItem();
            foreach (var item in EnumToSelectListItems<TEnum>())
                yield return item;
        }

        /// <summary>
        /// Parses string to enumeration value.
        /// </summary>
        /// <typeparam name="TEnum">type of the enumeration</typeparam>
        /// <param name="stringValue">string to parse</param>
        /// <returns>parsed enum</returns>
        /// <exception cref="TechnicalException">Throws TechnicalException when string value cannot be parsed.</exception>
        public static TEnum ToEnumValue<TEnum>(this string stringValue) where TEnum : struct
        {
            TEnum result;
            if (!Enum.TryParse(stringValue, out result))
                throw new TechnicalException(string.Format("Unable to find member {0} in enum type {1}", stringValue, typeof(TEnum).Name));
            return result;
        }
    }
}