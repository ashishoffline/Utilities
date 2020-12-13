using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Utilities.Extensions
{
    public static class DataTableExtension
    {
        public static IList<T> ToList<T>(this DataTable dataTable)
        {
            IList<T> data;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                data = new List<T>(dataTable.Rows.Count);
                PropertyInfo[] propertyInfos = (PropertyInfo[])typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetSetMethod() != null);

                int dictionaryLength = Math.Min(propertyInfos.Length, dataTable.Columns.Count);
                IDictionary<PropertyInfo, string> keyValues = new Dictionary<PropertyInfo, string>(dictionaryLength);

                foreach (DataColumn column in dataTable.Columns)
                {
                    foreach (PropertyInfo property in propertyInfos)
                    {
                        if (property.Name.ToUpperInvariant() == column.ColumnName.ToUpperInvariant())
                        {
                            keyValues.Add(property, column.ColumnName);
                        }
                    }
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    T item = GetItem<T>(row, keyValues);
                    data.Add(item);
                }
            }
            else
            {
                data = new List<T>();
            }
            return data;
        }

        private static T GetItem<T>(DataRow row, IDictionary<PropertyInfo, string> keyValues)
        {
            T obj = Activator.CreateInstance<T>();
            foreach (var item in keyValues)
            {
                if (row[item.Value] != DBNull.Value)
                {
                    item.Key.SetValue(obj, row[item.Value]);
                }
            }
            return obj;
        }
    }
}
