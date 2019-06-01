using System;
using Taxes.Service.DataLayer.Models;

namespace Taxes.Tests.UnitTests.Helpers
{
    class TestData
    {
        public static Municipality[] Municipalities = {
            new Municipality(){Id = 1, Name="Vilnius"},
            new Municipality(){Id = 2, Name="Kaunas"},
        };

        public static Tax[] Taxes = {
            new Tax(){Id = 1, MunicipalityId = 1, Frequency = TaxFrequency.Yearly, StartDate = new DateTime(2016,01, 01), EndDate = new DateTime(2016,12, 31), Value = 0.2},
            new Tax(){Id = 2, MunicipalityId = 1, Frequency = TaxFrequency.Monthly, StartDate = new DateTime(2016,05, 01), EndDate = new DateTime(2016,05, 31), Value = 0.4},
            new Tax(){Id = 3, MunicipalityId = 1, Frequency = TaxFrequency.Daily, StartDate = new DateTime(2016,01, 01), EndDate = new DateTime(2016,01, 01), Value = 0.1},
            new Tax(){Id = 4, MunicipalityId = 1, Frequency = TaxFrequency.Daily, StartDate = new DateTime(2016,12, 25), EndDate = new DateTime(2016,12, 25), Value = 0.1},
        };
    }
}
