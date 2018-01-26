///////////////////////////
// START 
///////////////////////////
window.location.href = '$url';



///////////////////////////
// CHECK PRICE EXISTS (PAGE LOADED)
///////////////////////////
$('.askpriceB .askpriceB .scrollB>div>div>table>tbody>tr .bar>a>p')!=null && $('.askpriceB .scrollB>div>div>table>tbody>tr .bar>a>p').eq(9).text() != '';



///////////////////////////
// SET ELEMENTS
///////////////////////////
var au = {};

au.trigger = function (element, event) {
    let e = new Event(event, { bubbles: true });
    e.simulated = true;
    element.dispatchEvent(e);
};

true;


///////////////////////////
// CHECK LOGIN
///////////////////////////
$('.ty01 .halfB .rightB>article .orderB ul.btnB a[title="매도"]').length ? true : false;





///////////////////////////
// GET PRICE
///////////////////////////

// FOR KRW
'__buffer__' + JSON.stringify({
    buyPrice: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.downB>a>.ty03>strong,.upB>a>.ty03>strong').text(),
    buyableAmount: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.bar>a>p').text(),
    sellPrice: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.downB>a>.ty03>strong,.upB>a>.ty03>strong').text(),
    sellableAmount: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.bar>a>p').text()
});

// FOR USDT
'__buffer__' + JSON.stringify({
    buyPrice: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.downB>a>.ty01>em,.upB>a>.ty01>em').text(),
    buyPriceUSDT: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.downB>a>.ty01>strong,.upB>a>.ty01>strong').text(),
    buyableAmount: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(9).find('.bar>a>p').text(),
    sellPrice: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.downB>a>.ty01>em,.upB>a>.ty01>em').text(),
    sellableAmount: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.bar>a>p').text(),
    sellPriceUSDT: $('.askpriceB .scrollB>div>div>table>tbody>tr').eq(10).find('.downB>a>.ty01>strong,.upB>a>.ty01>strong').text()
});



///////////////////////////
// TRANSACTIONS
///////////////////////////

// CHECK AVAILABLE
parseFloat($('.halfB .rightB .orderB dd.price strong').text().replace(',','')) >= $amount ? true : false;

// BUY TAB
au.trigger($('a[title=\"매수\"]')[0], 'click');

// SELL TAB
au.trigger($('a[title=\"매도\"]')[0], 'click');

// PRICE CLICK
au.trigger($('.askpriceB .scrollB>div>div>table>tbody>tr').eq($row).find('.downB>a,.upB>a')[0], 'click');

// AMOUNT FILL
$('.halfB .rightB .orderB input.txt:eq(0)').val($val);
au.trigger($('.halfB .rightB .orderB input.txt:eq(0)')[0], 'input');

// BUY CLICK
au.trigger($('.halfB .rightB .orderB ul.btnB a[title=\"매수\"]')[0], 'click');

// SELL CLICK
au.trigger($('.halfB .rightB .orderB ul.btnB a[title=\"매도\"]')[0], 'click');

// CHECK CONFIRM BUY
$('#checkVerifMethodModal a[title=\"매수확인\"]').length ? true : false;

// CHECK CONFIRM SELL
$('#checkVerifMethodModal a[title=\"매도확인\"]').length ? true : false;

// GET ORDER INFO AND CONFIRM BUY
var data = '__buffer__' + JSON.stringify({
    amount: $('#checkVerifMethodModal article>dl>div>dd:eq(1)>strong').text().replace('/[^\d.]/', ''),
    price: $('#checkVerifMethodModal article>dl>div>dd:eq(2)>strong').text().replace('/[^\d.]/', ''),
    totalCost: $('#checkVerifMethodModal article>dl>div>dd:eq(3)>strong').text().replace('/[^\d.]/', '')
});
au.trigger($('#checkVerifMethodModal a[title=\"매수확인\"]')[0], 'click');
data;

// GET ORDER INFO AND CONFIRM SELL
var data = '__buffer__' + JSON.stringify({
    Amount: $('#checkVerifMethodModal article>dl>div>dd:eq(1)>strong').text().replace('/[^\d.]/', ''),
    Price: $('#checkVerifMethodModal article>dl>div>dd:eq(2)>strong').text().replace('/[^\d.]/', ''),
    TotalCost: $('#checkVerifMethodModal article>dl>div>dd:eq(3)>strong').text().replace('/[^\d.]/', '')
});
au.trigger($('#checkVerifMethodModal a[title=\"매도확인\"]')[0], 'click');
data;

// CONFIRM ORDER WAIT
$('#checkVerifMethodModal a').text() == '확인';

// CONFIRM
au.trigger($('#checkVerifMethodModal a')[0], 'click');


