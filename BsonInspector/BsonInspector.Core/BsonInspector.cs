using BsonInspector.Core.Abstract;
using BsonInspector.Core.ValuePresenters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BsonInspector.Core
{
    public class BsonInspector : IBsonInsperctor
    {

        private const int Int32Length = 4;
        private const int Int64Length = 8;
        private const int Uint64Length = 8;
        private const int DoubleLength = 8;
        private const int Decimal128Length = 16;

        private const byte EndingSymbol = 0x00;

        public BsonInspector()
        {
        }

        public BsonInspectionResult Inspect(Stream bson)
        {
            try
            {
                using (var reader = new BinaryReader(bson))
                {
                    return new BsonInspectionResult(ReadDocument(reader, true));
                }
            }
            catch (Exception ex)
            {
                return new BsonInspectionResult(ex.Message);
            }
        }

        /// <summary>
        /// If top level validate bson to document length
        /// </summary>
        private BsonDocument ReadDocument(BinaryReader data, bool isTopLevel = false)
        {
            var docTotal = data.ReadInt32();
            if (isTopLevel && docTotal != data.BaseStream.Length)
                throw new InvalidDataException($"BSON document states that the length is: {docTotal}, but actual file size is: {data.BaseStream.Length}.");

            var elements = ReadElements(data);

            return new BsonDocument(docTotal, new byte[] { }, elements.ToArray());
        }

        private IList<BsonElement> ReadElements(BinaryReader data)
        {
            var elements = new List<BsonElement>();
            while (data.BaseStream.Position < data.BaseStream.Length)
            {
                var element = ReadElement(data);
                elements.Add(element);

                var checkEnd = data.ReadByte();
                if (checkEnd.Equals(EndingSymbol))
                    break;
                else
                    data.BaseStream.Position -= 1;
            }

            return elements;
        }

        private BsonElement ReadElement(BinaryReader data)
        {
            var dataType = data.ReadByte();
            var elementType = ParseElementType(dataType);
            string elementName = ReadElementName(data);

            IValuePresenter valuePresenter;
            byte[] value;
            BsonDocument innerDocument = null;
            BinarySubtybes? subtybe = null;
            switch (elementType)
            {
                case BsonElementTypes.bDouble:
                    value = data.ReadBytes(DoubleLength);
                    valuePresenter = new FloatingPointValuePresenter(value);
                    break;
                case BsonElementTypes.bString:
                case BsonElementTypes.bSymbol:
                case BsonElementTypes.bJavascriptCode:
                    value = ReadStringValue(data);
                    valuePresenter = new StringValuePresenter(value.SkipLast(1).ToArray());
                    break;
                case BsonElementTypes.bEmbeddedDocument:
                case BsonElementTypes.bDocumentArray:
                    innerDocument = ReadDocument(data);
                    value = innerDocument.ValueInBytes;
                    valuePresenter = new DocumentValuePresenter(innerDocument);
                    break;
                case BsonElementTypes.bBinary:
                    var binaryValueResult = ReadBinaryValue(data);
                    valuePresenter = binaryValueResult.presenter;
                    value = binaryValueResult.value;
                    subtybe = binaryValueResult.subtybe;
                    break;
                case BsonElementTypes.bUndefined:
                    value = new byte[] { };
                    valuePresenter = new PredefinedValuePresenter("UNDEFINED");
                    break;
                case BsonElementTypes.bObjectId:
                    value = data.ReadBytes(12);
                    valuePresenter = new ObjectIdValuePresenter(value);
                    break;
                case BsonElementTypes.bBoolean:
                    value = new byte[] { data.ReadByte() };
                    valuePresenter = new BooleanValuePresenter(value[0]);
                    break;
                case BsonElementTypes.bUTCDateTime:
                    value = data.ReadBytes(Int64Length).ToArray();
                    valuePresenter = new UtcDateTimePresenter(value);
                    break;
                case BsonElementTypes.bNull:
                    value = new byte[] { };
                    valuePresenter = new PredefinedValuePresenter("NULL");
                    break;
                case BsonElementTypes.bRegularExpression:
                    var patternValue = ReadCstringValue(data);
                    var optionsValue = ReadCstringValue(data);
                    value = patternValue.Concat(optionsValue).ToArray();
                    valuePresenter = new RegexValuePresenter(
                        new StringValuePresenter(patternValue.SkipLast(1).ToArray()),
                        new StringValuePresenter(optionsValue.SkipLast(1).ToArray()));
                    break;
                case BsonElementTypes.bDbPointer:
                    var stringValue = ReadStringValue(data);
                    var objectIdValue = data.ReadBytes(12);
                    value = stringValue.Concat(objectIdValue).ToArray();
                    valuePresenter = new DbPointerValuePresenter(
                        new StringValuePresenter(stringValue.SkipLast(1).ToArray()),
                        new ObjectIdValuePresenter(objectIdValue));
                    break;
                case BsonElementTypes.bJavascriptCodeScoped:
                    var codeWBytesCount = data.ReadInt32(); // ?
                    stringValue = ReadStringValue(data);
                    innerDocument = ReadDocument(data);
                    value = stringValue.Concat(data.ReadBytes(innerDocument.DocumentLength)).ToArray();
                    valuePresenter = new CompositeValuePresenter(
                        new StringValuePresenter(stringValue.SkipLast(1).ToArray()),
                        new DocumentValuePresenter(innerDocument));
                    break;
                case BsonElementTypes.bInt32:
                    value = data.ReadBytes(Int32Length);
                    valuePresenter = new IntValuePresenter(value);
                    break;
                case BsonElementTypes.bTimeStamp:
                    value = data.ReadBytes(Uint64Length);
                    valuePresenter = new TimeStampVauePresenter(value);
                    break;
                case BsonElementTypes.bInt64:
                    value = data.ReadBytes(Int64Length);
                    valuePresenter = new IntValuePresenter(value);
                    break;
                case BsonElementTypes.bDecimalFlloatingPoint:
                    value = data.ReadBytes(Decimal128Length);
                    valuePresenter = new FloatingPointValuePresenter(value);
                    break;
                case BsonElementTypes.bMinkey:
                    value = new byte[] { };
                    valuePresenter = new PredefinedValuePresenter("min key");
                    break;
                case BsonElementTypes.bMaxKey:
                    value = new byte[] { };
                    valuePresenter = new PredefinedValuePresenter("max key");
                    break;
                default:
                    throw new ArgumentException($"Unsupported element type {elementType}");
            }

            return new BsonElement(elementType, elementName, new BsonElementValue(value, valuePresenter, innerDocument), subtybe);
        }

        private byte[] ReadStringValue(BinaryReader data)
        {
            int numberOfValueBytes = data.ReadInt32();
            return data.ReadBytes(numberOfValueBytes).ToArray();
        }

        private byte[] ReadCstringValue(BinaryReader data)
        {
            var nameBytes = new List<byte>();
            byte readByte;
            do
            {
                readByte = data.ReadByte();
                nameBytes.Add(readByte);
            }
            while (readByte != EndingSymbol);

            return nameBytes.ToArray();
        }

        private (IValuePresenter presenter, byte[] value, BinarySubtybes subtybe) ReadBinaryValue(BinaryReader data)
        {
            var valueBytesCount = data.ReadInt32();
            var subtypeByte = data.ReadByte();
            var subtype = ParseBinarySubtype(subtypeByte);

            var value = data.ReadBytes(valueBytesCount);
            IValuePresenter valuePresenter;
            switch (subtype)
            {
                case BinarySubtybes.Generic:
                    valuePresenter = new GenericBinaryValuePresenter(value);
                    break;
                case BinarySubtybes.UUID:
                case BinarySubtybes.UUIDold:
                    valuePresenter = new GUIDBinaryValuePresenter(value);
                    break;
                default:
                    valuePresenter = new GenericBinaryValuePresenter(value);
                    break;
            }

            return (valuePresenter, value, subtype);
        }

        private string ReadElementName(BinaryReader data) => Encoding.UTF8.GetString(ReadCstringValue(data).SkipLast(1).ToArray());

        private BsonElementTypes ParseElementType(byte dataType)
        {
            switch (dataType)
            {
                case 0x01:
                    return BsonElementTypes.bDouble;
                case 0x02:
                    return BsonElementTypes.bString;
                case 0x03:
                    return BsonElementTypes.bEmbeddedDocument;
                case 0x04:
                    return BsonElementTypes.bDocumentArray;
                case 0x05:
                    return BsonElementTypes.bBinary;
                case 0x06:
                    return BsonElementTypes.bUndefined;
                case 0x07:
                    return BsonElementTypes.bObjectId;
                case 0x08:
                    return BsonElementTypes.bBoolean;
                case 0x09:
                    return BsonElementTypes.bUTCDateTime;
                case 0x0A:
                    return BsonElementTypes.bNull;
                case 0x0B:
                    return BsonElementTypes.bRegularExpression;
                case 0x0C:
                    return BsonElementTypes.bDbPointer;
                case 0x0D:
                    return BsonElementTypes.bJavascriptCode;
                case 0x0E:
                    return BsonElementTypes.bSymbol;
                case 0x0F:
                    return BsonElementTypes.bJavascriptCodeScoped;
                case 0x10:
                    return BsonElementTypes.bInt32;
                case 0x11:
                    return BsonElementTypes.bTimeStamp;
                case 0x12:
                    return BsonElementTypes.bInt64;
                case 0x13:
                    return BsonElementTypes.bDecimalFlloatingPoint;
                case 0xFF:
                    return BsonElementTypes.bMinkey;
                case 0x7F:
                    return BsonElementTypes.bMaxKey;
                default:
                    throw new ArgumentException($"Unknown bson element type: {dataType}");
            }
        }

        private BinarySubtybes ParseBinarySubtype(byte subtypeByte)
        {
            switch (subtypeByte)
            {
                case 0x00:
                    return BinarySubtybes.Generic;
                case 0x01:
                    return BinarySubtybes.Function;
                case 0x02:
                    return BinarySubtybes.Binary;
                case 0x03:
                    return BinarySubtybes.UUIDold;
                case 0x04:
                    return BinarySubtybes.UUID;
                case 0x05:
                    return BinarySubtybes.Md5;
                case 0x80:
                    return BinarySubtybes.UserDefined;
                default:
                    throw new ArgumentException($"Unknown binary subtype: {subtypeByte}");
            }
        }

        private int GetInt32(BinaryReader data, int start)
        {
            return BitConverter.ToInt32(data.ReadBytes(Int32Length));
        }

        private int GetInt32(ArraySegment<byte> data, int start)
        {
            return BitConverter.ToInt32(data.AsSpan(start, Int32Length));
        }
    }
}
