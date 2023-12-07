
let familyCars = [];

mp.events.add('family:loadCarsAndRanks', (cars, ranks) => {
    familyCars = JSON.parse(cars);
    var familyRanks = JSON.parse(ranks);
    familyRanks.forEach(rank => {
        loadRankVehicleAccess(rank);
    });
    global.gui.setData('familyMenu/controlPage/setRanksList', JSON.stringify(familyRanks));
});

function loadRankVehicleAccess(rank) {
    if (rank.accessCars == undefined)
        rank.accessCars = [];
    let rankAccessCars = rank.accessCars;
    rank.accessCars = [];
    familyCars.forEach(car => {
        var index = rankAccessCars.findIndex(item => item.key == car.key);
        if (index >= 0) {
            rank.accessCars.push({ key: rankAccessCars[index].key, access: rankAccessCars[index].access, carName: `${car.model} (${car.number})` });
        }
        else {
            rank.accessCars.push({ key: car.key, access: rank.rankId > 0 ? 0 : 3, carName: `${car.model} (${car.number})` });
        }
    });
}

mp.events.add('family:errorName', () => {
    global.gui.setData('familyMenu/controlPage/setErrorName', JSON.stringify(true));
});

mp.events.add('family:newNation', (nation) => {
    global.gui.setData('familyMenu/setNation', JSON.stringify(nation));
});

mp.events.add('family:newBiography', (biography) => {
    global.gui.setData('familyMenu/setBiography', JSON.stringify(biography));
});

mp.events.add('family:newRules', (tabooJson, rulesJson) => {
    global.gui.setData('familyMenu/setRules', JSON.stringify({taboo: JSON.parse(tabooJson), rules: JSON.parse(rulesJson)}));
});

mp.events.add('family:updateRank', (jsonRank) => {
    let rank = JSON.parse(jsonRank);
    loadRankVehicleAccess(rank);
    global.gui.setData('familyMenu/controlPage/updateRank', JSON.stringify(rank));
});

mp.events.add('family:deleteRank', (rankId) => {
    global.gui.setData('familyMenu/controlPage/removeRank', JSON.stringify(rankId));
});

mp.events.add('family:updateChat', (icon, color) => {
    global.gui.setData('familyMenu/controlPage/setChatOptions', JSON.stringify({
        currentColor: color,
        currentIcon: icon
    }));
});

mp.events.add('family:updateVehicle', (jsonCar) => {
    let car = JSON.parse(jsonCar);
    let carData = { key: car.key, access: 0, carName: `${car.model} (${car.number})` };
    let index = familyCars.findIndex(item => item.key == car.key);
    if (index > -1)
        familyCars[index] = car;
    else
        familyCars.push(car);
    global.gui.setData('familyMenu/controlPage/updateCar', JSON.stringify(carData));
});

mp.events.add('family:removeVehicle', (carId) => {
    let index = familyCars.findIndex(item => item.key == carId);
    if (index > -1)
        familyCars.splice(index, 1);
    global.gui.setData('familyMenu/controlPage/removeCar', JSON.stringify(carId));
});

mp.events.add('family:updateMemberRank', (rank) => {
    global.gui.setData('familyMenu/setRank', rank);
});
mp.events.add('family:updateMember', (member) => {
    global.gui.setData('familyMenu/membersPage/updateMember', member);
});

mp.events.add('family:removeMember', (member) => {
    global.gui.setData('familyMenu/membersPage/removeMember', member);
});

mp.events.add('family:updateBusiness', (business) => {
    global.gui.setData('familyMenu/propertyPage/updateProperty', business);
});

mp.events.add('family:removeBusiness', (bizId) => {
    global.gui.setData('familyMenu/propertyPage/removeProperty', JSON.stringify(bizId));
});

mp.events.add('family:updateBusinessFamPatronage', (bizId, famName) => {
    global.gui.setData('familyMenu/eventsPage/updateBusinessFamilyPatronage', JSON.stringify({biz: bizId, familyName: famName}));
});

mp.events.add('family:updateFamilyRating', (familId, name, ownerName, countBattles, countBusiness, membersCount, eloRating, rank) => {
    global.gui.setData('familyMenu/ratingPage/updateOrganization', JSON.stringify({id: familId, name: name, owner: ownerName, victories: countBattles, buissCount: countBusiness, membersCount: membersCount, rating: eloRating, rank: rank}));
});

mp.events.add('family:updateMoney', (money) => {
    global.gui.setData('familyMenu/setBankBalance', JSON.stringify(money));
});

mp.events.add('family:createBattleResponse', (message, confirm) => {
    global.gui.setData('familyMenu/eventsPage/setNotificationMessage', JSON.stringify({message: message, result: confirm ? 'createBattle_ResultOk' : 'createBattle_ResultCancel'}));
});

mp.events.add('family:updateBattleData', (data) => {
    global.gui.setData('familyMenu/battlePage/updateBattleData', data);
});



mp.events.add('family:loadMP', (data) => {
    global.gui.setData('familyMenu/eventsPage/loadGlobalEvents', data);
});
mp.events.add('family:updateFamilyMP', (data) => {
    global.gui.setData('familyMenu/eventsPage/updateGlobalEvents', data);
});

