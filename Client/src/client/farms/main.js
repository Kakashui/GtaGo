const { pointsConfig, plantsConfig, pitsConfig, fertilizersConfig, productConfigs } = require('./config');
const Plant = require('./plant');


let sendingExcept = false;


let currentFarm = -1;
let lastCheck = Date.now();
let nearestPoint = null;
let currentPlants =
{
    [0]: [

    ]
};

let distanceToNearestPoint = 3;

setInterval(() => {
    UpdatePlants();
}, 15000);

mp.events.add({
    "farm:loadPoints": (farmId, pointData) => {
        currentFarm = farmId;
        if (!currentPlants[currentFarm])
            currentPlants[currentFarm] = [];
        LoadPlants(JSON.parse(pointData), farmId)
        UpdatePlants();
    },

    "farm:unloadPoints": (farm) => {
        if (currentFarm == farm) {
            currentFarm = -1;
            global.gui.setData('hud/setHoleInformation', JSON.stringify({ show: false, itemInfo: null, stateInfo: null }));
        }
    },

    // "farm:plantSeed": (seedId) => {
    //     if (nearestPoint != null)
    //         mp.events.callRemote("farm:plantSeedOnPoint", seedId, currentFarm, nearestPoint.ID);
    //     else
    //         mp.events.callRemote("farm:plantSeedOnPoint", seedId, currentFarm, -1);
    // },

    "farm:updatePlant": (plantData) => {
        let plant = JSON.parse(plantData);
        if (plant.FarmId == currentFarm) {
            UpdatePointData(plant, plant.FarmId)
        }
    },

    "farm:deletePlant": (farmId, plantId) => {
        if (farmId == currentFarm) {
            DeletePointData(plantId, farmId)
        }
    },



});

mp.events.add('render', () => {
    try {
        if (currentFarm < 0)
            return;
        if (nearestPoint != null)
            mp.game.graphics.drawText(`•`, [nearestPoint.Position.x, nearestPoint.Position.y, nearestPoint.Position.z], {
                font: 0,
                color: [182, 211, 0, 200],
                scale: [0.5, 0.5],
                outline: false
            });
        if (Date.now() - lastCheck > 500)
            UpdategNearestPoint();
        else
            return;
        if (nearestPoint == null) {
            global.gui.setData('hud/setHoleInformation', JSON.stringify({ show: false, itemInfo: null, stateInfo: null }));
            return;
        }
        let plant = currentPlants[currentFarm].find(item => item.PointId == nearestPoint.ID);
        itemInfo =
        {
            isMy: null,
            name: null,
            icon: null,
            isWatered: null,
            fertilizer: null,
            hole: `farmHouse_${29 + nearestPoint.PitType}`
        };
        stateInfo = {};
        if (plant) {
            let status = plant.GetCurrentStatus();
            if (status) {
                itemInfo = {
                    isMy: global.UUID == plant.UUID,
                    name: `item_${status.plantName.toLowerCase()}`,
                    icon: status.plantName.toLowerCase().replace('seed', ''),
                    isWatered: status.isWatering,
                    fertilizer: `fertilizer_${status.isFertilizering}`,
                    hole: `farmHouse_${29 + nearestPoint.PitType}`
                };
                stateInfo =
                {
                    show: true,
                    isWithers: status.currentStage >= 1,
                    value: Math.floor(status.currentStage < 1 ? status.timeToRipening : status.currentStage < 300 ? status.witheringTime + status.timeToRipening : 0),
                    maxValue: status.currentStage < 1 ? status.currentRipeningTime : status.witheringTime
                }
                nearestPoint.Position = plant.Position;
            }
        }
        global.gui.setData('hud/setHoleInformation', JSON.stringify({
            show: true,
            itemInfo: itemInfo,
            stateInfo: stateInfo
        }));

    } catch (e) {
        if (global.sendException && !sendingExcept) {
            sendingExcept = true;
            mp.serverLog(`Error in farm.main.render: ${e.name}\n${e.message}\n${e.stack}`);
        }
    }
});

