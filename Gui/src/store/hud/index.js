import warForEnterprice from './modules/war-for-enterprice'

export default {
    namespaced: true,
    state: {
        helpShow: true,
        ammo: 0,
        mammo: 0,
        money: 0,
        moneyChange: 0,
        bank: 0,
        mic: false,
        voiceState: 0,
        speakItems: [],
        minimap: {// в долях от экрана (по х слева по y свурху)
            width: 0.2,
            height: 0,
            scaleX: 0,
            scaleY: 0,
            leftX: 0.05,
            bottomY: 0.98,
            topY: 0.78,
            rightX: 0.2
        },
        artimer: undefined,
        realSpeakItems: [],
        time: "12:33",
        date: "25.08.2023",
        hudShow: true,
        id: 713,
        online: 1233,
        isKillog: false,
        isGreenZone: false,
        wantedLevels: 0,
        promptData: {
            show: false, //nn
            items: [
                { key: 'E', text: 'some tetx' },
                { key: 'I', text: 'bizhelp_1' },
                { key: 'I', text: 'Чтобы открыть инфопанель' },
            ]
        },
        reportsCount: null,
        // CHIPS
        isChipsList: true,
        chipsList: {
            'black': 0,
            'red': 0,
            'blue': 0,
            'green': 0,
            'yellow': 0 
        },
        // KILLSTAT
        isKillstat: true,
        killstatItems: [
             { id: 1, username: "Brian Smith", kills: 0, deaths: 2 },
            // { id: 4, username: "Brian Smith", kills: 2, deaths: 0 },
            // { id: 5, username: "Lyle Kauffman", kills: 11, deaths: 7 }
            // ДЛЯ ГЛОБАЛА 
            //{ id: 5, username: "Lyle Kauffman", points: 12, place:1 }
        ],
        killstatType: '',// match_result, global_rating, ''
        
        sortKillstatItems: [],
        killstatCurrentUser: { id: 4, username: "Brian Smith", kills: 41, deaths: 3 },
        killStatTime: null,
        killStatTimer: null,
        isVote: false,
        isFullKillstat: false,
        voteItems: [
            { title: 'Recycling', currentCount: 0, maxCount: 50 },
            { title: 'Camp', currentCount: 0, maxCount: 50 },
            { title: 'Sawmill', currentCount: 0, maxCount: 50 }
        ],
        // WORK TIMER
        workTime: null,
        workTimer: null,
        // CAPTURE
        isCaptureItems: false, //nn
        captureTeams: [
            { id: 0, key: 'bloods', title: 'Blood street', players: 0 },
            { id: 1, key: 'ballas', title: 'Ballas gang', players: 0 }
        ],
        isCaptureLog: false,
        captureLog: [
            // { killerName: "Killer_name", killerFraction: 'bloods', deceasedFraction: 'ballas', deceasedName: "Player_Name", weaponId: 103 },
            // { killerName: "Killer_name", killerFraction: 'neutral', deceasedFraction: 'ballas', deceasedName: "Player_Nam", weaponId: 113 },
            // { killerName: "Killer_name", killerFraction: 'bloods', deceasedFraction: 'ballas', deceasedName: "Player_Nme", weaponId: 123 },
            // { killerName: null, killerFraction: null, deceasedFraction: 'ballas', deceasedName: "Player_ame", weaponId: 99 },
            // { killerName: "Killer_name", killerFraction: 'bloods', deceasedFraction: 'ballas', deceasedName: "Plaer_Name", weaponId: 143 }
        ],
        captureTimer: null,
        captureTimeLimit: 600,
        captureTimeCurrent: 2,
        statusDisplayShow: true,
        statusDisplays: {
            hungerLevel: 31,
            thirstLevel: 10,
            restLevel: 100,
            joyLevel: 100
        },
        untilCaptureTimerMaxTime: 200,
        untilCaptureTimerCurrentTime: 75,
        untilCaptureTimerEnabled: false, //nn
        untilCaptureTimerMessage: '',
        notificationText: 'вы успешно сдали на прававы успешно сдали на права вы успешно сдали',
        notificationShow: false,
        driftScore: {
            show: false, //nn
            value: 1300,
            factor: 24,
            // rotateDeg максимум 30, минимум -30
            rotateDeg: 29
        },
        levelUp: {
            show: false,//nn
            currentLevel: 12,
            currentExp: 10,
            maxExp: 3,
            exp: 20,
        },
        tip: {
            show: false,
            text: 'Пример сообщения игроку'
        },
        phaseTimer: {
            show: false,
            title: 'Phase 2',
            time: 1400,
            alive: 50,
            kills: 0
        },
        winNotification: {
            show: false,
            nickname: 'Vasya Pupkin',
            itemId: 4006,
        },
        remoteController: {
            show: false,
            vehicleId: 0,
            backLeftDoor: false,
            backRightDoor: false,
            frontLeftDoor: false,
            frontRightDoor: false,
            hood: false,
            trunk: false,
            bibi: false,
            aim: false,
            evacuation: false,
            engine: false,
            signalization: false,
            centralLock: false,
        },
        warZone: {
            show: false,
            key: 20
        },
        holeInformation: {
            show: false,
            itemInfo: {
                isMy: true,
                name: 'Помидор',
                icon: 'tomato.png',
                isWatered: true,
                fertilizer: 'Топовое',
                hole: 'Быстрая',
            },
            stateInfo: {
                show: true,
                isWithers: true,
                value: 120,
                maxValue: 180
            }
        },
        transferReport: {
            show: true,
            maxReports: 50
        },
        biginfo: null
    },
    mutations: {
        showBigInfo(state, info){
            state.biginfo = info;
        },
        hideBigInfo(state){
            state.biginfo = null;
        },
        toggleTip(state, payload) {
            state.tip.show = payload
        },
        setTipText(state, { text }) {
            state.tip.text = text
        },
        setMinimapSize(state, payload) {
          state.minimap = payload  
        },
        addKillogItem(state, item) {
            if (state.killogItems.length >= 5) {
                state.killogItems.splice(0, 1);
                state.killogItems.push(item);
            } else {
                state.killogItems.push(item);
            }
        },
        resetArrestTimer(state){
            state.artimer = undefined;
        },
        setArrestTimer(state, {time, reason}){
            state.artimer = {
                expiried: new Date(time + " UTC"),
                reason
            };
        },
        resetKillog(state) {
            state.killogItems = [];
        },
        showKillog(state, val) {
            state.isKillog = val;
        },
        setVoiceState(state, val) {
            state.voiceState = val;
        },
        toggleGreenZone(state, toggle) {
            state.isGreenZone = toggle;
        },
        
        toggleLevelUpShow(state, toggle) {
            state.levelUp.show = toggle;
        },
        setLevelUp(state, { lvl, exp }) {
            state.levelUp.currentLevel = lvl;
            state.levelUp.currentExp = exp;
            state.levelUp.maxExp = 3 + state.levelUp.currentLevel * 3;
        },
        setLevel(state, lvl) {
            state.levelUp.maxExp = 3 + state.levelUp.currentLevel * 3;
            state.levelUp.exp = 0;
            state.levelUp.currentExp = 0;
            state.levelUp.currentLevel = lvl;
        },
        setNextExp(state, exp) {
            state.levelUp.exp = exp - state.levelUp.currentExp;
            state.levelUp.currentExp = exp;
        },
        
        /* Speaker methods */        
        addSpeakItem(state, { id, name }) {
            let item = {
                id: id,
                name: name
            }
            const rightIndex = state.realSpeakItems.findIndex(i => i.id === item.id)
            if (rightIndex > -1) return;
            
            state.realSpeakItems.push(item)
            state.speakItems = state.realSpeakItems.slice(0, 5);
        },
        removeSpeakItem(state, id) {
            const rightIndex = state.realSpeakItems.findIndex(item => item.id === id)
            if (rightIndex === -1) return;
            
            state.realSpeakItems.splice(rightIndex, 1)
            state.speakItems = state.realSpeakItems.slice(0, 5);
        },
        // --------------- //
        setTime: (state, {time, date}) => {
            state.time = time;
            state.date = date;
        },
        updateData(state, {name, value}){
            //window.console.log(`Check ${name} - ${value}`)
            if (name === 'money'){
                state.moneyChange = value - state.money;
                setTimeout(() => {
                    state.moneyChange = 0;
                }, 4000)
                state.money = parseInt(value);
             } else if(state[name] != undefined) state[name] = value;
        },
        showHelp(state, value){
            value = false;
            state.helpShow = value;
        },
        showHud(state, value){
            state.hudShow = value;
        },

        setPromptData: function(state, data) {
            state.promptData = data;
        },

        setKillstatItems: function (state, items) {
            const rightItems = items.sort(sortFunc)

            function sortFunc (a, b) {
                if (b.kills === a.kills) {
                    return a.deaths - b.deaths
                } else {
                    return b.kills - a.kills
                }
            }

            state.killstatItems = rightItems
        },
        
        setKillstatType: function (state, type) {
          state.killstatType = type.value;  
        },
        
        setKillstatCurrentUser: function (state, user) {
            state.killstatCurrentUser = user
        },

        startKillstatTimer: function (state, value) {
            state.killStatTime = value
            if (!state.killStatTimer) {
                state.killStatTimer = setInterval(() => {
                    if (state.killStatTime > 0) {
                        state.killStatTime--
                    } else {
                        clearInterval(state.killStatTimer)
                        state.killStatTimer = null
                    }
                }, 1000)
            }
        },

        stopKillstatTimer: function (state) {
            clearInterval(state.killStatTimer)
            state.killStatTime = null
            state.killStatTimer = null
        },

        setIsKillStat: function (state, value) {
            state.isKillstat = value
        },

        setIsFullKillStat: function (state, value) {
            state.isFullKillstat = value
        },

        setVoteItems: function (state, items) {
            state.voteItems = items
        },

        setIsVote: function (state, value) {
            state.isVote = value
        },

        startWorkTimer: function(state, value) {
            state.workTime = value
            state.workTimer = setInterval(() => {
                if (state.workTime > 0) {
                    console.log(state.workTimer)
                    state.workTime--
                } else {
                    clearInterval(state.workTimer)
                    state.workTimer = null
                    state.workTime = null
                }
            }, 1000)
        },

        stopWorkTimer: function(state) {
            clearInterval(state.workTimer)
            state.workTimer = null
            state.workTime = null
        },
        setWantedLevels: function(state, value) {
            state.wantedLevels = value
        },
        // SET CHIPS LIST ITEM
        setChipValue: function(state, { type, value }) {
            state.chipsList[type] = value
        },
        subChipValue: function(state, type) {
            state.chipsList[type] -= 1;
        },
        setIsChipsList: function(state, value) {
            state.isChipsList = value
        },
        // CAPTURE

        appendCaptureLog: function(state, item) {
            if (state.captureLog.length >= 5) {
                state.captureLog.shift()
            }
            state.captureLog.push(item)
        },
        setIsCaptureLog: function(state, value) {
            state.isCaptureLog = value
        },
        resetCaptureLogItems: function(state) {
            state.captureLog = [];
        },
        
        setCaptureTeams: function(state, teams) {
            state.captureTeams = teams
        },
        setCaptureTeamPlayers: function(state, { team, value }) {
            const rigthIndex = state.captureTeams.findIndex(item => item.key === team)
            state.captureTeams[rigthIndex].players = value
        },
        setIsCaptureItems: function(state, value) {
            state.isCaptureItems = value
        },
        setCaptureCurrentTime: function(state, value) {
            state.captureTimeCurrent = value
        },
        // startCaptureTimer: function(state, { limit, current }) {
        //     state.captureTimeLimit = limit
        //     state.captureTimeCurrent = current
        //     if (!state.captureTimer) {
        //         state.captureTimer = setInterval(() => {
        //             state.captureTimeCurrent ++
        //         }, 1000)
        //     }
        // },
        // stopCaptureTimer: function(state) {
        //     clearInterval(state.captureTimer)
        //     state.captureTimeLimit = null
        //     state.captureTimeCurrent = null
        // },

        setReportsCount: function(state, value) {
            if (value == 0) {
                value = null;
            }

            state.reportsCount = value
        },

        setStatusDisplayShow: function(state, isShow) {
            state.statusDisplayShow = isShow;
        },

        setStatusDisplay: function(state, { key, value }) {
            state.statusDisplays[key] = value;
        },
        
        setUntilCaptureTimer: function(state, { maxTime, currentTime }) {
            state.untilCaptureTimerMaxTime = maxTime;
            state.untilCaptureTimerCurrentTime = currentTime;
        },
        setUntilCaptureTimerShow: function(state, value) {
            state.untilCaptureTimerEnabled = value;
        },
        setUntilCaptureTimerMessage: function(state, value) {
            state.untilCaptureTimerMessage = value;
        },
        setNotificationShow: function(state, text) {
            state.notificationText = text
            state.notificationShow = true
            setTimeout(function(){
                state.notificationShow = false
            }, 3000)
        },
        setDriftScore(state, value){
            state.driftScore = value
        },
        setPhaseTimer(state, value){
            state.phaseTimer.show = value.show;
            state.phaseTimer.title = value.title;
            state.phaseTimer.time = value.time;
        },
        setRoyalBattleStats(state, value) {
            state.phaseTimer.alive = value.alive;
            state.phaseTimer.kills = value.kills;
        },
        showWinNotification(state, {name, id}){
            state.winNotification.nickname = name;
            state.winNotification.itemId = id;
            if(state.winNotification.show) return;
            state.winNotification.show = true;
            setTimeout(()=>{
                state.winNotification.show = false;
            }, 8000)
        },
        openKey(state, data){
            state.remoteController = data;
        },
        setWarZoneShow(state, value) {
            state.warZone.show = value;
        },
        setHoleInformation(state, data) {
            state.holeInformation.show = data.show;
            state.holeInformation.itemInfo = data.itemInfo;
            state.holeInformation.stateInfo= data.stateInfo
        }
    },
    actions: {
    },
    modules: {
        warForEnterprice
    }
}
  