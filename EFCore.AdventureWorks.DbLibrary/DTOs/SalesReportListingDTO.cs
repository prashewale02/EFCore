using System.ComponentModel.DataAnnotations;

namespace EFCore.AdventureWorks.DbLibrary.DTOs
{
    public class SalesReportListingDTO
    {
        [Required]
        public int BusinessEntityId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal? SalesYtd { get; set; }
        public IEnumerable<string> Territories { get; set; } = new List<string>();
        public int TotalProductsSold { get; set; }
        public int TotalOrders { get; set; }
        public string DisplayName => $"{FirstName} {LastName}";
        public string DisplayTerritories => string.Join(",", Territories);
        public override string ToString()
        {
            return $"{BusinessEntityId,-5}" +
                   $"{DisplayName,-30}" +
                   $"{SalesYtd,-20}" +
                   $"{DisplayTerritories,-30}" +
                   $"{TotalOrders,-15}" +
                   $"{TotalProductsSold,-15}";
        }

    }
}
