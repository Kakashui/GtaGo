const Use3d = true;
const UseAutoVolume = true;
const MaxRange = 10.0;
const voiceStates = {
    ONLY_LOCAL: 0,
    WITH_RADIO: 1
}
const voice = {
    nowState: 0
}

const enableMicrophone = () => {
    if (global.chatActive || !global.loggedin) return;

    if (mp.players.local.getVariable('voice.muted') === true || mp.players.local.getVariable('InDeath') == true) return;
    if (global.isFight) return;

    if (mp.voiceChat.muted) {
        mp.voiceChat.muted = false;
        global.gui.setData('hud/updateData', JSON.stringify({ name: 'mic', value: true }));
        mp.players.local.playFacialAnim("mic_chatter", "mp_facial");
    }

    if (voice.nowState === voiceStates.WITH_RADIO) {
        addPlayerToSpeakItems(mp.players.local);
    }
}

const disableMicrophone = () => {
    if (!global.loggedin) return;
    if (!mp.voiceChat.muted) {
        mp.voiceChat.muted = true;
        global.gui.setData('hud/updateData', JSON.stringify({ name: 'mic', value: false }));
        mp.players.local.playFacialAnim("mood_normal_1", "facials@gen_male@variations@normal");
    }

    if (voice.nowState === voiceStates.WITH_RADIO) {
        removePlayerFromSpeakItems(mp.players.local);
    }
}

const speakItems = [];

function addPlayerToSpeakItems(player) {
    const rightIndex = speakItems.findIndex(i => i.id === player.remoteId);
    if (rightIndex > -1) return;

    const item = {
        id: player.remoteId,
        name: player.name
    };

    speakItems.push(item);

    global.gui.setData('hud/addSpeakItem', JSON.stringify(item));
    global.gui.setData('sounds/play', JSON.stringify({ name: "radio_turn_on", volume: 1, loop: false }));
}

function removePlayerFromSpeakItems(player) {
    setTimeout(() => {
        if (!player.isVoiceActive)
            global.gui.setData('hud/removeSpeakItem', player.remoteId);

        const rightIndex = speakItems.findIndex(item => item.id === player.remoteId);
        if (rightIndex === -1) return;

        speakItems.splice(rightIndex, 1)
        global.gui.setData('sounds/play', JSON.stringify({ name: "radio_turn_off", volume: 1, loop: false }));
    }, 100)
}
mp.events.add("gui:ready", ()=>{
    global.keyActions["microphone"].subscribe(enableMicrophone, true);
    global.keyActions["microphone"].subscribe(disableMicrophone, false);
})

let g_voiceMgr =
{
    listeners: [],

    add: function (player, notify) {
        if (this.listeners.indexOf(player) === -1) {
            if (notify) mp.events.callRemote("add_voice_listener", player);
            this.listeners.push(player);

            if (player.isRoomMuted) {
                player.voiceVolume = 0.0;
                player.voice3d = true;
            }

            player.isLocalListening = true;
        }
    },

    remove: function (player, notify) {
        let idx = this.listeners.indexOf(player);
        if (idx !== -1) {
            if (notify) mp.events.callRemote("remove_voice_listener", player);
            this.listeners.splice(idx, 1);
            player.isLocalListening = false;
            player.voice3d = false;
        }
    },

    getLocalVolume: function (player) {
        let playerPos = player.position;
        let dist = mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, localPos.x, localPos.y, localPos.z);
        return 1 - (dist / MaxRange);
    },

    setLocalVoice: function (player) {
        let volumeToSet = this.getLocalVolume(player);
        player.voiceVolume = volumeToSet;
        player.voice3d = true;
    }
};

mp.events.add("playerQuit", (player) => {
    if (player.isLocalListening) g_voiceMgr.remove(player, false);

    //if (player.isRoomListening) RadioRoom.remove(player);
});

