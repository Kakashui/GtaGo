let lastClick = Date.now();

function checkLastCheck() {
    if (Date.now() < lastClick + 1000)
        return false;
    lastClick = Date.now();
    return true;
}

mp.events.add('familyMenu:saveOrganizationName', (name) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:tryChangeName', name);
});

mp.events.add('familyMenu:saveNation', (nation) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:tryChangeNation', nation);
});

mp.events.add('familyMenu:setBio', (bio) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:tryChangeBiography', bio);
});

mp.events.add('familyMenu:saveEditFamilyRules', (tabooJson, rulesJson) => {
    if (!checkLastCheck())
        return;
    var taboo = JSON.parse(tabooJson);
    var rules = JSON.parse(rulesJson);
    mp.events.callRemote('family:tryChangeRules', JSON.stringify(taboo.map(item=> item.text)), JSON.stringify(rules.map(item=> item.text)));
});

mp.events.add('familyMenu:addNewRank', (rankId) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:tryCreateRank', rankId);
});

mp.events.add('familyMenu:deleteRank', (rankId) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:tryDeleteRank', rankId);
});

mp.events.add('familyMenu:setRank', (jsonRank) => {
    if (!checkLastCheck())
        return;
    let rank = JSON.parse(jsonRank);
    let accessCars = {};
    rank.accessCars.forEach(car => {
        accessCars[car.key] = car.access
    });
    mp.events.callRemote('family:trySetRank', rank.rankId, rank.rankName, rank.accessHouse, rank.accessFurniture, rank.accessClothes, rank.accessWar, rank.accessMembers, JSON.stringify(accessCars));
});

mp.events.add('familyMenu:saveChatOptions', (icon, color) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:trySetChatOptions', icon, color);
});

mp.events.add('familyMenu:setCurrentRank', (memberId, rankId) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:trySetCurrentRank', memberId, rankId);
});
mp.events.add('familyMenu:kickMember', (memberId) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:tryKickMember', memberId);
});
mp.events.add('familyMenu:leaveFromOrganization', () => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:leaveFromOrganization');
});
mp.events.add('familyMenu:deleteOrganization', () => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:deleteOrganization');
});



mp.events.add('familyMenu:selectCarOption', (iventKey, vehKey) => {
    if (!checkLastCheck())
        return;
    switch (iventKey) {
        case 0:
            mp.events.callRemote('family:repairCar', vehKey);
            break;
        case 1:
            mp.events.callRemote('family:evacCar', vehKey);
            break;
        case 2:
            mp.events.callRemote('family:sellCar', vehKey);
            break;
        case 3:
            mp.events.callRemote('vehicle::key::enableGPS', vehKey);
            break;
        case 4:
            mp.events.callRemote('family:transferCarToMe', vehKey);
            break;
    }
});

mp.events.add('familyMenu:setOnGps', (bizId) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:setOnGps', bizId);
});


mp.events.add('familyMenu:takeMoney', (amount) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:takeMoney', amount);
});

mp.events.add('familyMenu:putMoney', (amount) => {
    if (!checkLastCheck())
        return;
    mp.events.callRemote('family:putMoney', amount);
});

mp.events.add('familyMenu:pushRegBattle', (place, bizId, weapon, date, time, comment) => {
    mp.events.callRemote('family:createBattle', place, bizId, weapon, date, time, comment);
});

mp.events.add('familyMenu:acceptBattle', (battleId, accepted) => {
    mp.events.callRemote('family:acceptBattle', battleId, accepted);
});
