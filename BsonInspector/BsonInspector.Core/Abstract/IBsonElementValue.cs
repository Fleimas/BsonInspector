using System;
using System.Collections.Generic;
using System.Text;

namespace BsonInspector.Core.Abstract
{
    public interface IBsonElementValue
    {
        byte[] Bytes { get; }

        IValuePresenter HumanReadablePresenter { get; }

        bool IsDocument { get; }

        BsonDocument Document { get; }
    }
}
