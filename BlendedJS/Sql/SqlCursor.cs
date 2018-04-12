using System.Collections.Generic;
using System.Data;

namespace BlendedJS.Sql
{
    public class SqlCursor : Cursor
    {
        private IDataReader _dataReader;

        public SqlCursor(IEnumerable<object> enumerable, IDataReader dataReader) : base(enumerable)
        {
            _dataReader = dataReader;
        }

        public override void close()
        {
            base.close();
            _dataReader.Dispose();
        }
    }
}