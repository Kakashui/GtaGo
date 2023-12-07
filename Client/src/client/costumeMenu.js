let opened = false;

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        closeMenu()
});

function closeMenu() {
    global.gui.close();
    opened = false;
    global.showCursor(false)
    global.updateClientClothes(mp.players.local);
}

mp.events.add('costumeMenu:close', () => {
    if (opened)
        closeMenu()
});

mp.events.add('costumeMenu:open', () => {
    opened = global.gui.openPage('CostumeMenu');
    let gender = mp.players.local.getVariable("GENDER") ? true : false;
    global.gui.setData('costumeMenu/setData', JSON.stringify(global.defaultCLothes[gender]));
    for (let i = 3; i <= 11; i++) {
        global.setClothing(mp.players.local, i, global.defaultCLothes[gender][i], 0, 0);
    }
});

mp.events.add('costumeMenu:setClothes', (type, key, drawable, texture) => {
    drawable = parseInt(drawable ? drawable : 0);
    texture = parseInt(texture ? texture : 0);
    if (type == 'clothes')
        global.setClothing(mp.players.local, key, drawable, texture, texture);
    else
        global.setProp(mp.players.local, key, drawable, texture)
});

mp.events.add('costumeMenu:setArmor', (drawable, texture, viewArmor) => {
    drawable = parseInt(drawable ? drawable : 0);
    texture = parseInt(texture ? texture : 0);
    if (viewArmor)
        global.setClothing(mp.players.local, 9, drawable, texture, texture);
    else
        global.setClothing(mp.players.local, 9, 0, 0, 0);
});

mp.events.add('costumeMenu:save', (costumeName, clothesListJson, armorItemJson) => {
    let gender = mp.players.local.getVariable("GENDER") ? true : false;
    let clothesList = JSON.parse(clothesListJson);
    let armorItem = JSON.parse(armorItemJson);
    let clothes = ``;
    let props = ``;
    for (let i = 0; i < clothesList.length; i++) {
        const clothItem = clothesList[i];
        clothItem.drawable = parseInt(clothItem.drawable ? clothItem.drawable : 0);
        clothItem.texture = parseInt(clothItem.texture ? clothItem.texture : 0);
        if (clothItem.type == 'clothes') {
            if (clothItem.drawable != global.defaultCLothes[gender][clothItem.key] && (clothItem.texture >= 0 && clothItem.texture <= 25))
            clothes = clothes + `
                    { CostumeClothesSlots.${clothItem.name}, new CostumeElement(${clothItem.drawable}, ${clothItem.texture}) },`;
        }
        else {
            if (clothItem.drawable != -1 && (clothItem.texture >= 0 && clothItem.texture <= 25))
                props = props + `
                    { CostumePropsSlots.${clothItem.name}, new CostumeElement(${clothItem.drawable}, ${clothItem.texture}) },`;
        }

    }
    if (armorItem.type == 1) {
        armorItem.drawable = parseInt(armorItem.drawable ? armorItem.drawable : 0);
        armorItem.texture = parseInt(armorItem.texture ? armorItem.texture : 0);
        clothes = clothes + `
                    { CostumeClothesSlots.BodyArmor, new CostumeElement(${armorItem.drawable}, ${armorItem.texture}) },`;
    } else {
        armorItem.armType = parseInt(armorItem.armType ? armorItem.armType : 0);
    }
    result = `
            SkinList.Add(CostumeNames.${gender ? 'M' : 'F'}${costumeName}, new CostumeModel(${gender}, 
                new Dictionary<CostumeClothesSlots, CostumeElement>
                {${clothes}
                }, new Dictionary<CostumePropsSlots, CostumeElement>
                {${props}
                }${armorItem.type == 0 ? (',' + armorItem.armType) : ''}));`;
    mp.events.callRemote('costumeMenu:saveServer', result)
    mp.events.call('notify', 2, 9, "costumeMenu:save", 3000);
});

