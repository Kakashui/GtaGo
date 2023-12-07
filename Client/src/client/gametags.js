mp.nametags.enabled = false;

let reupdateTagLabel = [];
let tagLabelPool = [];

let playerPos = void 0;
//let playerTarget = void 0;
let playerAimAt = void 0;
let width = 0.025;
let height = 0.004;
let border = 0.001;
const actionShowTime = 15 * 1000;

let sendingExcept = false;

mp.events.add("tag:add:action", (id, msg)=>{
    let player = mp.players.atRemoteId(id);
    global.gui.pushChat(player.name.replace('_', ' '), msg, "#5D55FF");
    player.doAction = {action: msg, expiried: Date.now() + actionShowTime};
})

mp.events.add('setFriendList', function (friendlistString) {
    let friendlist = JSON.parse(friendlistString);
    friendlist.forEach(friend => {
        global.friends[friend.Nickname] = true;
    });
});

mp.events.add('addFriendToList', function (name) {
    global.friends[name] = true;
});

function calculateDistance(v1, v2) {
    let dx = v1.x - v2.x;
    let dy = v1.y - v2.y;
    let dz = v1.z - v2.z;

    return Math.sqrt(dx * dx + dy * dy + dz * dz);
}

global.playerIsFriend = (player)=>{
    return global.friends[player.name] !== undefined;
}

global.playerIsFractionMember = (player)=>{
    let localFraction = global.localplayer.getVariable('fraction');
    let playerFraction = player.getVariable('fraction');
    return localFraction != null && localFraction !== 0 && playerFraction != null && localFraction === playerFraction ;
}

global.playerIsFamilyMember = (player)=>{
    let localFamily = global.getVariable(mp.players.local, 'familyuuid', 0);
    let playerFamily = global.getVariable(player, 'familyuuid', 0);
    return localFamily !== 0 && localFamily === playerFamily;
}

global.iKnowThisPlayer = (player)=>{
    if(!player) return false;
    return player === mp.players.local || global.getVariable(mp.players.local, 'ALVL', 0) > 0 || global.playerIsFractionMember(player) || global.playerIsFamilyMember(player) || global.playerIsFriend(player);
}

const nameHash = {
    "name": {
        "firstName": "firstName",            
        "lastName": "lastName"
    }
}

