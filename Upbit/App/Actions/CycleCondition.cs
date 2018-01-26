using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upbit.App.Models;

namespace Upbit.App.Actions
{
    class CycleCondition
    {
        public static CycleCoins FindCycleCoins(List<string> coins, Dictionary<string, Price> prices)
        {
            decimal premium = -100;
            decimal discount = 100;

            Coin premiumCoin = null;
            Coin discountCoin = null;
            decimal profit;

            foreach ( string coinName in coins)
            {
                Coin coin = new Coin();
                coin.Name = coinName;
                coin.UsdtPrice = prices[coinName + "/USDT"];
                coin.KrwPrice = prices[coinName + "/KRW"];
                coin.PremiumRate = coin.KrwPrice.SellPrice / coin.UsdtPrice.BuyPrice - 1;
                coin.DiscountRate = 1 - coin.UsdtPrice.SellPrice / coin.KrwPrice.BuyPrice;

                if(coin.PremiumRate > premium)
                {
                    premium = coin.PremiumRate;
                    premiumCoin = coin;                    
                }

                if(coin.DiscountRate < discount)
                {
                    discount = coin.DiscountRate;
                    discountCoin = coin;
                }
            }

            profit = (1 + premium) * (1 - discount) - 1;

            return new CycleCoins()
            {
                PremiumCoin = premiumCoin,
                DiscountCoin = discountCoin,
                Profit = profit
            };
        }

        public static bool CheckSameCoinCondition(CycleCoins cycleCoins)
        {
            return cycleCoins.PremiumCoin.Name != cycleCoins.DiscountCoin.Name;

        }

        public static bool CheckProfitCondition(CycleCoins cycleCoins, decimal profitThreshold)
        {
            return cycleCoins.Profit >= profitThreshold;
        }

        public static bool CheckAmountCondition(CycleCoins cycleCoins, decimal availablityMultiplier)
        {
            decimal premiumCoinAmount = cycleCoins.PremiumCoinAmount;
            decimal discountCoinAmount = cycleCoins.DiscountCoinAmount;

            decimal premiumBuyableAmount = cycleCoins.PremiumBuyableAmount;
            decimal premiumSellableAmount = cycleCoins.PremiumSellableAmount;
            decimal discountBuyableAmount = cycleCoins.DiscountBuyableAmount;
            decimal discountSellableAmount = cycleCoins.DiscountSellableAmount;


            return premiumBuyableAmount >= premiumCoinAmount * availablityMultiplier &&
                premiumSellableAmount >= premiumCoinAmount * availablityMultiplier &&
                discountBuyableAmount >= discountCoinAmount * availablityMultiplier &&
                discountSellableAmount >= discountCoinAmount * availablityMultiplier;
        }
    }
}
