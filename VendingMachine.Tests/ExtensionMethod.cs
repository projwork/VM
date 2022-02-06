using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace VendingMachine.Tests
{
    public static class ExtensionMethods
    {
        public static DbSet<T> Initialize<T>(this DbSet<T> dbSet, IQueryable<T> data) where T : class
        {
            A.CallTo(() => ((IQueryable<T>)dbSet).Provider).Returns(data.Provider);
            A.CallTo(() => ((IQueryable<T>)dbSet).Expression).Returns(data.Expression);
            A.CallTo(() => ((IQueryable<T>)dbSet).ElementType).Returns(data.ElementType);
            A.CallTo(() => ((IEnumerable<T>)dbSet).GetEnumerator()).Returns(data.GetEnumerator());
            return dbSet;
        }
    }
}
