using System;
using System.Collections.Generic;
using System.Text;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Converters;

namespace RaceYa.Models.Converters
{
    class TimeSpanConverter : DocumentConverter
    {
        public TimeSpanConverter(Type targetType) : base(targetType)
        {
        }

        public override bool ConvertFrom(DocumentObject value, out object? result)
        {
            if (value.Type == DocumentObjectType.Double)
            {
                result = TimeSpan.FromMilliseconds(value.Double);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool ConvertTo(object? value, out object? result)
        {
            if (value is TimeSpan)
            {
                result = ((TimeSpan) value).TotalMilliseconds;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }
    }
}
