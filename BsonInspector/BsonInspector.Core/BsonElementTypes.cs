using System;
using System.ComponentModel.DataAnnotations;

namespace BsonInspector.Core
{
    public enum BsonElementTypes
    {
        [Display(Name = "Double")]
        bDouble, //x01
        [Display(Name = "String")]
        bString, //x02
        [Display(Name = "Document")]
        bEmbeddedDocument, //x03
        [Display(Name = "Array")]
        bDocumentArray, //x04
        [Display(Name = "Binary")]
        bBinary, //x05
        [Obsolete]
        [Display(Name = "Undefined (deprecated)")]
        bUndefined, //x06
        [Display(Name = "Object id")]
        bObjectId, //x07
        [Display(Name = "Boolean")]
        bBoolean, //x08
        [Display(Name = "UTC datetime")]
        bUTCDateTime, //x09
        [Display(Name = "Null value")]
        bNull, //x0A
        [Display(Name = "Regular expression")]
        bRegularExpression, //x0B
        [Obsolete]
        [Display(Name = "DBPointer (deprecated)")]
        bDbPointer, //x0C
        [Display(Name = "JavaScript code")]
        bJavascriptCode, //x0D
        [Obsolete]
        [Display(Name = "Symbol (deprecated)")]
        bSymbol, //x0E
        [Display(Name = "JavaScript code w/ scope")]
        bJavascriptCodeScoped, //x0F
        [Display(Name = "Integer 32-bit")]
        bInt32, //x10
        [Display(Name = "Timestamp")]
        bTimeStamp,//x11
        [Display(Name = "Integer 64-bit")]
        bInt64, //x12
        [Display(Name = "Decimal 128-bit")]
        bDecimalFlloatingPoint, //x13
        [Display(Name = "Min key")]
        bMinkey, //xFF
        [Display(Name = "Max key")]
        bMaxKey //x7F
    }
}
