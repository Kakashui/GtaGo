const currentUI = {
    firstTeamID: 1,
    secondTeamID: 1
};

const fractions = {
    [1]: { key: 'families', name: 'The Families' },
    [2]: { key: 'ballas', name: 'Ballas Gang' },
    [3]: { key: 'vagos', name: 'The Vagos' },
    [4]: { key: 'marabunta', name: 'Marabunta' },
    [5]: { key: 'bloods', name: 'Bloods Street' },
    [10]: { key: 'lcn', name: 'La Cosa Nostra' },
    [11]: { key: 'mexican', name: 'Mexican Mafia' },
    [12]: { key: 'yakuza', name: 'Yakuza' },
    [13]: { key: 'bloods', name: 'Amogebuli Mafia' },
    [100]: { key: 'attackers', name: 'Attackers' },
    [200]: { key: 'defenders', name: 'Defenders' },    
};

let opened = false;
let currTime = 0;
let captureTimer = null;

function getFractionKey(fractionId) {
    return fractions[fractionId] ? fractions[fractionId].key : 'neutral';
}

mp.events.add({
    // CAPTURE UI
    "captureUI:enable": (firstTeam, secondTeam, currentTime, firstTeamPlayers, secondTeamPlayers, isGangCapture) => {
        const teamsDto = [
            { id: 0, key: fractions[firstTeam].key, title: fractions[firstTeam].name, players: firstTeamPlayers },
            { id: 1, key: fractions[secondTeam].key, title: fractions[secondTeam].name, players: secondTeamPlayers },
        ];

        currentUI.firstTeamID = firstTeam;
        currentUI.secondTeamID = secondTeam;

        global.gui.setData('hud/setCaptureTeams', JSON.stringify(teamsDto));
        global.gui.setData('hud/setIsCaptureItems', true);
        currTime = currentTime;
        global.gui.setData('hud/setCaptureCurrentTime', currentTime);
        if (captureTimer === null)
        {
            if (isGangCapture) {
                captureTimer = setInterval(gangCaptureTimer, 1000);
            }
            else {
                captureTimer = setInterval(bizWarTimer, 1000);
            }
        }
    },

    "captureUI:setStats": (firstTeamPlayers, secondTeamPlayers, currentTime) => {
        global.gui.setData('hud/setCaptureTeamPlayers', JSON.stringify({ team: fractions[currentUI.firstTeamID].key, value: firstTeamPlayers }));
        global.gui.setData('hud/setCaptureTeamPlayers', JSON.stringify({ team: fractions[currentUI.secondTeamID].key, value: secondTeamPlayers }));
        currTime = currentTime;
    },

    "captureUI:disable": () => {
        global.gui.setData('hud/setIsCaptureItems', false);
        if (captureTimer !== null)
        {
            clearInterval(captureTimer);
            captureTimer = null;
        }
    },


    // CAPTURE LOG
    "captureUI:log:enable": () => {
        global.gui.setData('hud/setIsCaptureLog', true);
    },

    "captureUI:log:disable": (reset) => {
        global.gui.setData('hud/setIsCaptureLog', false);

        if (reset) {
            global.gui.setData('hud/resetCaptureLogItems');
        }
    },

    "captureUI:log:append": (killerName, killerFraction, victimName, victimFraction, weaponId) => {
        global.gui.setData('hud/appendCaptureLog', JSON.stringify({
            killerName: killerName,
            killerFraction: getFractionKey(killerFraction),
            deceasedFraction: getFractionKey(victimFraction),
            deceasedName: victimName,
            weaponId: weaponId
        }));
    },


    // UNTIL CAPTURE TIMER
    "captureUI:untilCapt:send": (maxTime, currentTime, message) => {
        if (untilCapture.timer != null) {
            clearInterval(untilCapture.timer);
        }

        untilCapture.currentTime = currentTime;
        untilCapture.maxTime = maxTime;
        untilCapture.timer = setInterval(untilCaptureTimer, 1000);
        
        sendUntilCaptureToCef();
        global.gui.setData("hud/setUntilCaptureTimerShow", true);
        global.gui.setData("hud/setUntilCaptureTimerMessage", message);
    },

    "captureUI:untilCapt:disable": () => {
        global.gui.setData("hud/setUntilCaptureTimerShow", false);

        if (untilCapture.timer != null) {
            clearInterval(untilCapture.timer);
            untilCapture.timer = null;
        }
    }
});

const untilCapture = {
    timer: null,
    currentTime: 0,
    maxTime: 500
};

function sendUntilCaptureToCef() {
    global.gui.setData("hud/setUntilCaptureTimer", JSON.stringify({
        maxTime: untilCapture.maxTime,
        currentTime: untilCapture.maxTime - untilCapture.currentTime
    }));
}

function untilCaptureTimer() {
    untilCapture.currentTime++;
    if (untilCapture.maxTime >= untilCapture.currentTime)
        sendUntilCaptureToCef();
    else
    {
        global.gui.setData("hud/setUntilCaptureTimerShow", false);
        if (untilCapture.timer != null) {
            clearInterval(untilCapture.timer);
        }
        untilCapture.timer = null;
    }

}

function gangCaptureTimer()
{
    if (currTime >= 0)
        global.gui.setData('hud/setCaptureCurrentTime', currTime--);
}

function bizWarTimer()
{
    if (currTime >= 0)
        global.gui.setData('hud/setCaptureCurrentTime', currTime++);
}


mp.events.add("capt:openTeamMenu", (members, fracID, enemyFracID, attackStatus) => {
    if(!global.gui.openPage('Captures')) return;
    global.gui.setData('captures/setCapturing', JSON.stringify({
        attackStatus: attackStatus ? 'we' : 'us', // us
        myGangName: fractions[fracID] ? fractions[fracID].name : '',
        enemy: fractions[enemyFracID] ? fractions[enemyFracID].name : ''
    }));

    let memberList = JSON.parse(members);
    let gangList = memberList.filter(item => item.State != 2);
    let teamList = memberList.filter(item => item.State == 2);

    global.gui.setData('captures/setGangList', JSON.stringify(
        gangList.map(item => (
            { nickname: item.Name, rang: item.Rank, status: item.State == 1 }
        ))
    ));

    global.gui.setData('captures/setTeamList', JSON.stringify(
        teamList.map(item => (
            { nickname: item.Name, rang: item.Rank }
        ))
    ));

    opened = true;
    setTimeout(() => {
        global.showCursor(true);
    }, 100);
});

mp.events.add("capt:inviteMember", (name) => {
    mp.events.callRemote('capt:srvInviteMember', name);
});

mp.events.add("capt:kickMember", (name) => {
    mp.events.callRemote('capt:srvKickMember', name);
});

mp.events.add("capt:setMember", (name, rank, state) => {
    if (state != 3)
        global.gui.setData('captures/setPlayerState', JSON.stringify(
            { nickname: name, rang: rank, status: state }
        ));
    else
        global.gui.setData('captures/setDeletePlayer', JSON.stringify(name));
});

mp.events.add("capt:attack", () => {
    mp.events.callRemote('capt:attackToEnemy');
});

mp.events.add("capt:deff", () => {
    mp.events.callRemote('capt:deffendRegion');
});

mp.events.add("capt:closeMenu", () => {
    if (opened)
        closeCaptMenu();
});

function closeCaptMenu() {
    global.gui.close();
    mp.events.callRemote('capt:exitTeamMenu');
    opened = false;
    global.showCursor(false)
}

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        closeCaptMenu();
});


