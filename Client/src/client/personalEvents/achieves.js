
const achievesConfig = require('./configs/achievesConfig');
const actionSyncParams = require('./configs/playerActionSyncParams');
const Achieve = require('./models/achieve');


let notSyncActions = {};
let myAchieves = {};
let subscribes = {};
let needSyncAchieves = {};
let syncTimers = {}

mp.events.add("personalEvents:load", (currentStatsJson) => {
    let currentStats = JSON.parse(currentStatsJson);
    achievesConfig.forEach(achieveConf => {
        if (achieveConf.IsActive)
        {
            if (currentStats[achieveConf.AchieveName]) {
                myAchieves[achieveConf.AchieveName] = new Achieve(currentStats[achieveConf.AchieveName].CurrentLevel, currentStats[achieveConf.AchieveName].GivenReward, currentStats[achieveConf.AchieveName].DateCompleated, achieveConf);
            }
            else {
                myAchieves[achieveConf.AchieveName] = new Achieve(0, false, 0, achieveConf);
            }
        }
    });
});

mp.events.add("personalEvents:updateAchieve", (achieveJson) => {
    var achieve = JSON.parse(achieveJson);
    if (myAchieves[achieve.AchieveName])
        myAchieves[achieve.AchieveName].UpdateProgress(achieve.CurrentLevel, achieve.GivenReward, achieve.DateCompleated)
});

global.personalEventsSubscribe = (achieve, ActionName) => {
    if (subscribes[ActionName]) {
        if (subscribes[ActionName].findIndex(item => item === achieve) < 0)
            subscribes[ActionName].push(achieve);
    }
    else {
        subscribes[ActionName] = [achieve];
    }
}


mp.events.add("personalEvents:invokeEvents", (ActionName, points) => {
    points = Math.round(points);
    if (subscribes[ActionName]) {
        subscribes[ActionName].forEach(achieve => {
            achieve.AchieveProgress(points);
        });
    }
    UpdateCurrentActionState(ActionName, points)
});

mp.events.add("personalEvents:needSync", (ActionName) => {
    needSyncAchieves[ActionName] = true;
});

mp.events.add("personalEvents:syncAction", (ActionName) => {
    let points = notSyncActions[ActionName];
    if (points) {
        notSyncActions[ActionName] = 0;
        if (syncTimers[ActionName])
        {
            clearTimeout(syncTimers[ActionName]);
            syncTimers[ActionName] = null;
        }
        mp.events.callRemote("personalEvents:syncClientEvents", ActionName, points)
    }
});


function UpdateCurrentActionState(ActionName, points) {
    if (notSyncActions[ActionName])
        notSyncActions[ActionName] += points;
    else
        notSyncActions[ActionName] = points;
    if (needSyncAchieves[ActionName] || actionSyncParams[ActionName].SyncMaxPoint <= notSyncActions[ActionName]) {
        needSyncAchieves[ActionName] = false;
        mp.events.call("personalEvents:syncAction", ActionName);
    }
    else {
        if (!syncTimers[ActionName] && actionSyncParams[ActionName].SyncTime) {
            syncTimers[ActionName] = setTimeout(() => {
                mp.events.call("personalEvents:syncAction", ActionName);
            }, actionSyncParams[ActionName].SyncTime * 1000);
        }
    }
}