using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Parser.DAL.Context
{
    public static class DbFunctionExtensions
    {
        public static int? DatePart(string type, DateTime? date) => throw new Exception();

        public static void ConfigureDbFunctions(this ModelBuilder modelBuilder)
        {
            var mi = typeof(DbFunctionExtensions).GetMethod(nameof(DatePart));

            modelBuilder.HasDbFunction(mi, b => b.HasTranslation(e =>
            {
                var ea = e.ToArray();
                var args = new[]
                {
                    new SqlFragmentExpression((ea[0] as SqlConstantExpression)?.Value.ToString()),
                    ea[1]
                };
                return SqlFunctionExpression.Create(nameof(DatePart), args, typeof(int?), null);
            }));
        }
    }
}