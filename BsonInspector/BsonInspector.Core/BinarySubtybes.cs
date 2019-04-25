using System;
using System.ComponentModel.DataAnnotations;

namespace BsonInspector.Core
{
    public enum BinarySubtybes
    {
        [Display(Name="Generic")]
        Generic, //0x00
        [Display(Name = "Function")]
        Function, //0x01
        [Obsolete]
        [Display(Name = "Binary (deprecated)")]
        Binary, //0x02
        [Obsolete]
        [Display(Name = "UUID (deprecated)")]
        UUIDold, //0x03
        [Display(Name = "UUID")]
        UUID, //0x04
        [Display(Name = "Md5")]
        Md5, //0x05
        [Display(Name = "User defined")]
        UserDefined //0x80
    }
}
