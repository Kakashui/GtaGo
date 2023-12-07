let members = [];
global.showFamMembersOnMinimup = false;
let checkInterval = 500;
let nextCheck = Date.now();

mp.events.add('familyMenu:saveMapOptions', (showMember) => {
    global.showFamMembersOnMinimup = showMember;
    mp.storage.data.showFamMembersOnMinimup = showMember;
    mp.storage.flush();
    if (global.showFamMembersOnMinimup)
        LoadMembers(global.getVariable(mp.players.local, 'familyuuid', -1))
    else
        LoadMembers(-1);
    global.gui.setData('familyMenu/setMembersOnMap', JSON.stringify(global.showFamMembersOnMinimup));
});

let lastRenderLog = Date.now();

mp.events.add('render', () => {
    try {
        if (!global.showFamMembersOnMinimup)
            return;
        //update positions 
        if (Date.now() > nextCheck) {
            nextCheck = Date.now() + checkInterval;
            members.forEach(member => {
                if (member.blip && mp.blips.exists(member.blip) && member.player && mp.players.exists(member.player) && member.player.handle && member.player.position !== undefined)
                    member.blip.setCoords(member.player.getCoords(true));
            });
        }
    } catch (e) {
        if (global.sendException){
            if(lastRenderLog > Date.now()) return;
            lastRenderLog = Date.now() + 3000;
            mp.serverLog(`syncMemberBlip.render: ${e.name}\n${e.message}\n${e.stack}`);
        } 
    }
});

mp.events.addDataHandler("familyuuid", (player, familyId) => {
    try {
        if (!global.showFamMembersOnMinimup)
            return;
        if (player == mp.players.local) {
            LoadMembers(familyId);
        }
        else if (player.handle != 0) {
            DeleteMember(player);
            if (familyId == global.getVariable(mp.players.local, 'familyuuid', -1))
                AddMember(player)
        }
    } catch (e) {
        if (global.sendException) mp.serverLog(`syncMemberBlip.addDataHandler(familyuuid): ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add('entityStreamOut', (player) => {
    try {
        if (!global.showFamMembersOnMinimup)
            return;
        if (!player || player.type !== "player") return;
        DeleteMember(player);
    } catch (error) {
        if (global.sendException) mp.serverLog(`syncMemberBlip.entityStreamOut: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

//Sync data on stream in
mp.events.add("entityStreamIn", (player) => {
    try {
        if (!global.showFamMembersOnMinimup)
            return;
        if (!player || player.type !== "player") return;
        if (global.getVariable(player, 'familyuuid', -2) == global.getVariable(mp.players.local, 'familyuuid', -1))
            AddMember(player)
    } catch (error) {
        if (global.sendException) mp.serverLog(`syncMemberBlip.entityStreamIn: ${e.name}\n${e.message}\n${e.stack}`);

    }
});

function LoadMembers(familyId) {
    try {
        members.forEach(member => {
            if (member.blip && mp.blips.exists(member.blip))
                member.blip.destroy();
        });
        members = [];
        if (familyId > 0) {
            mp.players.forEachInStreamRange(player => {
                if (player !== mp.players.local && familyId == global.getVariable(player, 'familyuuid', -1)) {
                    AddMember(player);
                }
            });
        }
    } catch (error) {
        if (global.sendException) mp.serverLog(`syncMemberBlip.LoadMembers: ${e.name}\n${e.message}\n${e.stack}`);
    }
}

function DeleteMember(player) {    
    let index = members.findIndex(item => item.player == player);
    if (index >= 0) {
        if (members[index].blip && mp.blips.exists(members[index].blip))
            members[index].blip.destroy();
        members.splice(index, 1);
    }
}

function AddMember(player) {
    const blip = mp.blips.new(1, player.getCoords(true),
        {
            name: 'Family member',
            scale: 0.5,
            color: 25,
            alpha: 255,
            drawDistance: 250,
            shortRange: false,
            rotation: 0,
            dimension: 0,
        }
    );
    members.push({ player, blip });
}