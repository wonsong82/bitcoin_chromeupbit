using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Models
{
    class CycleCoins
    {
        public Coin PremiumCoin;
        public Coin DiscountCoin;
        public decimal Profit;

        public decimal PremiumCoinAmount;
        public decimal DiscountCoinAmount;

        public decimal PremiumBuyableAmount;
        public decimal PremiumSellableAmount;
        public decimal DiscountBuyableAmount;
        public decimal DiscountSellableAmount;

        public void CalculateAmounts(decimal cycleMoney)
        {
            PremiumCoinAmount = cycleMoney / PremiumCoin.UsdtPrice.BuyPriceUSDT;
            DiscountCoinAmount = (PremiumCoinAmount * PremiumCoin.KrwPrice.SellPrice) / DiscountCoin.KrwPrice.BuyPrice;

            PremiumBuyableAmount = PremiumCoin.UsdtPrice.BuyableAmount;
            PremiumSellableAmount = PremiumCoin.KrwPrice.SellableAmount;
            DiscountBuyableAmount = DiscountCoin.KrwPrice.BuyableAmount;
            DiscountSellableAmount = DiscountCoin.UsdtPrice.SellableAmount;
        }
    }


    class Coin
    {
        public string Name;
        public Price UsdtPrice;
        public Price KrwPrice;
        public decimal PremiumRate;
        public decimal DiscountRate;
    }
}
