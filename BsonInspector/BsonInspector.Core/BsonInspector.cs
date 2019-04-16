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

        public BsonInspectionResult Inspect(byte[] bson)
        {
            try
            {
                return new BsonInspectionResult(ReadDocument(bson, true));
            }
            catch (Exception ex)
            {
                return new BsonInspectionResult(ex.Message);
            }
        }

        /// <summary>
        /// If top level validate bson to document length
        /// </summary>
        private BsonDocument ReadDocument(byte[] data, bool isTopLevel = false)
        {
            int offset = 0;
            var docTotal = GetInt32(data, 0);
            offset += Int32Length;
            if (isTopLevel && docTotal != data.Length)
                throw new InvalidDataException("bson length is not valid");

            var lastByte = data[docTotal - 1];
            if (isTopLevel && !lastByte.Equals(EndingSymbol))
                throw new InvalidDataException("bson missing ending symbol \x00");

            var elements = ReadElements(new ArraySegment<byte>(data, offset, docTotal - offset - 1));

            return new BsonDocument(docTotal, elements.ToArray());
        }

        private IList<BsonElement> ReadElements(ArraySegment<byte> data)
        {
            var elements = new List<BsonElement>();

            while (data.Count > 0)
            {
                var element = ReadElement(data, out int readBytesCount);
                data = data.Slice(readBytesCount, data.Count - readBytesCount);
                elements.Add(element);
            }

            return elements;
        }

        private BsonElement ReadElement(ArraySegment<byte> data, out int readBytesCount)
        {
            int offset = 0;
            var dataType = data[offset];
            offset += 1;
            var elementType = ParseElementType(dataType);
            string elementName = ReadElementName(data.Slice(offset, data.Count - offset), out int readNameBytesCount);
            offset += readNameBytesCount;

            int readValueBytesCount;
            IValuePresenter valuePresenter;
            byte[] value;
            BsonDocument innerDocument = null;

            switch (elementType)
            {
                case BsonElementTypes.bDouble:
                    value = data.Slice(offset, DoubleLength).ToArray();
                    valuePresenter = new FloatingPointValuePresenter(value);
                    offset += DoubleLength;
                    break;
                case BsonElementTypes.bString:
                case BsonElementTypes.bSymbol:
                case BsonElementTypes.bJavascriptCode:
                    value = ReadStringValue(data.Slice(offset, data.Count - offset), out readValueBytesCount);
                    valuePresenter = new StringValuePresenter(value);
                    offset += readValueBytesCount;
                    break;
                case BsonElementTypes.bEmbeddedDocument:
                case BsonElementTypes.bDocumentArray:
                    innerDocument = ReadDocument(data.Slice(offset, data.Count - offset).ToArray());
                    value = data.Slice(offset, innerDocument.DocumentLength).ToArray();
                    offset += innerDocument.DocumentLength;
                    valuePresenter = new DocumentValuePresenter(innerDocument);
                    break;
                case BsonElementTypes.bBinary:
                    valuePresenter = ReadBinaryValue(data.Slice(offset, data.Count - offset), out readValueBytesCount, out value);
                    offset += readValueBytesCount;
                    break;
                case BsonElementTypes.bUndefined:
                    value = new byte[] { };
                    valuePresenter = new PredefinedValuePresenter("UNDEFINED");
                    break;
                case BsonElementTypes.bObjectId:
                    value = data.Slice(offset, 12).ToArray();
                    valuePresenter = new ObjectIdValuePresenter(value);
                    offset += 12;
                    break;
                case BsonElementTypes.bBoolean:
                    value = new byte[] { data[offset] };
                    valuePresenter = new BooleanValuePresenter(data[offset]);
                    offset += 1;
                    break;
                case BsonElementTypes.bUTCDateTime:
                    value = data.Slice(offset, Int64Length).ToArray();
                    valuePresenter = new UtcDateTimePresenter(value);
                    offset += Int64Length;
                    break;
                case BsonElementTypes.bNull:
                    value = new byte[] { };
                    valuePresenter = new PredefinedValuePresenter("NULL");
                    break;
                case BsonElementTypes.bRegularExpression:
                    var patternValue = ReadCstringValue(data.Slice(offset, data.Count - offset).ToArray(), out readValueBytesCount);
                    offset += readValueBytesCount;
                    var optionsValue = ReadCstringValue(data.Slice(offset, data.Count - offset).ToArray(), out readValueBytesCount);
                    offset += readValueBytesCount;
                    value = patternValue.Concat(optionsValue).ToArray();
                    valuePresenter = new RegexValuePresenter(new StringValuePresenter(patternValue), new StringValuePresenter(optionsValue));
                    break;
                case BsonElementTypes.bDbPointer:
                    var stringValue = ReadStringValue(data.Slice(offset, data.Count - offset), out readValueBytesCount);
                    offset += readValueBytesCount;
                    var objectIdValue = data.Slice(offset, 12).ToArray();
                    offset += 12;
                    value = stringValue.Concat(objectIdValue).ToArray();
                    valuePresenter = new DbPointerValuePresenter(new StringValuePresenter(stringValue), new ObjectIdValuePresenter(objectIdValue));
                    break;
                case BsonElementTypes.bJavascriptCodeScoped:
                    var codeWBytesCount = GetInt32(data, offset);
                    offset += Int32Length;
                    stringValue = ReadStringValue(data.Slice(offset, data.Count - offset).ToArray(), out readValueBytesCount);
                    offset += readValueBytesCount;
                    innerDocument = ReadDocument(data.Slice(offset, data.Count - offset).ToArray());
                    offset += innerDocument.DocumentLength;
                    value = stringValue.Concat(data.Slice(offset, innerDocument.DocumentLength)).ToArray();
                    valuePresenter = new CompositeValuePresenter(new StringValuePresenter(stringValue), new DocumentValuePresenter(innerDocument));
                    break;
                case BsonElementTypes.bInt32:
                    value = data.Slice(offset, Int32Length).ToArray();
                    valuePresenter = new IntValuePresenter(value);
                    offset += Int32Length;
                    break;
                case BsonElementTypes.bTimeStamp:
                    value = data.Slice(offset, Uint64Length).ToArray();
                    valuePresenter = new TimeStampVauePresenter(value);
                    offset += Uint64Length;
                    break;
                case BsonElementTypes.bInt64:
                    value = data.Slice(offset, Int64Length).ToArray();
                    valuePresenter = new IntValuePresenter(value);
                    offset += Int64Length;
                    break;
                case BsonElementTypes.bDecimalFlloatingPoint:
                    value = data.Slice(offset, Decimal128Length).ToArray();
                    valuePresenter = new FloatingPointValuePresenter(value);
                    offset += Decimal128Length;
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

            readBytesCount = offset;
            return new BsonElement(elementType, elementName, new BsonElementValue(value, valuePresenter, innerDocument));
        }

        private byte[] ReadStringValue(ArraySegment<byte> data, out int readBytesCount)
        {
            int offset = 0;
            int numberOfValueBytes = GetInt32(data, offset);
            offset += Int32Length;

            readBytesCount = offset + numberOfValueBytes;
            return data.Slice(offset, numberOfValueBytes - 1).ToArray();
        }

        private byte[] ReadCstringValue(ArraySegment<byte> data, out int readBytesCount)
        {
            var nameBytes = new List<byte>();
            int index = 0;
            byte readByte = data[index];

            while (readByte != EndingSymbol)
            {
                nameBytes.Add(readByte);
                index++;
                readByte = data[index];
            }

            readBytesCount = index + 1; //1 is end symbol 
            return nameBytes.ToArray();
        }

        private IValuePresenter ReadBinaryValue(ArraySegment<byte> data, out int readBytesCount, out byte[] value)
        {
            int offset = 0;
            var valueBytesCount = GetInt32(data, offset);
            offset += Int32Length;
            var subtypeByte = data[offset];
            offset += 1;
            var subtype = ParseBinarySubtype(subtypeByte);

            readBytesCount = offset + valueBytesCount;

            value = data.AsSpan(offset, valueBytesCount).ToArray();
            switch (subtype)
            {
                case BinarySubtybes.Generic:
                    return new GenericBinaryValuePresenter(value);
                case BinarySubtybes.UUID:
                case BinarySubtybes.UUIDold:
                    return new GUIDBinaryValuePresenter(value);
                default:
                    return new GenericBinaryValuePresenter(value);
            }
        }

        private string ReadElementName(ArraySegment<byte> data, out int readNameBytesCount) =>
                Encoding.UTF8.GetString(ReadCstringValue(data, out readNameBytesCount));

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

        private int GetInt32(byte[] data, int start)
        {
            return BitConverter.ToInt32(data.AsSpan(start, Int32Length));
        }

        private int GetInt32(ArraySegment<byte> data, int start)
        {
            return BitConverter.ToInt32(data.AsSpan(start, Int32Length));
        }
    }
}
