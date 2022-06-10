namespace Boil.Assignment.Logic;

public class AssignmentModel
{
    // Init data
    public int[,] TransportCost { get; set; }
    public int[] Demand { get; set; }
    public int[] Supply { get; set; }
    public int[] PurchaseCost { get; set; }
    public int[] SellingCost { get; set; }
    public int SupplierCount { get; set; }
    public int RecipientCount { get; set; }
    public int[,] UnitProfit { get; set; }
    // Calculation data
    public int?[] Alpha { get; set; }
    public int?[] Beta { get; set; }
    // CHUJ WIE CO TO ZA DATA
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
        Alpha = new int?[RecipientCount];
        Beta = new int?[SupplierCount];

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

    public (int, int) FindMaxElementPos(int?[,] matrix) {

        var max_value = matrix.Cast<int?>().Max();
        
        for(int xi = 0; xi < SupplierCount; xi++) {
            for(int yi = 0; yi < RecipientCount; yi++) {

                if(matrix[xi, yi].Equals(max_value)) {

                    return (xi, yi);
                }
            }
        }

        return (-1, -1);
    }
    public void NorthWestCornerMethod() {
        for(int xi = 0; xi < SupplierCount; xi++) {
            for(int yi = 0; yi < RecipientCount; yi++) {
                int quantity = Math.Min(Demand[yi], Supply[xi]);
                if(quantity > 0) {
                    OptimalTransportPlan[xi, yi] = quantity;
                    Demand[yi] -= quantity;
                    Supply[xi] -= quantity;
                }
            }
        }
    }

    public void CalculateAlphaBeta() {
        // NIEWIEM KURWA JAK TO NAPISAC
    }

    public ((int, int), (int, int), (int, int), (int, int)) FindCycle() {
        int?[,] cycleMatrix = new int?[SupplierCount, RecipientCount];
        for(int xi = 0; xi < SupplierCount; xi++) {
            for(int yi = 0; yi < RecipientCount; yi++) {
                if(OptimalTransportPlan[xi ,yi] == 0) {
                    cycleMatrix[xi, yi] = UnitProfit[xi, yi] - Alpha[yi].GetValueOrDefault() - Beta[xi].GetValueOrDefault();
                }
            }
        }
        var (x, y) = FindMaxElementPos(cycleMatrix);
        if(cycleMatrix[x, y] < 0) {
            return ((-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }
        for(int xi = 0; xi < SupplierCount; xi++) {
            for(int yi = 0; yi < RecipientCount; yi++) {
                if(x == xi || y == yi || cycleMatrix[xi, yi] != null) {
                    break;
                }
                if(cycleMatrix[x, yi] == null && cycleMatrix[xi, y] == null) {
                    return ((x, y), (xi, yi), (x, yi), (xi, y));
                }
            }
        }
        return ((-1, -1), (-1, -1), (-1, -1), (-1, -1));
    }

    public void DoCycle() {
        var ((x1, y1), (x2, y2), (x3, y3), (x4, y4)) = FindCycle();
        if (x1 == -1) { return; }
        int MinValue = (new int[] {OptimalTransportPlan[x1, y1], OptimalTransportPlan[x2, y2], OptimalTransportPlan[x3, y3], OptimalTransportPlan[x4, y4]}).Min();
        OptimalTransportPlan[x1, y1] += MinValue;
        OptimalTransportPlan[x2, y2] += MinValue;
        OptimalTransportPlan[x3, y3] -= MinValue;
        OptimalTransportPlan[x4, y4] -= MinValue;
    }
}