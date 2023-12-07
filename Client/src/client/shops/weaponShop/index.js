const WeaponShop = require('./WeaponShop');

let shop = new WeaponShop();

mp.events.add('wshop:open', (prices) => {
    shop.open(prices);
})

mp.events.add('cef:wshop:close', () => {
    shop.close();
})

mp.events.add('cef:wshop:update:model', (id, slot, compIndex, model)=>{
    shop.update(id, slot, compIndex, model);
});

mp.events.add('wshopbuy', (cat, id, val) => {
    if (global.lastCheck > Date.now()) return;
    global.lastCheck = Date.now() + 500;
    mp.events.callRemote('wshopbuy', cat, id, val);
})

mp.events.add('wshop:move:x', (val) => {
    shop.moveX(val);
})

mp.events.add('wshop:move:y', (val) => {
    shop.moveY(val);
})

mp.events.add('wshop:move:z', (val) => {
    shop.moveZ(val);
})

mp.events.add('cef:wshop:buy', (count, payCard)=>{
    shop.buy(count, payCard);
})

mp.keys.bind(global.Keys.Key_ESCAPE, false, ()=>{
    shop.close();
});