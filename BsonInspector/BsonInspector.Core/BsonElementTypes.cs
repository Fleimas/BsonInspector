using System;

namespace BsonInspector.Core
{
    public enum BsonElementTypes
    {
        bDouble, //x01
        bString, //x02
        bEmbeddedDocument, //x03
        bDocumentArray, //x04
        bBinary, //x05
        [Obsolete]
        bUndefined, //x06
        bObjectId, //x07
        bBoolean, //x08
        bUTCDateTime, //x09
        bNull, //x0A
        bRegularExpression, //x0B
        [Obsolete]
        bDbPointer, //x0C
        bJavascriptCode, //x0D
        [Obsolete]
        bSymbol, //x0E
        bJavascriptCodeScoped, //x0F
        bInt32, //x10
        bTimeStamp,//x11
        bInt64, //x12
        bDecimalFlloatingPoint, //x13
        bMinkey, //xFF
        bMaxKey //x7F
    }
}
