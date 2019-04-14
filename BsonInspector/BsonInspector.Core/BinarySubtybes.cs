using System;

namespace BsonInspector.Core
{
    public enum BinarySubtybes
    {
        Generic, //0x00
        Function, //0x01
        [Obsolete]
        Binary, //0x02
        [Obsolete]
        UUIDold, //0x03
        UUID, //0x04
        Md5, //0x05
        UserDefined //0x80
    }
}
