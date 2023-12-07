const { pointsConfig, plantsConfig, pitsConfig, fertilizersConfig, productConfigs } = require('./config');
let opened = false;

mp.events.add('farm::openMenu', (experience, productsJSON) => {
    if (global.checkIsAnyActivity()) return;
    let seeds = [];
    let fertilizers = [];
    let others = [];
    let products = JSON.parse(productsJSON);
    let plantsConfigArray = Object.values(plantsConfig);
    products.forEach(product => {
        let plant = plantsConfigArray.find(item => item.Name == product.Name);
        if (plant) {
            seeds.push({
                id: product.Name,
                minTime: Math.floor(plant.RipeningTime / 60),
                countFetus: plant.CountFetus,
                exp: plant.Exp,
                title: `item_${product.Name.toLowerCase()}`,
                cost: product.Price,
                img: product.Name.toLowerCase()
            });
            return;
        }
        let prodConf = productConfigs[product.Name]
        if (prodConf) {
            if (prodConf.Type == 'Fert') {
                fertilizers.push({
                    id: product.Name,
                    minTime: prodConf.TimeBonus,
                    exp: prodConf.Experience,
                    title: `item_${product.Name.toLowerCase()}`,
                    cost: product.Price,
                    img: product.Name.toLowerCase()
                });
            }
            else {
                others.push({
                    id: product.Name,
                    param: prodConf.Parametr,
                    paramType: prodConf.ParametrType,
                    title: `item_${product.Name.toLowerCase()}`,
                    cost: product.Price,
                    img: product.Name.toLowerCase()
                });
            }
        }
    });
    opened = global.gui.openPage('FarmHouse');
    global.gui.setData('farmHouse/setSeedsList', JSON.stringify(seeds));
    global.gui.setData('farmHouse/setNeedfulList', JSON.stringify(others));
    global.gui.setData('farmHouse/setFertilizersList', JSON.stringify(fertilizers));
    global.gui.setData('farmHouse/setExp', JSON.stringify(experience));
    global.gui.setData('farmHouse/setNickname', JSON.stringify(mp.players.local.name));


});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});

mp.events.add('farm::closeMenu', () => {
    if (opened)
        CloseMenu();
});

function CloseMenu() {
    global.gui.close();
    opened = false;
    mp.events.callRemote('farmHouse:closeMenu')
}
