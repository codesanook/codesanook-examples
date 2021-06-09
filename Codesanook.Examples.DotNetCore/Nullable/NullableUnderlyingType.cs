using System;
using System.Data;
using Xunit;

namespace Codesanook.Examples.DotNetCore.Tests.Nullable
{

    public class NullableUnderlyingType
    {
        [Fact]
        public void Test()
        {
            var type = GetUnderlyingType<DateTime?>();
            DbType dbType;
            Enum.TryParse(Type.GetTypeCode(type).ToString(), true, out dbType);
            Assert.Equal(DbType.DateTime, dbType);

        }

        public Type GetUnderlyingType<TProperty>()
        {
            var propertyType = typeof(TProperty);
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return System.Nullable.GetUnderlyingType(propertyType);
            }

            return propertyType;
        }
    }
}