const RadioRoom = {
    realPlayerNames: [],
    connectedPlayers: [],
    isRoomMuted: false,
    isConnected: false,

    add: function (player, muted) {
        let item = { id: player.remoteId, object: player };
        this.connectedPlayers.push(item);
        this.realPlayerNames[player.remoteId] = player.name;

        player.isRoomListening = true;
        this.setPlayerMute(player, muted);
    },

    remove: function (player) {
        let item = this.connectedPlayers.find(element => element.id === player.remoteId);
        let idx = this.connectedPlayers.indexOf(item);

        if (idx !== -1) {
            this.connectedPlayers.splice(idx, 1);
            this.realPlayerNames[player.remoteId] = undefined;

            player.isRoomListening = false;

            if (player.isLocalListening) {
                g_voiceMgr.setLocalVoice(player);
            }
        }
    },

    removeById: function (id) {
        let item = this.connectedPlayers.find(p => p.id === id);
        let idx = this.connectedPlayers.indexOf(item);

        if (idx !== -1) {
            let player = this.connectedPlayers[idx].object;

            this.connectedPlayers.splice(idx, 1);
            this.realPlayerNames[id] = undefined;

            player.isRoomListening = false;
        }
    },

    setPlayerMute: function (player, muted) {
        if (muted) {
            if (player.isLocalListening) {
                g_voiceMgr.setLocalVoice(player);
            }
            else {
                player.voiceVolume = 0.0;
            }
        }
        else {
            if (!this.isRoomMuted) {
                player.voice3d = false;
                player.voiceVolume = 5;
            }
        }

        player.isRoomMuted = muted;
    },

    toggleMuteRoom: function () {
        this.isRoomMuted = !this.isRoomMuted;
        global.gui.setData('radio/setIsMuted', this.isRoomMuted);

        if (this.isRoomMuted) {
            this.connectedPlayers.forEach(p => {
                if (p.object.isLocalListening) {
                    g_voiceMgr.setLocalVoice(p.object);
                }
                else {
                    p.object.voice3d = false;
                    p.object.voiceVolume = 0;
                }
            })
        }
        else {
            this.connectedPlayers.forEach(p => {
                if (p.object.isRoomMuted) {
                    if (p.object.isLocalListening) {
                        g_voiceMgr.setLocalVoice(p.object);
                    }
                }
                else {
                    p.object.voice3d = false;
                    p.object.voiceVolume = 5;
                }
            })
        }
    },

    isPlayerConnected: function (player) {
        return this.connectedPlayers.findIndex(p => p.id === player.remoteId) !== -1;
    },

    clearRoom: function () {
        this.connectedPlayers.forEach(p => {
            this.remove(p.object);
        })
    },

    validatePlayer: function (player) {
        if (this.realPlayerNames[player.remoteId] !== player.name) {
            return false;
        }
        else {
            return true;
        }
    }
}

// events from serverside
mp.events.add('voice.radio:add', (player, isMuted) => {
    RadioRoom.add(player, isMuted);
});

mp.events.add('voice.radio:addRange', (playersArray, playersMuteState) => {
    RadioRoom.isConnected = true;
    global.gui.setData('radio/setIsConnected', true);

    playersArray.forEach((remoteId, index) => {
        let player = mp.players.atRemoteId(remoteId);
        RadioRoom.add(player, playersMuteState[index]);
    });
});

mp.events.add('voice.radio:toggleMute', (player, mute) => {
    RadioRoom.setPlayerMute(player, mute);
});

mp.events.add('voice.radio:remove', (player) => {
    RadioRoom.remove(player);
});

mp.events.add('voice.radio:removeById', (remoteId) => {
    RadioRoom.removeById(remoteId);
});

mp.events.add('voice.radio:disconnect', () => {
    if (voice.nowState === voiceStates.WITH_RADIO) {
        switchVoiceState();
    }

    RadioRoom.isConnected = false;
    global.gui.setData('radio/setIsConnected', false);

    RadioRoom.clearRoom();
});

mp.events.add('voice.radio:open', () => {
    global.gui.closeInventory();
    global.gui.openPage('Radio', false);
});

// events from clientside
mp.events.add('radio:setWave', (wave) => {
    mp.events.callRemote('voice.radio::connectWave', wave);
});

mp.events.add('radio:clearWave', () => {
    mp.events.callRemote('voice.radio::clearWave');
});

mp.events.add('radio:mute', () => {
    RadioRoom.toggleMuteRoom();

    if (voice.nowState === voiceStates.WITH_RADIO) {
        switchVoiceState();
    }
});

mp.events.add('radio:close', () => {
    gui.close();
});

function switchVoiceState() {
    if (!RadioRoom.isConnected) return;

    if (voice.nowState === voiceStates.ONLY_LOCAL) {
        voice.nowState = voiceStates.WITH_RADIO;
        mp.events.callRemote('voice.radio::switchState', 'WITH_RADIO');
    }
    else {
        voice.nowState = voiceStates.ONLY_LOCAL;
        mp.events.callRemote('voice.radio::switchState', 'ONLY_LOCAL');
    }

    global.gui.setData('hud/setVoiceState', voice.nowState);
}

