using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;

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
        Association = 3,

        [Display(Name = "مؤوسسة")]
        Foundation = 4
    }

    public enum Gender
    {
       

        [Display(Name = "ذكر")]
        Male = 1,

        [Display(Name = "انثى")]
        Female = 2,

        
    }


    public enum Age
    {
        [Display(Name = "-- تحديد العمر --")]
        Message = 0,

        [Display(Name = "اقل من 20 سنة")]
        TwentyYounger = 1,

        [Display(Name = " 20 سنة إلى 30 سنة")]
        OverTwenty = 2,

        [Display(Name = "30 سنة إلى 40 سنة")]
        OverThirty = 3,

        [Display(Name = "أكبر من 40 سنة")]
        OverForty = 4
    }


    //public const string Check = "شيك";
    //public const string Cash = "نقد";
    //public const string Account = "حساب";
    //public const string Bank = "بنك";

    public enum PaymentTypes
    {
        [Display(Name = "-- تحديد نوع الدفع --")]
        Message = 0,

        [Display(Name = "شيك")]
        Check = 1,

        [Display(Name = "نقد")]
        Cash = 2,

        [Display(Name = "حساب")]
        Account = 3,

        [Display(Name = "بنك")]
        Bank = 4
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
