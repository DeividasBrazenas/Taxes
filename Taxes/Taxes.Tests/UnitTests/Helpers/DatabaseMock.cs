using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Taxes.Service.DataLayer;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Tests.UnitTests.Helpers
{
    public static class DatabaseMock
    {
        public static Mock<TaxesContext> GetDataBaseMock(Municipality[] municipalities, Tax[] taxes)
        {
            municipalities[0].Taxes = taxes;

            var mockMunicipalities = MockDbSet(municipalities);
            var mockTaxes = MockDbSet(taxes);
            var mockContext = new Mock<TaxesContext>();

            mockContext.Setup(x => x.Set<Municipality>()).Returns(mockMunicipalities.Object);
            mockContext.Setup(x => x.Set<Tax>()).Returns(mockTaxes.Object);
            mockContext.Setup(x => x.Municipalities).Returns(mockMunicipalities.Object);
            mockContext.Setup(x => x.Taxes).Returns(mockTaxes.Object);

            return mockContext;
        }

        private static Mock<DbSet<T>> MockDbSet<T>(IEnumerable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            var qdata = data.AsQueryable();
            mockSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(qdata.Provider);
            mockSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(qdata.Expression);
            mockSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(qdata.ElementType);
            mockSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(qdata.GetEnumerator());
            return mockSet;
        }
    }
}