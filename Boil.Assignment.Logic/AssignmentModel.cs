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
    public int MaximumMiddlemanProfit { get; set; }
    
    //Tabela na optymalne plany przewozów dla każdej iteracji i dla ustawienia początkowego. Finalna tabela zostanie wypisana.
    // id odpowiadają tabeli UnitProfit
    public int[,] OptimalTransportPlan { get; set; }

    public AssignmentModel(string[,] transportCost, string[] demand, string[] supply, 
        string[] purchaseCost, string[] sellingCost, int supplierCount, int recipientCount)
    {
        SupplierCount = supplierCount + 1; // plus 1 żeby było miejsce na fikcyjnych
        RecipientCount = recipientCount + 1;
        
        TransportCost = new int[supplierCount,recipientCount];
        Demand = new int[RecipientCount];
        Supply = new int[SupplierCount];
        PurchaseCost = new int[supplierCount];
        SellingCost = new int[recipientCount];
        UnitProfit = new int[SupplierCount, RecipientCount];
        OptimalTransportPlan = new int[SupplierCount, RecipientCount];
        

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
        for (int i = 0; i < SupplierCount - 1; i++)
        {
            for (int j = 0; j < RecipientCount - 1; j++)
            {
                UnitProfit[i, j] = SellingCost[j] - (PurchaseCost[i] + TransportCost[i, j]);
            }
        }
    }

    public void CalculateFictional()
    {
        var fullSupply = 0;
        var fullDemand = 0;
        
        for (int i = 0; i < SupplierCount - 1 ; i++)
        {
            fullSupply += Supply[i];
        }

        for (int i = 0; i < RecipientCount - 1; i++)
        {
            fullDemand += Demand[i];
        }

        Supply[SupplierCount - 1] = fullDemand;
        Demand[RecipientCount - 1] = fullSupply;
    }
}