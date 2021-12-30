
using EFCore.DBLibrary.AdventureWorks;
using EFCore.DBLibrary.AdventureWorks.DTOs;
using EFCore.InventoryHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFCore.ConsoleApp.AdventureWorks
{
    class Program
    {
        #region Private Members

        private static IConfigurationRoot? _configuration;
        private static DbContextOptionsBuilder<AdventureWorksContext> _optionsBuilder = new DbContextOptionsBuilder<AdventureWorksContext>();

        #endregion


        static void Main(String[] args)
        {
            BuildOptions();

            ////ListPeople();
            //ListPeopleThenOrderAndTake();
            //Console.WriteLine();

            //QueryPeopleOrderedToListAndTake();
            //Console.WriteLine();

            //do
            //{
            //    Console.WriteLine("-> Please Enter the partial First or Last Name, or the Person Type to search for: ");
            //    var result = Console.ReadLine();

            //    if (result is not null)
            //    {
            //        //int pageSize = 10;
            //        //for (int pageNumber = 0; pageNumber < 10; pageNumber++)
            //        //{
            //        //    Console.WriteLine($"\tPage {pageNumber + 1} : ");
            //        //    FilteredAndPagedResult(result, pageNumber, pageSize);
            //        //}

            //        FilteredPeople(result);
            //    }

            //    Console.Write("\tWould you like to continue [Y/N] : ");
            //} while (Console.ReadLine() == "Y");

            //QueryPeopleWithNoTracking();

            //ListAllSalesPeople();

            //ShowAllSalesPeopleUsingProjection();

            //GenerateSalesReportData();

            GenerateSalesReportDataToDTO();

            Console.WriteLine();
            Console.WriteLine("-> Please Press Enter to Exit.");
            Console.ReadLine();
        }


        #region Helper Methods

        static void BuildOptions()
        {
            Console.WriteLine("-> Connecting to AdventureWorks database using connection string.");
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            //_optionsBuilder = new DbContextOptionsBuilder<AdventureWorksContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AdventureWorks"));
            Console.WriteLine("\tSuccessfully connected to the database.");
        }

        private static void ListPeople()
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var people = db.People.OrderByDescending(x => x.LastName).Take(20).ToList();
                foreach (var person in people)
                {
                    Console.WriteLine($"\t" +
                        $"{person.FirstName} {person.LastName}");
                }
            }
        }

        private static void ListPeopleThenOrderAndTake()
        {
            Console.WriteLine("-> List people then order and take: ");
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var people = db.People.ToList().OrderByDescending(x => x.LastName);
                foreach (var person in people.Take(10))
                {
                    Console.WriteLine($"\t{person.FirstName} {person.LastName}");
                }
            }
            Console.WriteLine();
        }

        private static void QueryPeopleOrderedToListAndTake()
        {
            Console.WriteLine("-> Query people ordered to list and take: ");
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var query = db.People.OrderByDescending(x => x.LastName);
                var result = query.Take(10);
                foreach (var person in result)
                {
                    Console.WriteLine($"\t{person.FirstName} {person.LastName}");
                }
            }
            Console.WriteLine();
        }

        private static void FilteredPeople(string filter)
        {
            Console.WriteLine("-> Filtering People : ");
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var searchTerm = filter.ToLower();
                var query = db.People.Where(x =>
                    x.LastName.ToLower().Contains(searchTerm) ||
                    x.FirstName.ToLower().Contains(searchTerm) ||
                    x.PersonType.ToLower().Contains(searchTerm));

                foreach (var person in query)
                {
                    Console.WriteLine($"\t{person.FirstName} {person.LastName}, {person.PersonType}");
                }
            }
        }

        private static void FilteredAndPagedResult(string filter, int pageNumber, int pageSize)
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var searchTerm = filter.ToLower();
                var query = db.People.Where(x =>
                    x.LastName.ToLower().Contains(searchTerm) ||
                    x.FirstName.ToLower().Contains(searchTerm) ||
                    x.PersonType.ToLower().Contains(searchTerm)
                ).OrderBy(x => x.LastName)
                 .Skip(pageNumber * pageSize)
                 .Take(pageSize);

                foreach(var person in query)
                {
                    Console.WriteLine($"\t\t{person.FirstName} {person.LastName}, {person.PersonType}");
                }
            }
        }

        private static void QueryPeopleWithNoTracking()
        {
            Console.WriteLine("-> Query people ordered with no tracking : ");
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var query = db.People.AsNoTracking()
                                     .OrderByDescending(x => x.LastName);
                var result = query.Take(10);
                foreach (var person in result)
                {
                    Console.WriteLine($"\t{person.FirstName} {person.LastName}");
                }
            }
            Console.WriteLine();
        }

        private static string GetSalesPersonDetails(SalesPerson sp)
        {
            return $"{sp.BusinessEntityId,-5}" +
                              $"{sp.TerritoryId,-5}" +
                              $"{sp.SalesQuota,-15}" +
                              $"{sp.Bonus,-15}" +
                              $"{sp.SalesYtd,-20}" +
                              $"{sp.BusinessEntity?.BusinessEntity?.FirstName ?? ""}, {sp.BusinessEntity?.BusinessEntity?.LastName ?? ""}";
        }

        private static void ListAllSalesPeople()
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var salesPeople = db.SalesPeople.Include(x => x.BusinessEntity)
                                                .ThenInclude(y => y.BusinessEntity)
                                                .AsNoTracking()
                                                .ToList();
                Console.WriteLine($"{"ID",-5}" +
                                  $"{"TID",-5}" +
                                  $"{"Quota",-15}" +
                                  $"{"Bonus",-15}" +
                                  $"{"YTDSales",-20}" +
                                  $"{"Name",-50}");
                foreach (var salesPerson in salesPeople)
                {
                    //var p = db.People.FirstOrDefault(x =>
                    //                    x.BusinessEntityId 
                    //                        == salesPerson.BusinessEntityId);
                    Console.WriteLine(GetSalesPersonDetails(salesPerson));
                }
            }
        }

        private static void ShowAllSalesPeopleUsingProjection()
        {
            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                // No need to add .Include() and .ThenInclude() while using projection.
                var salesPeople =
                    db.SalesPeople.AsNoTracking()
                                  .Select(x => new
                                  {
                                      x.BusinessEntityId,
                                      x.BusinessEntity.BusinessEntity.FirstName,
                                      x.BusinessEntity.BusinessEntity.LastName,
                                      x.SalesQuota,
                                      x.SalesYtd,
                                      x.SalesLastYear
                                  }).ToList();

                Console.WriteLine($"{"BID",-5}" +
                                  $"{"Name",-50}" +
                                  $"{"Quota",-15}" +
                                  $"{"YTDSales",-20}" +
                                  $"{"SalesLastYear",-20}");
                foreach (var sp in salesPeople)
                {
                    Console.WriteLine(
                        $"{sp.BusinessEntityId,-5}" +
                        $"{sp.FirstName + " " + sp.LastName, -50}" +
                        $"{sp.SalesQuota, -15}" +
                        $"{sp.SalesYtd, -20}" +
                        $"{sp.SalesLastYear, -20}"
                    );
                }
            }
        }

        private static void GenerateSalesReportData()
        {
            Console.WriteLine("-> Generating sales report : ");
            Console.Write("\tWhat is the minimum amount of sales? => ");
            var input = Console.ReadLine();
            
            if(!decimal.TryParse(input, out decimal filter))
            {
                Console.WriteLine("Bad Input");
                return;
            }

            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var salesReportData = db.SalesPeople.Select(sp => new
                {
                    BeId = sp.BusinessEntityId,
                    sp.BusinessEntity.BusinessEntity.FirstName,
                    sp.BusinessEntity.BusinessEntity.LastName,
                    sp.SalesYtd,
                    Territories =
                        sp.SalesTerritoryHistories.Select(y => y.Territory.Name),
                    OrderCount = sp.SalesOrderHeaders.Count(),
                    TotalProductsSold = 
                        sp.SalesOrderHeaders.SelectMany(y => y.SalesOrderDetails)
                                            .Sum(z => z.OrderQty)
                }).Where(srdata => srdata.SalesYtd > filter)
                  .OrderBy(srds => srds.LastName)
                  .ThenBy(srds => srds.FirstName)
                  .ThenByDescending(srds => srds.SalesYtd)
                  .ToList();

                Console.WriteLine($"{"BID",-5}" +
                                  $"{"Name",-30}" +
                                  $"{"YTDSales",-20}" +
                                  $"{"Territories",-30}" +
                                  $"{"OrderCount", -15}" +
                                  $"{"ProductsSold", -15}");
                Console.WriteLine();

                foreach (var srd in salesReportData)
                {
                    Console.WriteLine($"{srd.BeId, -5}" +
                        $"{srd.FirstName + " " + srd.LastName, -30}" +
                        $"{srd.SalesYtd, -20}" +
                        $"{string.Join(',', srd.Territories), -30}" +
                        $"{srd.OrderCount, -15}" +
                        $"{srd.TotalProductsSold, -15}");
                }
            }
        }

        private static void GenerateSalesReportDataToDTO()
        {
            Console.WriteLine("-> Generating sales report : ");
            Console.Write("\tWhat is the minimum amount of sales? => ");
            var input = Console.ReadLine();

            if (!decimal.TryParse(input, out decimal filter))
            {
                Console.WriteLine("Bad Input");
                return;
            }

            using (var db = new AdventureWorksContext(_optionsBuilder.Options))
            {
                var salesReportData = db.SalesPeople.Select(sp => new SalesReportListingDTO
                {
                    BusinessEntityId = sp.BusinessEntityId,
                    FirstName = sp.BusinessEntity.BusinessEntity.FirstName,
                    LastName = sp.BusinessEntity.BusinessEntity.LastName,
                    SalesYtd = sp.SalesYtd,
                    Territories =
                        sp.SalesTerritoryHistories.Select(y => y.Territory.Name),
                    TotalOrders = sp.SalesOrderHeaders.Count(),
                    TotalProductsSold =
                        sp.SalesOrderHeaders.SelectMany(y => y.SalesOrderDetails)
                                            .Sum(z => z.OrderQty)
                }).Where(srdata => srdata.SalesYtd > filter)
                  .OrderBy(srds => srds.LastName)
                  .ThenBy(srds => srds.FirstName)
                  .ThenByDescending(srds => srds.SalesYtd);

                Console.WriteLine($"{"BID",-5}" +
                                  $"{"Name",-30}" +
                                  $"{"YTDSales",-20}" +
                                  $"{"Territories",-30}" +
                                  $"{"OrderCount",-15}" +
                                  $"{"ProductsSold",-15}");
                Console.WriteLine();

                foreach (var srd in salesReportData)
                {
                    Console.WriteLine(srd.ToString());
                }
            }
        }

        #endregion
    }
}
