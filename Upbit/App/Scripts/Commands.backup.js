///////////////////////////
// START 
///////////////////////////

window.location.href = '$url';



///////////////////////////
// CHECK PRICE EXISTS (PAGE LOADED)
///////////////////////////
$('.askpriceB .scrollB>div>div>table>tbody>tr .bar>a>p').eq(9).text() != '';



///////////////////////////
// SET ELEMENTS
///////////////////////////
var au = {};

au.trigger = function (element, event) {
    let e = new Event(event, { bubbles: true });
    e.simulated = true;
    element.dispatchEvent(e);
};

// Menu
au.menu = $('.mainB>section.ty01 .titB.link');
au.menu.link = $('a.select', au.menu);
au.menu.pairs = $('p', au.menu.link);

// Modal
au.modal = $('#checkVerifMethodModal');

// Prices
au.prices = $('.askpriceB .scrollB>div>div>table>tbody>tr');
for (var i = 0; i < au.prices.length; i++) {
    au.prices[i].price = $('.downB>a>.ty01>em,.downB>a>.ty03>strong,.upB>a>.ty01>em,.upB>a>.ty03>strong', au.prices.eq(i));
    au.prices[i].usdPrice = $('.downB>a>.ty01>strong,.upB>a.ty01>strong', au.prices.eq(i));
    au.prices[i].link = $('.downB>a,.upB>a', au.prices.eq(i));
    au.prices[i].amount = $('.bar>a>p', au.prices.eq(i));
}

au.prices.buy = au.prices[9];
au.prices.sell = au.prices[10];

// purchase
au.order = $('.ty01 .halfB .rightB>article');
au.buytab = $('a[title="매수"]', au.order);
au.selltab = $('a[title="매도"]', au.order);
au.historytab = $('a[title="거래내역"]', au.order);

au.orderInputs = $('.orderB input.txt', au.order);
au.amountInput = au.orderInputs.eq(0);
au.priceInput = au.orderInputs.eq(1);
au.available = $('.orderB dd.price strong', au.order);

au.buyBtn = $('.orderB ul.btnB a[title="매도"]', au.order);
au.sellBtn = $('.orderB ul.btnB a[title="매수"]', au.order);

true;


///////////////////////////
// CHECK LOGIN
///////////////////////////

au.buyBtn.length ? true : false;



///////////////////////////
// GO TO COIN PAGE
///////////////////////////

au.trigger(au.menu.link[0], 'click');
au.trigger($("li a:contains('$pair')", au.menu)[0], 'click');


///////////////////////////
// GET PRICE
///////////////////////////
JSON.stringify({
    buy_price: au.prices.buy.price.text(),
    buyable_amount: au.prices.buy.amount.text(),
    sell_price: au.prices.sell.price.text(),
    sellable_amount: au.prices.sell.amount.text()
});



