namespace JHC_Market_Cap___Basic_Supply_and_Demand
{
    public class BasicSupplyDemandWithVolumeCycle
    {
        public static void RunBasicSupplyDemandWithVolumeCycle()
        {
            decimal price = 1.0m; // Initial price in USD
            int totalSupply = 500; // Total supply cap
            int availableCoins = totalSupply; // Coins available for purchase
            int marketVolume = 50; // Maximum coins that can be traded in one cycle
            decimal marketCap = price * totalSupply;

            Console.WriteLine("=== Welcome to JHC Coin Market Simulator ===");
            Console.WriteLine($"Initial Price: ${price}, Total Supply: {totalSupply} coins\n");

            while (true)
            {
                Console.WriteLine($"Available Coins: {availableCoins}/{totalSupply}");
                Console.WriteLine($"Market Volume (Max Tradeable Coins): {marketVolume}");
                Console.WriteLine("Enter transaction type (buy/sell) and amount of coins (e.g., 'buy 10' or 'sell 5'):");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                string[] parts = input.Split(' ');
                if (parts.Length != 2 || !int.TryParse(parts[1], out int amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }

                if (amount > marketVolume)
                {
                    Console.WriteLine($"Transaction exceeds market volume! You can only trade up to {marketVolume} coins per cycle.");
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

                    price = AdjustPrice(price, amount, true); // Increase price
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

                    price = AdjustPrice(price, amount, false); // Decrease price
                    availableCoins += amount; // Add coins back to availability
                    Console.WriteLine($"You sold {amount} coins.");
                }
                else
                {
                    Console.WriteLine("Invalid action. Use 'buy' or 'sell'.");
                    continue;
                }

                // Recalculate market cap
                marketCap = price * totalSupply;

                // Display updated stats
                Console.WriteLine($"\nUpdated Price: ${price:F2}");
                Console.WriteLine($"Market Cap: ${marketCap:F2}");
                Console.WriteLine($"Available Coins: {availableCoins}/{totalSupply}");
                Console.WriteLine($"Market Volume Remaining: {marketVolume - amount} coins\n");

                // Reduce market volume for this cycle
                marketVolume -= amount;

                // Reset market volume every cycle (for simplicity)
                if (marketVolume <= 0)
                {
                    Console.WriteLine("Market volume has been reset for the next cycle.\n");
                    marketVolume = 50; // Reset volume for the next cycle
                }
            }
        }

        private static decimal AdjustPrice(decimal currentPrice, int amount, bool isBuy)
        {
            // Adjust price by 1% per coin bought/sold
            decimal adjustmentFactor = 0.01m * amount;
            return isBuy ? currentPrice * (1 + adjustmentFactor) : currentPrice * (1 - adjustmentFactor);
        }
    }
}
