let opened = false;
let inFamily = false;

mp.keys.bind(global.Keys.Key_K, false, function () {
    if (!global.loggedin) return;
    if (global.checkIsAnyActivity()) return;
    if (!opened)
        mp.events.callRemote('family:openFamilyMenu');
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        CloseMenu();
});

mp.events.add('family:closeMenu', () => {
    CloseMenu();
});
mp.events.add('family:openMenu', (money) => {
    global.gui.setData('familyMenu/setBankBalance', JSON.stringify(money));
    if (!opened)
        OpenFamilyMenu()
});


function CloseMenu() {
    global.gui.close();
    opened = false;
}
function OpenFamilyMenu() {
    if (!global.loggedin) return;
    if (global.checkIsAnyActivity()) return;
    if (!inFamily)
        return;
    global.gui.openPage('FamilyMenu');
    opened = true;
}

mp.events.add('family:loadData', (data, rank, chatIcon, chatColor, members, name, businesses, allBiz, myUUID) => {
    global.gui.setData('familyMenu/setInfoPage', data);
    global.gui.setData('familyMenu/setRank', rank);
    global.gui.setData('familyMenu/setCurrentMemberId', myUUID);
    global.gui.setData('familyMenu/controlPage/setChatOptions', JSON.stringify({
        currentColor: chatColor,
        currentIcon: chatIcon
    }));
    global.showFamMembersOnMinimup = mp.storage.data.showFamMembersOnMinimup;
    global.gui.setData('familyMenu/setMembersOnMap', JSON.stringify(global.showFamMembersOnMinimup));
    global.gui.setData('familyMenu/membersPage/setMembers', members);
    global.gui.setData('familyMenu/controlPage/setOrganizationName', JSON.stringify(name));
    global.gui.setData('familyMenu/propertyPage/setPropertyList', businesses);
    global.gui.setData('familyMenu/eventsPage/setBusinessList', allBiz);
    inFamily = true;
    global.gui.setData('familyMenu/setInFamily', inFamily);
});


mp.events.add('family:loadFamilyBattles', (battles) => {
    global.gui.setData('familyMenu/battlePage/loadBattles', battles);
});

mp.events.add('family:unloadData', () => {
    inFamily = false;
    global.gui.setData('familyMenu/setInFamily', inFamily);
});


mp.events.add('family:loadFamilyRatings', (ratings) => {
    global.gui.setData('familyMenu/ratingPage/setOrgList', ratings);

});

mp.events.add('familyMenu:editHouse', () => {
    CloseMenu();
    mp.events.callRemote('house:openFamilyHouseMenu');
});


let openedCompanyCapt = false;

mp.keys.bind(global.Keys.Key_OEM_6, false, function () {
    if (!global.loggedin) return;
    if (global.checkIsAnyActivity()) return;
    let family = global.getVariable(mp.players.local, 'familyuuid', -1);
    let fraction = global.getVariable(mp.players.local, 'fraction', -1);
    if (family <= 0) {
        if ((fraction < 1 || fraction > 5) && fraction != 16)
            return;
        else
            global.gui.setData('warForEnterprice/setOwner', JSON.stringify({ owner: fraction, ownerType: 2 }));

    }
    else
        global.gui.setData('warForEnterprice/setOwner', JSON.stringify({ owner: family, ownerType: 1 }));

    openedCompanyCapt = global.gui.openPage('WarForEnterprice');
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (openedCompanyCapt)
        CloseCompanyMenu();
});

mp.events.add('warForEnterprice::exit', () => {
    CloseCompanyMenu();
});

function CloseCompanyMenu() {
    global.gui.close();
    openedCompanyCapt = false;
}

let peds = [];
mp.events.add('warForEnterprice:loadPeds', (pedsJSON) => {
    pedinfo = JSON.parse(pedsJSON);

    pedinfo.forEach(ped => {
        let p = mp.peds.newValid(579932932, ped.position, ped.heading, 0);
        let obj = { ped: p, id: ped.id };
        peds.push(obj);
        //if(p !== null) p.taskPlayAnim("friends@frm@ig_1", "greeting_idle_a", 8.0, 1.0, -1, 1, 1.0, false, false, false);
    });
});
mp.events.add('warForEnterprice:updatePeds', (pedJSON) => {
    ped = JSON.parse(pedJSON);
    const index = peds.findIndex(item => item.id == ped.id)
    if (index >= 0) {
        peds[index].ped.destroy();
        peds[index].ped = mp.peds.newValid(579932932, ped.position, ped.heading, 0);
    }
    else {
        let p = mp.peds.newValid(579932932, ped.position, ped.heading, 0);
        let obj = { ped: p, id: ped.id };
        peds.push(obj);
    }
});