mp.keys.bind(global.Keys.Key_OEM_COMMA, false, switchVoiceState);

mp.events.add('voice.mute', () => {
    disableMicrophone();
})

let PHONE = {
    target: null,
    status: false
};

mp.events.add('voice.phoneCall', (target) => {
    if (!PHONE.target) {
        PHONE.target = target;
        PHONE.status = true;
        mp.events.callRemote("add_voice_listener", target);
        target.voiceVolume = 5.0;
        target.voice3d = false;
        g_voiceMgr.remove(target, false);
    }
});
mp.events.add("voice.phoneStop", () => {
    if (PHONE.target) {
        if (mp.players.exists(PHONE.target)) {
            let localPos = mp.players.local.position;
            const playerPos = PHONE.target.position;
            let dist = mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, localPos.x, localPos.y, localPos.z);
            if (dist > MaxRange) mp.events.callRemote("remove_voice_listener", PHONE.target);
            else g_voiceMgr.add(PHONE.target, false);
        } else mp.events.callRemote("remove_voice_listener", PHONE.target);
        PHONE.target = null;
        PHONE.status = false;
    }
});

let f3KD = 0;

mp.keys.bind(global.Keys.Key_F3, true, () => {
    if(f3KD > Date.now()) return;
    f3KD = Date.now() + 15000;
    mp.events.call('notify', 2, 9, "client_41", 3000);
    mp.voiceChat.cleanupAndReload(true, true, true);

});


mp.events.add('playerStartTalking', (player) => {
    //if (PHONE.target != player) player.voice3d = true;

    if (player.handle !== 0) {
        player.playFacialAnim("mic_chatter", "mp_facial");
    }

    if (player.isRoomListening && !player.isRoomMuted && !RadioRoom.isRoomMuted && RadioRoom.validatePlayer(player)) {
        player.voiceVolume = 5;
        addPlayerToSpeakItems(player);
    }
    else if (PHONE.target === player) {
        player.voiceVolume = 5;
    }
    else if (!player.isLocalListening) {
        player.voiceVolume = 0;
    }

});

mp.events.add('playerStopTalking', (player) => {
    if (player.handle !== 0) {
        player.playFacialAnim("mood_normal_1", "facials@gen_male@variations@normal");
    }

    if (RadioRoom.validatePlayer(player)) {
        removePlayerFromSpeakItems(player);
    }
});

let localPos = mp.players.local.position;
let playerPos = mp.players.local.position;
setInterval(() => {
    localPos = mp.players.local.position;
    if(global.mediaMute){
        if(g_voiceMgr.listeners.length > 0){
            g_voiceMgr.listeners.forEach(player => {
                let notify = !player.isRoomListening;
                g_voiceMgr.remove(player, notify);
            })
        }
    }
    mp.players.forEachInStreamRange(player => {
        if(global.mediaMute) return;
        if (player != mp.players.local) {
            if (!player.isLocalListening && (!PHONE.target || PHONE.target != player)) {
                playerPos = player.position;
                const lvl = global.getVariable(player, 'lvl', 0);
                if (mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, localPos.x, localPos.y, localPos.z) <= MaxRange) {
                    if(mp.storage.data.mainSettings.muteLowLevel && lvl < mp.storage.data.mainSettings.muteLowLevelValue) return;
                    let notify = !player.isRoomListening;
                    g_voiceMgr.add(player, notify);
                }
            }
        }
    });

    g_voiceMgr.listeners.forEach((player) => {
        if (player.handle !== 0) {
            playerPos = player.position;
            let dist = mp.game.system.vdist(playerPos.x, playerPos.y, playerPos.z, localPos.x, localPos.y, localPos.z);
            const lvl = global.getVariable(player, 'lvl', 0);
            if (dist > MaxRange || (mp.storage.data.mainSettings.muteLowLevel && lvl < mp.storage.data.mainSettings.muteLowLevelValue)) {
                let notify = !player.isRoomListening;
                g_voiceMgr.remove(player, notify);
            }
            else if (UseAutoVolume && (!player.isRoomListening || player.isRoomMuted)) {
                player.voiceVolume = 1 - (dist / MaxRange);
            }
        } else {
            let notify = !player.isRoomListening;
            g_voiceMgr.remove(player, notify);
        }
    });
}, 350);