function UpdategNearestPoint() {
    try {
        lastCheck = Date.now()
        // let resolution = mp.game.graphics.getScreenActiveResolution(1, 1);
        // let lookingPoint = mp.game.graphics.screen2dToWorld3d([resolution.x / 2, resolution.y / 2, (2 | 4 | 8)]);
        // if (!lookingPoint) {
        //     nearestPoint = null;
        //     return;
        // }
        nearestPoint = GetNearestPointForCoord(mp.players.local.position);
        if (nearestPoint != null && getDist(nearestPoint.Position, mp.players.local.position) > distanceToNearestPoint)
            nearestPoint = null;
        // if (nearestPoint != null) {
        //     const zCoord = mp.game.gameplay.getGroundZFor3dCoord(nearestPoint.Position.x, nearestPoint.Position.y, 1000, 0.0, false);
        //     nearestPoint.Position.z = zCoord;
        // }
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in farm.main.UpdategNearestPoint: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function GetNearestPointForCoord(coord) {
    try {
        if (!pointsConfig[currentFarm])
            return null;
        let nearest = pointsConfig[currentFarm][0];
        pointsConfig[currentFarm].forEach(point => {
            if (getDist(coord, nearest.Position) > getDist(coord, point.Position))
                nearest = point;
        });
        return nearest;
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in farm.main.GetNearestPointForCoord: ${e.name}\n${e.message}\n${e.stack}`);
        return null;
    }
}

function getDist(p1, p2) {
    return mp.game.system.vdist(p1.x, p1.y, p1.z, p2.x, p2.y, p2.z)
}

function UpdatePlants() {
    try {
        if (!currentPlants[currentFarm])
            return;
        currentPlants[currentFarm].forEach(item => {
            if (item)
                item.UpdateObject()
        });
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in farm.main.UpdatePlants: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function LoadPlants(plants, farmId) {
    try {
        //Удаляем растения, которых нет в обновленном списке
        currentPlants[farmId].forEach(itemCurr => {
            if (plants.findIndex(itemNew => itemNew.PointId == itemCurr.PointId) < 0)
                itemCurr.DeletePlant()
        });
        plants.forEach(itemNew => {
            let index = currentPlants[farmId].findIndex(itemCurr => itemNew.PointId == itemCurr.PointId)
            if (index >= 0)
                currentPlants[farmId][index].UpdateData(itemNew);
            else
                currentPlants[farmId].push(new Plant(itemNew));
        });
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in farm.main.LoadPlants: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function UpdatePointData(point, farmId) {
    try {
        let index = currentPlants[farmId].findIndex(itemCurr => point.PointId == itemCurr.PointId)
        if (index >= 0) {
            currentPlants[farmId][index].UpdateData(point);
        }
        else {
            currentPlants[farmId].push(new Plant(point));
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in farm.main.UpdatePointData: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function DeletePointData(pointId, farmId) {
    try {
        let index = currentPlants[farmId].findIndex(itemCurr => pointId == itemCurr.PointId)
        if (index >= 0) {
            currentPlants[farmId][index].DeletePlant();
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`Error in farm.main.DeletePointData: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

let helpMarkers = [];
let waipoint = false;
mp.events.add({
    "farm:createHelpMarkers": (farmId, maxPitType) => {
        DeleteHelpMarkers();
        waipoint = false;
        pointsConfig[farmId].forEach(point => {
            if (point.PitType <= maxPitType) {
                let index = currentPlants[farmId].findIndex(itemCurr => point.PointId == itemCurr.PointId)
                if (index < 0 || index >= 0 && currentPlants[farmId][index].PlantName == -1)
                {
                    CreateHelpMarker(point.Position)
                    return;
                }
            }
        });
    },

    "farm:deleteHelpMarkers": () => {
        DeleteHelpMarkers();
    },
});

function CreateHelpMarker(pos) {
    let marker = mp.markers.new(0, new mp.Vector3(pos.x, pos.y, pos.z + 2), 1,
        {
            rotation: new mp.Vector3(),
            color: [50, 200, 100, 200],
            visible: true,
            dimension: 0
        });
    helpMarkers.push(marker);
    if (!waipoint)
    {
        mp.game.ui.setNewWaypoint(pos.x, pos.y);
        waipoint = true;
    }
}

function DeleteHelpMarkers() {
    helpMarkers.forEach(marker => {
        marker.destroy();
    });
    helpMarkers = [];
}