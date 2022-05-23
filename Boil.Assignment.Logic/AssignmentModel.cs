namespace Boil.Assignment.Logic;

public class AssignmentModel
{
    public int[,] TransportCost { get; set; }
    public int[] Demand { get; set; }
    public int[] Supply { get; set; }
    public int[] PurchaseCost { get; set; }
    public int[] SellingCost { get; set; }
    public int SupplierCount { get; set; }
    public int RecipientCount { get; set; }
    public int[,] UnitProfit { get; set; }

    public AssignmentModel(string[,] transportCost, string[] demand, string[] supply, 
        string[] purchaseCost, string[] sellingCost, int supplierCount, int recipientCount)
    {
        TransportCost = new int[supplierCount,recipientCount];
        Demand = new int[recipientCount];
        Supply = new int[supplierCount];
        PurchaseCost = new int[supplierCount];
        SellingCost = new int[recipientCount];
        UnitProfit = new int[supplierCount, recipientCount];

        SupplierCount = supplierCount;
        RecipientCount = recipientCount;
        
        for (int i = 0; i < supplierCount; i++)
        {
            for (int j = 0; j < recipientCount; j++)
            {
                TransportCost[i, j] = int.Parse(transportCost[i, j]);
            }
        }

        for (int i = 0; i < supplierCount; i++)
        {
            Supply[i] = int.Parse(supply[i]);
            PurchaseCost[i] = int.Parse(purchaseCost[i]);
        }

        for (int i = 0; i < recipientCount; i++)
        {
            Demand[i] = int.Parse(demand[i]);
            SellingCost[i] = int.Parse(sellingCost[i]);
        }
    }
    
    public void CalculateUnitProfit()
    {
        for (int i = 0; i < SupplierCount; i++)
        {
            for (int j = 0; j < RecipientCount; j++)
            {
                UnitProfit[i, j] = SellingCost[j] - (PurchaseCost[i] + TransportCost[i, j]);
            }
        }
    }
}