mp.events.add('render', (nametags) => {

    if (!global.loggedin) return;
    playerPos = mp.players.local.position;
    playerAimAt = mp.game.player.getEntityIsFreeAimingAt();
    //playerTarget = mp.game.player.getTargetEntity();
    
    // Get variables
    let isAdmin = global.getVariable(mp.players.local, 'ALVL', 0) > 0;

    // Admin get target info
    /*if (isAdmin == true) {
        player = playerAimAt;
        if (player !== undefined && player.handle !== undefined && player.handle) {
            if (player.getType() === 4) {
                mp.game.graphics.drawText(player.name + ' (' + player.remoteId + ')', [0.5, 0.8], { font: 4, color: [255, 255, 255, 235], scale: [0.5, 0.5], outline: true });
            }
        }
    }*/

    // Player gamertags
    if (mp.storage.data.mainSettings.showNames) {

        //testTag(nametags); //<<<<<<<<<<<<<<<<<<<<<<<< COMMENT

        nametags.forEach(function (nametag) {
            try {
                    _player = nametag[0],
                    x = nametag[1],
                    y = nametag[2] - .035,
                    distance = nametag[3];
                
                if (calculateDistance(playerPos, _player.position) < 15.0) {
                    if (_player.getVariable('INVISIBLE') != true && _player.getVariable('HideNick') != true) {
                        
                        if (tagLabelPool[_player.remoteId] === undefined || reupdateTagLabel[_player.remoteId] === undefined || reupdateTagLabel[_player.remoteId] < Date.now()) {                            
                            reupdateTagLabel[_player.remoteId] = Date.now() + 500;
                            if(nameHash[_player.name] == undefined) {
                                const nameArray = _player.name.split('_');
                                nameHash[_player.name] = {"firstName": nameArray[0], "lastName": nameArray[1]}
                            }

                            let text = "";

                            //C_ID
                            //_player.getVariable('C_ID')
                            //if (_player.getVariable('ADM_NAME') === true)
                            //    text += "ADMINISTRATOR\n";
                            
                            if (isAdmin === true){                                
                                text += `${nameHash[_player.name].firstName} ${nameHash[_player.name].lastName} ${_player.getVariable('IS_MASK') == true ? '(M)' : ''}\nID: ${_player.getVariable('C_ID')}`;
                                //text += `Незнакомец\n#${_player.remoteId}`;
                            } else if (global.isFight || global.isDemorgan) {
                                text += `Незнакомец\n#${_player.getVariable('C_ID')}`;
                            } else if (global.playerIsFractionMember(_player) || global.playerIsFamilyMember(_player)){
                                text += `${nameHash[_player.name].firstName} ${nameHash[_player.name].lastName}\nID: ${_player.getVariable('C_ID')}`;
                            } else if (_player.getVariable('IS_MASK') == true) {
                                text += `Незнакомец\nID: ${_player.getVariable('C_ID')}`;
                            } else {
                                if (!global.playerIsFriend(_player))                                
                                    text += `Незнакомец\nID: ${_player.getVariable('C_ID')}`;
                                else
                                    text += `${nameHash[_player.name].firstName} ${nameHash[_player.name].lastName}\nID: ${_player.getVariable('C_ID')}`;
                            }
                            
                            let color;
                            if (global.localplayer.getVariable('fraction') != 7) { // only policeman could see carthiefs' red names
                                color = _player.getVariable('ADM_NAME') === true ? [182, 211, 0, 255] : [255, 255, 255, 255];
                            }
                            else {
                                color = _player.getVariable('REDNAME') === true ? [255, 0, 0, 255] : _player.getVariable('ADM_NAME') === true ? [182, 211, 0, 255] : [255, 255, 255, 255];
                            }
                            let hpNarmor = "";
                            let fraction = "";
                            
                            if(_player.doAction)
                            {
                                if(_player.doAction.expiried < Date.now()) {
                                    _player.doAction = undefined;
                                    text = `\n${text}`;
                                }else text = `~p~${_player.doAction.action}\n~w~${text}`;
                            }else text = `\n${text}`;

                            tagLabelPool[_player.remoteId] = { text, color: color, hpNarmor: hpNarmor, fraction: fraction };
                            //add action
                            
                        }
                        if (_player.vehicle) 
                            y += 0.065;
                        let label = tagLabelPool[_player.remoteId];
                        if (label !== undefined) {
                            if (!isAdmin || global.esptoggle != 1 && global.esptoggle != 3)
                            {
                                drawPlayerTag(_player, x, y, label.text, label.color, label.hpNarmor, label.fraction);
                                drawPlayerDead(_player, x, y);
                            }
                            drawPlayerVoiceIcon(_player, x, y);
                            drawPlayerIcon(_player, x, y);
                        }
                    }
                }
            } catch (e) {
                if (global.sendException && !sendingExcept) {
                    sendingExcept = true;
                    mp.serverLog(`Error in gametags.render: ${e.name}\n${e.message}\n${e.stack}`);
                } 
            }
        });
    }
});
function drawPlayerTag(player, x, y, displayname, color, hpNarmor, fraction) {
    //let position = player.getBoneCoords(12844, 0.6, 0, 0); //player.position;
    //position.z += 1.5;
    //let frameTime = lastFrameTime;
    //const frameRate = 1.0 / (mp.game.invoke("0x15C40837039FFAF7") / );

    
    // draw user name
    mp.game.graphics.drawText(displayname, [x, y], { font: 4, color: color, scale: [0.35, 0.35], outline: true });

    if (hpNarmor != "")
    {
        mp.game.graphics.drawText(`Frac: ${fraction}, ${hpNarmor}`, [x, y + 0.04], { font: 4, color: color, scale: [0.35, 0.35], outline: true });
    }
    // draw health & ammo bar
    else if (/*playerTarget != undefined && player.handle == playerTarget.handle ||*/ playerAimAt != undefined && player.handle == playerAimAt.handle || global.spectating) {
        y += 0.08;
        let health = player.getHealth();
        health = health <= 100 ? health / 100 : (health - 100) / 100;

        let armour = player.getArmour() / 100;
        if (armour > 0) {

            mp.game.graphics.drawRect(x, y, width + border * 2, height + border * 2, 0, 0, 0, 200);
            mp.game.graphics.drawRect(x, y, width, height, 150, 150, 150, 255);
            mp.game.graphics.drawRect(x - width / 2 * (1 - health), y, width * health, height, 255, 255, 255, 200);

            y -= 0.007;

            mp.game.graphics.drawRect(x, y, width + border * 2, height + border * 2, 0, 0, 0, 200);
            mp.game.graphics.drawRect(x, y, width, height, 41, 66, 78, 255);
            mp.game.graphics.drawRect(x - width / 2 * (1 - armour), y, width * armour, height, 48, 108, 135, 200);
        } else {

            mp.game.graphics.drawRect(x, y, width + border * 2, height + border * 2, 0, 0, 0, 200);
            mp.game.graphics.drawRect(x, y, width, height, 150, 150, 150, 255);
            mp.game.graphics.drawRect(x - width / 2 * (1 - health), y, width * health, height, 255, 255, 255, 200);
        }
    }
}

