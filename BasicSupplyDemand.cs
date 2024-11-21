namespace JHC_Market_Cap___Basic_Supply_and_Demand
{
    public class BasicSupplyDemand
    {
        public static void RunBasicSupplyDemand()
        {
            decimal initialPrice = 1.0m; // Initial price in USD
            int totalSupply = 10_000; // Total supply cap
            int availableCoins = totalSupply; // Coins available for purchase
            decimal adjustmentPerCoin = 0.001m; // 0.01% price change per coin bought/sold

            Console.WriteLine("=== Welcome to JHC Coin Market Simulator ===");
            Console.WriteLine($"Initial Price: ${initialPrice}, Total Supply: {totalSupply} coins\n");

            while (true)
            {
                Console.WriteLine($"Available Coins: {availableCoins}/{totalSupply}");
                Console.WriteLine("Enter transaction type (buy/sell) and amount of coins (e.g., 'buy 10' or 'sell 5'):");

                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                string[] parts = input.Split(' ');
                if (parts.Length != 2 || !int.TryParse(parts[1], out int amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }

                string action = parts[0].ToLower();
                if (action == "buy")
                {
                    if (amount > availableCoins)
                    {
                        Console.WriteLine("Not enough coins available to buy.");
                        continue;
                    }

                    availableCoins -= amount; // Reduce available coins
                    Console.WriteLine($"You bought {amount} coins.");
                }
                else if (action == "sell")
                {
                    if (amount > (totalSupply - availableCoins))
                    {
                        Console.WriteLine("Cannot sell more than the coins you own.");
                        continue;
                    }

                    availableCoins += amount; // Add coins back to availability
                    Console.WriteLine($"You sold {amount} coins.");
                }
                else
                {
                    Console.WriteLine("Invalid action. Use 'buy' or 'sell'.");
                    continue;
                }

                // Update price based on current supply
                decimal currentPrice = AdjustPrice(initialPrice, totalSupply, availableCoins, adjustmentPerCoin);

                // Recalculate market cap
                decimal marketCap = currentPrice * totalSupply;

                // Display updated stats
                Console.WriteLine($"\nUpdated Price: ${currentPrice:F2}");
                Console.WriteLine($"Market Cap: ${marketCap:F2}");
                Console.WriteLine($"Available Coins: {availableCoins}/{totalSupply}\n");
            }
        }

        static decimal AdjustPrice(decimal initialPrice, int totalSupply, int availableCoins, decimal adjustmentPerCoin)
        {
            // Calculate the number of coins sold
            int coinsSold = totalSupply - availableCoins;

            // Calculate the adjustment factor based on coins sold
            decimal adjustmentFactor = adjustmentPerCoin * coinsSold;

            // Calculate the new price
            decimal newPrice = initialPrice * (1 + adjustmentFactor);

            // Enforce a minimum price floor
            const decimal minimumPrice = 0.01m;
            return newPrice < minimumPrice ? minimumPrice : newPrice;
        }

    }
}
