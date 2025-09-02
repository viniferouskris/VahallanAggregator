// Define models in this same file for now
using CsvHelper.Configuration.Attributes;

// Define models in this same file for now
// Define models in this same file for now
public class ProductionOrder
{
    public string Customer { get; set; }
    public string Pattern { get; set; }
    public string Colorway { get; set; }
    public decimal SquareFeet { get; set; }
    public decimal Sheets { get; set; }
    public string DueDate { get; set; }
    public string Notes { get; set; }
    public string Lot { get; set; }

    // Calculated properties
    public bool PatternExists { get; set; }
    public int? RecipeId { get; set; }
    public List<MaterialNeed> MaterialNeeds { get; set; } = new List<MaterialNeed>();
}

public class MaterialNeed
{
    public string MaterialName { get; set; }
    public decimal QuantityNeeded { get; set; }
    public string Unit { get; set; }
    public string MaterialType { get; set; }
    public decimal EstimatedCost { get; set; }
}

public class ProductionReport
{
    public DateTime GeneratedAt { get; set; }
    public int TotalOrders { get; set; }
    public int MatchedPatterns { get; set; }
    public int UnknownPatternsCount { get; set; } // Fixed: changed from UnknownPatterns
    public decimal TotalSquareFeet { get; set; }

    public List<ProductionOrder> Orders { get; set; } = new List<ProductionOrder>();
    public List<MaterialSummary> MaterialSummary { get; set; } = new List<MaterialSummary>();
    public List<string> UnknownPatternsList { get; set; } = new List<string>();
}

public class MaterialSummary
{
    public string MaterialName { get; set; }
    public decimal TotalQuantityNeeded { get; set; }
    public string Unit { get; set; }
    public string MaterialType { get; set; }
    public decimal TotalEstimatedCost { get; set; }
    public decimal CurrentInventory { get; set; }
    public decimal ShortfallQuantity { get; set; }
    public bool HasShortfall => ShortfallQuantity > 0;
}

public class ProductionCsvRecord
{
    [Name("\nCustomer")] // Handle the newline in the header
    public string Customer { get; set; }

    public string Pattern { get; set; }
    public string Colorway { get; set; }

    [Name("Sq Ft")]
    public decimal SqFt { get; set; }

    public decimal Sheets { get; set; }

    [Name("Due Date")]
    public string DueDate { get; set; }

    public string Notes { get; set; }

    [Name("Lot  ")] // Handle the extra spaces
    public string Lot { get; set; }
}