function drawPlayerVoiceIcon(player, x, y) {
    let lvl = global.getVariable(player, 'lvl', 0);
    if (player.isVoiceActive)
        drawVoiceSprite("mpleaderboard", 'leaderboard_audio_3', [0.7, 0.7], 0, lvl >= 2 ? [255, 255, 255, 255] : [255, 0, 0, 255], x, y - 0.02 * 0.7);
    else if (player.getVariable('voice.mute') == true)
        drawVoiceSprite("mpleaderboard", 'leaderboard_audio_mute', [0.7, 0.7], 0, [255, 0, 0, 255], x, y - 0.02 * 0.7);
}

function drawPlayerDead(player, x, y) {
    let InDeath = (player.getVariable('InDeath') == true);
    if (InDeath) {
        drawVoiceSprite("mpinventory", 'deathmatch', [0.7, 0.7], 0, [255, 0, 0, 255], x, y + 2 * 0.08);
        // mp.game.graphics.drawText("Dying", [x, y + 0.06], { font: 4, color: [70, 70, 70, 255], scale: [0.35, 0.35], outline: true });
    }
}

function drawPlayerIcon(player, x, y) {
    let colorIndex = global.getVariable(player, 'playerIcon:color', -1);
    let dict = global.getVariable(player, 'playerIcon:dict', 'none');
    let name = global.getVariable(player, 'playerIcon:name', 'none');
    if (dict == 'none' || name == 'none' || colorIndex == -1)
        return;
    let color = { "Red":255, "Green":255, "Blue":255 };
    let index = global.RageColorsList.findIndex(item => item.Number == colorIndex);
    if (index > -1)
        color = global.RageColorsList[index];
    drawVoiceSprite(dict, name, [0.3, 0.3], 0, [color.Red, color.Green, color.Blue, 255], x, y + 0.08);
}

function drawVoiceSprite(dict, name, scale, heading, color, x, y, layer) {
    if (mp.game.graphics.hasStreamedTextureDictLoaded(dict)) {
        const resolution = mp.game.graphics.getScreenActiveResolution(0, 0);
        const textureResolution = mp.game.graphics.getTextureResolution(dict, name);
        const textureScale = [scale[0] * textureResolution.x / resolution.x, scale[1] * textureResolution.y / resolution.y];
        if (typeof layer === 'number') mp.game.graphics.set2dLayer(layer);        
        mp.game.graphics.drawSprite(dict, name, x, y, textureScale[0], textureScale[1], heading, color[0], color[1], color[2], color[3]);
    } else mp.game.graphics.requestStreamedTextureDict(dict, true);
}

function testTag(nametags){
    const pos = mp.game.graphics.world3dToScreen2d(global.localplayer.position.x, global.localplayer.position.y, global.localplayer.position.z + 1.2)
    if(pos) nametags.push([global.localplayer, pos.x, pos.y, 5])
}