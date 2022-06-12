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
        Alpha = new int?[SupplierCount];
        Beta = new int?[RecipientCount];
        MaximumMiddlemanProfit = 0;

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
    public void NorthWestCornerMethod()
    {
        var demand = (int[])Demand.Clone();
        var supply = (int[])Supply.Clone();
        for(int xi = 0; xi < SupplierCount; xi++) {
            for(int yi = 0; yi < RecipientCount; yi++) {
                int quantity = Math.Min(demand[yi], supply[xi]);
                if(quantity > 0) {
                    OptimalTransportPlan[xi, yi] = quantity;
                    demand[yi] -= quantity;
                    supply[xi] -= quantity;
                }
            }
        }
    }

    public void CalculateAlphaBeta()
    {
        var alpha = (int?[])Alpha.Clone();
        var beta = (int?[])Beta.Clone();

        var isValue = true;

        foreach (var t in Alpha)
        {
            if (t is null)
                isValue = false;
        }
        
        foreach (var t in Beta)
        {
            if (t is null)
                isValue = false;
        }

        if (isValue) return;

        for (int i = 0; i < Alpha.Length; i++)
        {
            for (int j = 0; j < Beta.Length; j++)
            {
                if (OptimalTransportPlan[i,j] > 0)
                {
                    if (Beta[j] is null && Alpha[i] is not null)
                    {
                        if (j >= UnitProfit.GetLength(0) || i >= UnitProfit.GetLength(1))
                            Beta[j] = 0 - Alpha[i];
                        else
                            Beta[j] = UnitProfit[i, j] - Alpha[i];
                    }
                    else if (Beta[j] is not null && Alpha[i] is null)
                    {
                        if (i >= UnitProfit.GetLength(1) || j >= UnitProfit.GetLength(0))
                            Alpha[i] = 0 - Beta[j];
                        else
                            Alpha[i] = UnitProfit[i, j] - Beta[j];
                    }
                }
            }
        }

        if (alpha.SequenceEqual(Alpha) && beta.SequenceEqual(Beta))
        {
            for (int i = 0; i < Alpha.Length; i++)
            {
                if (Alpha[i] is null)
                {
                    Alpha[i] = 0;
                    break;
                }
            }
        }
        
        CalculateAlphaBeta();
    }
    
    public void print_matrix(int?[,] matrix, string text) {
        Console.WriteLine(text);
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (matrix[i,j] is null)
                    Console.Write("NULL | ");
                else
                    Console.Write($"{matrix[i, j]} | ");
            }
            Console.WriteLine();
        }
    }
    public ((int, int), (int, int), (int, int), (int, int)) FindCycle() {

        // Create new matrix with values for finding cycle
        int?[,] cycleMatrix = new int?[SupplierCount, RecipientCount];

        // Reinicialize aplha and beta
        Alpha = new int?[SupplierCount];
        Beta = new int?[RecipientCount];

        // Calculate alpha and beta values
        this.CalculateAlphaBeta();

        // Fill up new matrix with values
        for(int xi = 0; xi < SupplierCount; xi++) {
            for(int yi = 0; yi < RecipientCount; yi++) {

                // Initialize only values with no transportation
                if(OptimalTransportPlan[xi ,yi] == 0) {
                    cycleMatrix[xi, yi] = UnitProfit[xi, yi] - Alpha[xi].GetValueOrDefault() - Beta[yi].GetValueOrDefault();
                }
            }
        }

        print_matrix(cycleMatrix, "cycleMatrix:");

        // Find the greatest value in matrix
        var (x, y) = FindMaxElementPos(cycleMatrix);

        Console.WriteLine("FindMaxElementPos:\n(" + x + ", " + y + ") = " + cycleMatrix[x, y]);

        // Check wheter the matrix is complete
        if(cycleMatrix[x, y] < 0) {
            return ((-1, -1), (-1, -1), (-1, -1), (-1, -1));
        }

        Console.WriteLine("FindCycle:");

        // Iterate threw matrix in order to find cycle
        for(int xi = 0; xi < SupplierCount; xi++) {
            for(int yi = 0; yi < RecipientCount; yi++) {

                Console.WriteLine("(" + xi + ", " + yi + ") = " + (cycleMatrix[xi, yi] == null ? "null" : cycleMatrix[xi, yi]));
                Console.WriteLine("(" + x + ", " + y + ") = " + (cycleMatrix[x, y] == null ? "null" : cycleMatrix[x, y]));
                Console.WriteLine("(" + x + ", " + yi + ") = " + (cycleMatrix[x, yi] == null ? "null" : cycleMatrix[x, yi]));
                Console.WriteLine("(" + xi + ", " + y + ") = " + (cycleMatrix[xi, y] == null ? "null" : cycleMatrix[xi, y]) + "\n");

                // If value is on the same row, column or value is not null - skip
                if(x == xi || y == yi || cycleMatrix[xi, yi] != null) {
                    Console.WriteLine("BREAK!");
                    continue;
                }

                // If can create cycle - return coordinates of cycle edges in matrix
                if(cycleMatrix[x, yi] == null && cycleMatrix[xi, y] == null) {
                    return ((x, y), (xi, yi), (x, yi), (xi, y));
                }
            }
        }

        // If no cycle has been foun return -1
        return ((-1, -1), (-1, -1), (-1, -1), (-1, -1));
    }

    public bool DoCycle() {

        // Get coordinates of cycle edges in matrix
        var ((x1, y1), (x2, y2), (x3, y3), (x4, y4)) = FindCycle();

        Console.WriteLine("Cycle:\n[{0}]", string.Join(", ", x1, x2, x3, x4));
        Console.WriteLine("[{0}]", string.Join(", ", y1, y2, y3, y4));

        // Check wheter any cycle has been found
        if (x1 == -1) { return false; }

        // Get minimal value in cycle
        int MinValue = (new int[] {OptimalTransportPlan[x1, y1], OptimalTransportPlan[x2, y2], OptimalTransportPlan[x3, y3], OptimalTransportPlan[x4, y4]}).Where(val => val > 0).Min();

        // Carry out the cycle
        OptimalTransportPlan[x1, y1] += MinValue;
        OptimalTransportPlan[x2, y2] += MinValue;
        OptimalTransportPlan[x3, y3] -= MinValue;
        OptimalTransportPlan[x4, y4] -= MinValue;

        return true;
    }

    public void CalculateMaximumProfit()
    {
        for (int i = 0; i < SupplierCount; i++)
        {
            for (int j = 0; j < RecipientCount; j++)
            {
                if (OptimalTransportPlan[i, j] > 0)
                {
                    MaximumMiddlemanProfit += OptimalTransportPlan[i, j] * UnitProfit[i, j];
                }
                    
            }
        }
    }
}