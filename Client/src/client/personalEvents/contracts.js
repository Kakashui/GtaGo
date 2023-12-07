
const contractsConfig = require('./configs/contractsConfig');
let allContract = {};




mp.events.add("personalEvents:loadMyContracts", (contractsJson) => {
    let contracts = JSON.parse(contractsJson);
    contracts.forEach(item => {
        item.ExpirationDateMS = Date.now() + item.SecondToEndContract * 1000
        allContract[item.ContractName] = item;
    });
    global.gui.setData("optionsMenu/setMyContracts", JSON.stringify(contracts));
});


mp.events.add("personalEvents:loadFamilyContracts", (contractsJson) => {
    let contracts = JSON.parse(contractsJson);
    contracts.forEach(item => {
        item.ExpirationDateMS = Date.now() + item.SecondToEndContract * 1000
        allContract[item.ContractName] = item;
    });
    global.gui.setData("optionsMenu/setFamilyContracts", JSON.stringify(contracts));
});


mp.events.add("personalEvents:updateMyContracts", (contractJson) => {
    let contract = JSON.parse(contractJson);
    contract.ExpirationDateMS = Date.now() + contract.SecondToEndContract * 1000
    allContract[contract.ContractName] = contract;
    global.gui.setData("optionsMenu/updateMyContracts", JSON.stringify(contract));
});


mp.events.add("personalEvents:updateFamilyContracts", (contractJson) => {
    let contract = JSON.parse(contractJson);
    contract.ExpirationDateMS = Date.now() + contract.SecondToEndContract * 1000
    allContract[contract.ContractName] = contract;
    global.gui.setData("optionsMenu/updateFamilyContracts", JSON.stringify(contract));
});


mp.events.add("personalEvents:selectCoordinats", (positionJson) => {
    let position = JSON.parse(positionJson);
    mp.game.ui.setNewWaypoint(position.x, position.y);
});

mp.events.add('family:unloadData', () => {
    contractsConfig.forEach((contractConf) => {
        if (contractConf.ContractType =="Family")
            allContract[contractConf.ContractName] = null;
    });
});

let defaultColor = {
    Red : 182, 
    Green : 211, 
    Blue : 0, 
    Alpha : 200,
}

mp.events.add('render', () => {
    let i = 0;
    contractsConfig.forEach((contractConf) => {
        if (allContract[contractConf.ContractName] && allContract[contractConf.ContractName].InProgress && allContract[contractConf.ContractName].ExpirationDateMS > Date.now() && 0 === mp.players.local.dimension) {
            contractConf.Coords.forEach(coord => {
                coord.Positions.forEach(position => {
                    const pos1 = mp.players.local.position;
                    const pos2 = new mp.Vector3(position.x, position.y, position.z);
                    const distance = mp.game.system.vdist(pos1.x, pos1.y, pos1.z, pos2.x, pos2.y, pos2.z);
        
                    if (distance < 50) {
                        global.drawRotateMarkerInRender(27, pos2, 2, defaultColor)
                        i++;
                    }
                });
            });
        }
    });
})    