using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ReceiptVoucher.Core.Enums
{
    public enum GrantDest
    {
        [Display(Name = "-- اختيار الجهه المانحه --")]
        Message = 0,

        [Display(Name = "فرد")]
        Individual = 1,

        [Display(Name = "شركة")]
        Company = 2,

        [Display(Name = "جمعية")]
        Association = 3
    }

    public enum Gender
    {
       

        [Display(Name = "ذكر")]
        Male = 1,

        [Display(Name = "انثى")]
        Female = 2,

        
    }

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DisplayAttribute>();
            return attribute == null ? value.ToString() : attribute.Name;
        }
    }
}
