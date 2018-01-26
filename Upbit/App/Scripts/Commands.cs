using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upbit.App.Scripts
{
    class Commands
    {
        // https://jscompress.com/ -> minify
        // https://www.freeformatter.com/json-escape.html#ad-output -> json escape

        public static string NavigateTo(string url)
        {
            return String.Format("window.location.href='{0}';", url);
        }

        public static string PageLoaded()
        {
            return "$(\".askpriceB .askpriceB .scrollB>div>div>table>tbody>tr .bar>a>p\")!=null&&$(\".askpriceB .scrollB>div>div>table>tbody>tr .bar>a>p\").eq(9).text()!='';";
        }

        public static string SetElements()
        {
            return "var au={};au.trigger=function(a,b){let c=new Event(b,{bubbles:!0});c.simulated=!0,a.dispatchEvent(c)},!0;";

        }

        public static string BuyButtonExists()
        {
            return "!!$('.ty01 .halfB .rightB>article .orderB ul.btnB a[title=\"\\uB9E4\\uB3C4\"]').length;";
        }

        
        public static string GetPriceKRW()
        {
            return "'__buffer__'+JSON.stringify({buyPrice:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.downB>a>.ty03>strong,.upB>a>.ty03>strong').text(),buyableAmount:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.bar>a>p').text(),sellPrice:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.downB>a>.ty03>strong,.upB>a>.ty03>strong').text(),sellableAmount:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.bar>a>p').text()});";
        }

        public static string GetPriceUSDT()
        {
            return "'__buffer__'+JSON.stringify({buyPrice:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.downB>a>.ty01>em,.upB>a>.ty01>em').text(),buyPriceUSDT:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.downB>a>.ty01>strong,.upB>a>.ty01>strong').text(),buyableAmount:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.bar>a>p').text(),sellPrice:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.downB>a>.ty01>em,.upB>a>.ty01>em').text(),sellableAmount:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.bar>a>p').text(),sellPriceUSDT:$('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.downB>a>.ty01>strong,.upB>a>.ty01>strong').text()});";
        }


        public static string CheckAmountAvailable(decimal amount)
        {
            return String.Format("!!(parseFloat($('.halfB .rightB .orderB dd.price strong').text().replace(',',''))>={0});", amount);
        }

        public static string ClickBuyTab()
        {
            return "au.trigger($('a[title=\"매수\"]')[0],'click');";
        }

        public static string ClickSellTab()
        {
            return "au.trigger($('a[title=\"매도\"]')[0],'click');";
        }

        public static string ClickPrice(int row)
        {
            return String.Format("au.trigger($('.askpriceB .scrollB>div>div>table>tbody>tr').eq({0}).find('.downB>a,.upB>a')[0],'click');", row);
        }

        public static string FillAmount(decimal amount)
        {
            return String.Format("$('.halfB .rightB .orderB input.txt:eq(0)').val({0});au.trigger($('.halfB .rightB .orderB input.txt:eq(0)')[0],'input');", amount); 
        }

        public static string ClickBuy()
        {
            return "au.trigger($('.halfB .rightB .orderB ul.btnB a[title=\"매수\"]')[0],'click');";
        }

        public static string ClickSell()
        {
            return "au.trigger($('.halfB .rightB .orderB ul.btnB a[title=\"매도\"]')[0],'click');";
        }

        public static string Buy(int row, decimal amount)
        {
            return String.Format("au.trigger($('.askpriceB .scrollB>div>div>table>tbody>tr').eq({0}).find('.downB>a,.upB>a')[0],'click');$('.halfB .rightB .orderB input.txt:eq(0)').val({1});au.trigger($('.halfB .rightB .orderB input.txt:eq(0)')[0],'input');au.trigger($('.halfB .rightB .orderB ul.btnB a[title=\"매수\"]')[0],'click');", row, amount);
        }

        public static string Sell(int row, decimal amount)
        {
            return String.Format("au.trigger($('.askpriceB .scrollB>div>div>table>tbody>tr').eq({0}).find('.downB>a,.upB>a')[0],'click');$('.halfB .rightB .orderB input.txt:eq(0)').val({1});au.trigger($('.halfB .rightB .orderB input.txt:eq(0)')[0],'input');au.trigger($('.halfB .rightB .orderB ul.btnB a[title=\"매도\"]')[0],'click');", row, amount);
        }


        public static string WaitConfirmBuy()
        {
            return "!!$('#checkVerifMethodModal a[title=\"매수확인\"]').length;";
        }

        public static string WaitConfirmSell()
        {
            return "!!$('#checkVerifMethodModal a[title=\"매도확인\"]').length;";
        }

        public static string ConfirmBuy()
        {
            return "var data='__buffer__'+JSON.stringify({Amount:$('#checkVerifMethodModal article>dl>div>dd:eq(1)>strong').text().replace(/[^\\d.]/g,''),Price:$('#checkVerifMethodModal article>dl>div>dd:eq(2)>strong').text().replace(/[^\\d.]/g,''),TotalCost:$('#checkVerifMethodModal article>dl>div>dd:eq(3)>strong').text().replace(/[^\\d.]/g,'')});au.trigger($('#checkVerifMethodModal a[title=\"매수확인\"]')[0],'click'),data;";
        }

        public static string ConfirmSell()
        {
            return "var data='__buffer__'+JSON.stringify({Amount:$('#checkVerifMethodModal article>dl>div>dd:eq(1)>strong').text().replace(/[^\\d.]/g,''),Price:$('#checkVerifMethodModal article>dl>div>dd:eq(2)>strong').text().replace(/[^\\d.]/g,''),TotalCost:$('#checkVerifMethodModal article>dl>div>dd:eq(3)>strong').text().replace(/[^\\d.]/g,'')});au.trigger($('#checkVerifMethodModal a[title=\"매도확인\"]')[0],'click'),data;";
        }

        public static string ConfirmOrderWait()
        {
            return "'확인'==$('#checkVerifMethodModal a').text();";
        }

        public static string ConfirmOrder()
        {
            return "au.trigger($('#checkVerifMethodModal a')[0],'click');";
        }








    }
}
