let isPlacing = false;
let boardPivotPoint;
let boardHeight = 0.95;
let chipVelocity = 0.003;
let chipAcceleration = 1;
let rouletteCam = null;
let dimension = 0;
let boardPeds = {};
let playingGameId = -1;
let loaded = false;
let isAtTable = false;
let chairIndex = -1;
let ball;
let lastResult;
let chipsFromServer = [0, 0, 0, 0, 0];

mp.events.add('roulette:updatePlayerBank', (totalBank, chipsData) => {
    let dto = JSON.parse(chipsData);
    chipsFromServer[0] = dto.Black;
    chipsFromServer[1] = dto.Red;
    chipsFromServer[2] = dto.Blue;
    chipsFromServer[3] = dto.Green;
    chipsFromServer[4] = dto.Yellow;
})

mp.events.add('player:loadedRoulleteBoard', (data) => {
    let tables = JSON.parse(data);
    let board = null;
    tables.forEach(table => {
        switch (table.type) {
            case "Roulette":            
                board = mp.objects.new(mp.game.joaat('vw_prop_casino_roulette_01'), table.position,
                    {
                        rotation: table.rotation,
                        alpha: 255,
                        dimension: table.dimension
                    });
                board.setCollision(true, true);
                global.boardObjects[table.id] = board;
            
                let ped = mp.peds.newValid('mp_f_execpa_02', new mp.Vector3(table.position.x, table.position.y + 0.55, table.position.z + 1), 177.7, 0);
                if (ped != null)
                    boardPeds[table.id] = ped;
                break;
            case "Poker":
                board = mp.objects.new(mp.game.joaat('poker_table_gta_go'), table.position, {
                   rotation: table.rotation,
                   alpha: 255,
                   dimension: table.dimension
                });
                if(board){
                    board.info = table.message;
                    global.boardObjects[table.id] = board;
                }
                                
                break;
        }
    });

    mp.game.streaming.requestAnimDict("anim_casino_b@amb@casino@games@roulette@table")
    mp.game.streaming.requestAnimDict("anim_casino_b@amb@casino@games@shared@player@");
    mp.game.streaming.requestAnimDict("anim_casino_b@amb@casino@games@roulette@dealer");

    mp.peds.newValid('mp_f_execpa_02', new mp.Vector3(1117.406, 220.53, -49.55516), 85, 0);
    mp.peds.newValid('s_m_y_barman_01', new mp.Vector3(1110.982, 209.5823, -49.55), 30, 0);

    let phIntID = mp.game.interior.getInteriorAtCoords(1117.755, 220.1409, -49.55515);
    mp.game.interior.refreshInterior(phIntID);
    loaded = true;
});

let wheelResults = [19, 31, 18, 6, 21, 33, 16, 4, 23, 35, 14, 2, 0, 28, 9, 26, 30, 11, 7, 20, 32, 17, 5, 22, 34, 15, 3, 24, 36, 13, 1, 37, 27, 10, 25, 29, 12, 8];

mp.events.add('roulette:wheelAnim', (flag, result) => {
    lastResult = result;
    let bone = global.boardObjects[playingGameId].getWorldPositionOfBone(global.boardObjects[playingGameId].getBoneIndexByName("Roulette_Wheel"));

    if (flag) {
        if (ball != null) {
            ball.destroy();
            ball = null;
        }
        ball = mp.objects.new(mp.game.joaat('vw_prop_roulette_ball'), bone);
        global.boardObjects[playingGameId].playAnim("intro_wheel", "anim_casino_b@amb@casino@games@roulette@table", 1000, true, true, false, 0, 5642);
        ball.playAnim("intro_ball", "anim_casino_b@amb@casino@games@roulette@table", 1000, true, false, false, 0, 136704);//136704
        boardPeds[playingGameId].taskPlayAnimAdvanced("anim_casino_b@amb@casino@games@roulette@dealer", "spin_wheel", boardPivotPoint.x + 0.207, boardPivotPoint.y - 0.075, bone.z - 1, 0, 0, 0, -8, 1, -1, 5642, 0, 2, 0);
    }
    playBallAnim(flag);
});

function playBallAnim(flag) {
    if (flag) {
        let timer = setInterval(() => {
            if (!isAtTable) return;
            let bone = global.boardObjects[playingGameId].getWorldPositionOfBone(global.boardObjects[playingGameId].getBoneIndexByName("Roulette_Wheel"));
            ball.position = bone;
            ball.rotation = new mp.Vector3(0, 0, 15);
            global.boardObjects[playingGameId].playAnim(`exit_${wheelResults.lastIndexOf(lastResult)}_wheel`, "anim_casino_b@amb@casino@games@roulette@table", 1000, false, true, false, 0, 5642);
            ball.playAnim(`exit_${wheelResults.lastIndexOf(lastResult)}_ball`, "anim_casino_b@amb@casino@games@roulette@table", 1000, false, false, false, 0, 5642);

            clearInterval(timer);
        }, 6000);
    }
    else {
        boardPeds[playingGameId].taskPlayAnimAdvanced("anim_casino_b@amb@casino@games@roulette@dealer", "clear_chips_zone1", boardPivotPoint.x, boardPivotPoint.y, boardPivotPoint.z, 0, 0, 0, -8, 1, -1, 5642, 0, 2, 0);
        resetBets();
    }
};

let screenCoords = [
    [],
    [],
    [],
    [],
    []
];

mp.events.add('player:enterRoulette', (id, /*balance,*/ chairId) => {
    global.gui.openPage('Roulette');
    mp.events.callRemote('player:requestBank');
    playingGameId = id;
    let pos = global.boardObjects[id].position;
    let rot = global.boardObjects[id].rotation;
    chairIndex = chairId;
    boardPivotPoint = new mp.Vector3(pos.x, pos.y, pos.z);
    fillBets();
    global.showCursor(true);
    isAtTable = true;
    rouletteCam = mp.cameras.new('casino', new mp.Vector3(pos.x, pos.y, pos.z + 2.2), new mp.Vector3(-90, 0, 0), 70);
    rouletteCam.setActive(true);
    mp.game.cam.renderScriptCams(true, true, 1000, false, false);
    sit();
    clearHighlight();
})

mp.events.add('server:updateTimer', (stageDto) => {
    let dto = JSON.parse(stageDto);
    JSON.stringify(dto);
    global.gui.setData('casino/setData', JSON.stringify({ seconds: dto.Seconds, currentStage: dto.Name }));
})

mp.events.add('player:startPlacingBets', () => {
    global.showCursor(true);
    isPlacing = true;
})

mp.events.add('player:endPlacingBets', () => {
    isPlacing = false;
    destroyChipModels();
    clearHighlight();
})

mp.events.add('roulette:requestTimerInfo', () => {
    mp.events.callRemote('player:sentTimerInfo', mp.gui.execute(`rouletteHud.seconds`))
})

let chairPoint;
let chairDirection;
let enterAnimName;
let exitAnimName;

function sit() {
    chairPoint = global.boardObjects[playingGameId].getWorldPositionOfBone(global.boardObjects[playingGameId].getBoneIndexByName(`Chair_Base_0${chairIndex}`));
    chairDirection;
    switch (chairIndex) {
        case 1:
            enterAnimName = "sit_enter_right";
            chairDirection = global.boardObjects[playingGameId].rotation.z + 90;
            break;
        case 2:
            enterAnimName = "sit_enter_right";
            chairDirection = global.boardObjects[playingGameId].rotation.z + 90;
            break;
        case 3:
            enterAnimName = "sit_enter_right";
            chairDirection = global.boardObjects[playingGameId].rotation.z - 180;
            break;
        case 4:
            enterAnimName = "sit_enter_left";
            chairDirection = global.boardObjects[playingGameId].rotation.z + 90;
            break;
    }
    mp.players.local.taskPlayAnimAdvanced("anim_casino_b@amb@casino@games@shared@player@", enterAnimName, chairPoint.x, chairPoint.y, chairPoint.z, 0, 0, chairDirection, -8, 1, -1, 5642, 0, 2, 0);
    syncronizeAnim(true, enterAnimName, 5642, chairPoint.x, chairPoint.y, chairPoint.z);
}

function standUp() {
    switch (chairIndex) {
        case 1:
            exitAnimName = "sit_exit_left";
            break;
        case 2:
            exitAnimName = "sit_exit_left";
            break;
        case 3:
            exitAnimName = "sit_exit_right";
            break;
        case 4:
            exitAnimName = "sit_exit_left";
            break;
    }
    mp.players.local.stopAnim(enterAnimName, "anim_casino_b@amb@casino@games@shared@player@", 0.0);
    mp.events.callRemote("player:stopAnim");
    mp.players.local.taskPlayAnimAdvanced("anim_casino_b@amb@casino@games@shared@player@", exitAnimName, chairPoint.x, chairPoint.y, chairPoint.z, 0, 0, chairDirection, -8, 1, -1, 136704, 0, 2, 0);
    syncronizeAnim(true, exitAnimName, 136704, chairPoint.x, chairPoint.y, chairPoint.z);
    chairIndex = -1;
}

let screenNumbers = [
    [],
    [],
    []
];
let screenBottoms = [];
let screenRows = [];
let screenDozens = [];
let screenZeros = [
    [0.45, 0.44],
    [0.45, 0.58]
]

function fillScreenBets() {
    fillNumberScreenBets();
    fillDozenScreenBets();
    fillBottomScreenBets();
    fillRowScreenBets();
}

let numScreenWidth = 0.028;
let numScreenHeight = 0.080;
let botScreenwidth = 2 * numScreenWidth;

function fillNumberScreenBets() {
    let num3 = [0.48, 0.42];
    for (let i = 0; i < 3; i++) {
        for (let j = 0; j < 12; j++) {
            screenNumbers[i][j] = screenCalc(num3, j, i);
        }
    }
}

function fillBottomScreenBets() {
    let num18 = [0.48, 0.72];
    for (let i = 0; i < 6; i++) {
        screenBottoms[i] = [num18[0] + (i * botScreenwidth), num18[1]];
    }
}

function fillRowScreenBets() {
    let row1 = [0.79, 0.42];
    for (let i = 0; i < 3; i++) {
        screenRows[i] = [row1[0], row1[1] + (i * numScreenHeight)];
    }
}

function fillDozenScreenBets() {
    let d1 = [0.52, 0.67];
    for (let i = 0; i < 3; i++) {
        screenDozens[i] = [d1[0] + (i * 2 * botScreenwidth), d1[1]];
    }
}

function clearHighlight() {
    for (let i = 0; i < 3; i++) {
        for (let j = 0; j < 12; j++) {
            markers[i][j].setVisible(false, false);
        }
    }
    zeroMarkers.forEach(x => x.setVisible(false, false));
}

let res = mp.game.graphics.getScreenActiveResolution(0, 0);
let aspectRatio = mp.game.invoke('0xF1307EF624A80D87', false);

mp.events.add('click', (x, y, upOrDown, leftOrRight, relativeX, relativeY, worldPosition, hitEntity) => {
    if (!isPlacing) return;
    if (leftOrRight == 'right') {
        if (upOrDown == 'up') {
            cancelLastBet();
        }
        return;
    }
    if (chipModels.filter(c => c.length != 0).length == 0) return;

    if (bet === undefined) return;

    if (upOrDown == 'up') {
        if (parseInt(bet) >= 0 || parseInt(bet) <= 0 || bet.length == 2) {//if number
            let index = (bet[1] + 1) * 3 - bet[0];
            placeBet(index);
        }
        else {
            placeBet(bet);
        }
    }
});

function screenRelToWorld(camPos, camRot, screenCoords) {
    let camForward = rotationToDirection(camRot);
    let rotUp = camRot + new mp.Vector3(10, 0, 0);
    let rotDown = camRot + new mp.Vector3(-10, 0, 0);
    let rotLeft = camRot + new mp.Vector3(0, 0, -10);
    let rotRight = camRot + new mp.Vector3(0, 0, 10);

    let camRight = rotationToDirection(rotRight) - rotationToDirection(rotLeft);
    let camUp = rotationToDirection(rotUp) - rotationToDirection(rotDown);

    let rollRad = -degToRad(camRot.y);

    let camRightRoll = camRight * Math.cos(rollRad) - camUp * Math.sin(rollRad);
    let camUpRoll = camRight * Math.sin(rollRad) + camUp * Math.cos(rollRad);

    let point3d = camPos + camForward * 10.0 + camRightRoll + camUpRoll;

    let point2d = worldToScreenRel(point3d);
    if (!flag) return camPos + camForward * 10.0;
    let point3dzero = camPos + camForward * 10.0;
    let point2dzero = worldToScreenRel(point3dzero);
    if (!flag) return camPos + camForward * 10.0;

    const eps = 0.001;
    if (Math.abs(point2d.x - point2dzero.x) < eps || Math.abs(point2d.y - point2dzero.y) < eps) return camPos + camForward * 10.0;
    let scaleX = (screenCoords.x - point2dzero.x) / (point2d.x - point2dzero.x);
    let scaleY = (screenCoords.y - point2dzero.y) / (point2d.y - point2dzero.y);
    let point3Dret = camPos + camForward * 10.0 + camRightRoll * scaleX + camUpRoll * scaleY;
    return point3Dret;
}

let flag;
function worldToScreenRel(worldCoords) {
    let x = 0;
    let y = 0;
    let screenCoords;
    if (!mp.game.invoke('0x34E82F05DF2974F5', worldCoords.x, worldCoords.y, worldCoords.z, x, y)) {
        screenCoords = new mp.Vector3();
        flag = false;
        return screenCoords;
    }
    screenCoords = new mp.Vector3((x - 0.5) * 2, (y - 0.5) * 2, 0);
    flag = true;
    return screenCoords;
}

function degToRad(deg) {
    return deg * Math.PI / 180.0;
}
function rotationToDirection(rotation) {
    let z = degToRad(rotation.z);
    let x = degToRad(rotation.x);
    let num = Math.abs(Math.cos(x));
    let v = new mp.Vector3(-Math.sin(z) * num, Math.cos(z) * num, Math.sin(x))
    return v;
}
global.colors = [//todo: one line array
    [1, "red"],
    [2, "black"],
    [3, "red"],
    [4, "black"],
    [5, "red"],
    [6, "black"],
    [7, "red"],
    [8, "black"],
    [9, "red"],
    [10, "black"],
    [11, "black"],
    [12, "red"],
    [13, "black"],
    [14, "red"],
    [15, "black"],
    [16, "red"],
    [17, "black"],
    [18, "red"],
    [19, "red"],
    [20, "black"],
    [21, "red"],
    [22, "black"],
    [23, "red"],
    [24, "black"],
    [25, "red"],
    [26, "black"],
    [27, "red"],
    [28, "black"],
    [29, "black"],
    [30, "red"],
    [31, "black"],
    [32, "red"],
    [33, "black"],
    [34, "red"],
    [35, "black"],
    [36, "red"],
    [37, "green"],
    [38, "green"],
];

let ratioFactor = aspectRatio / 1070386381;

let bet;
mp.events.add('render', () => {
    if (!isPlacing) return;

    let mouseRel = { x: mp.gui.cursor.position[0] / res.x, y: mp.gui.cursor.position[1] / res.y };

    bet = detectBet(mouseRel.x, mouseRel.y);
    clearHighlight();

    if (bet == undefined) return;
    if (parseInt(bet) >= 0 || parseInt(bet) <= 0) {//if number
        if (bet == 0) return;
        markers[2 - bet[0]][11 - bet[1]].setVisible(true, false);
        setChipPosition(markers[2 - bet[0]][11 - bet[1]].position.x, markers[2 - bet[0]][11 - bet[1]].position.y);
        return;
    }
    if (bet.includes('firstrow')) {
        i = 0;
        for (let j = 0; j < 12; j++) {
            markers[2 - i][11 - j].setVisible(true, false);
            setChipPosition(markers[2 - i][0].position.x + numWidth, markers[2 - i][0].position.y);
        }
        return;
    }
    if (bet.includes('secondrow')) {
        i = 1;
        for (let j = 0; j < 12; j++) {
            markers[2 - i][11 - j].setVisible(true, false);
            setChipPosition(markers[2 - i][0].position.x + numWidth, markers[2 - i][0].position.y);
        }
        return;
    }
    if (bet.includes('thirdrow')) {
        i = 2;
        for (let j = 0; j < 12; j++) {
            markers[2 - i][11 - j].setVisible(true, false);
            setChipPosition(markers[2 - i][0].position.x + numWidth, markers[2 - i][0].position.y);
        }
        return;
    }
    if (bet.includes('firstdozen')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 0; j < 4; j++) {
                markers[2 - i][11 - j].setVisible(true, false);
                setChipPosition(markers[0][10].position.x + numWidth / 2, markers[0][11].position.y - numHeight / 2 - dozenBetHeight / 1.5);
            }
        }
        return;
    }
    if (bet.includes('seconddozen')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 4; j < 8; j++) {
                markers[2 - i][11 - j].setVisible(true, false);
                setChipPosition(markers[0][6].position.x + numWidth / 2, markers[0][6].position.y - numHeight / 2 - dozenBetHeight / 1.5);
            }
        }
        return;
    }
    if (bet.includes('thirddozen')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 8; j < 12; j++) {
                markers[2 - i][11 - j].setVisible(true, false);
                setChipPosition(markers[0][2].position.x + numWidth / 2, markers[0][3].position.y - numHeight / 2 - dozenBetHeight / 1.5);
            }
        }
        return;
    }
    if (bet.includes('even')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 0; j < 12; j++) {
                let index = (11 - j) * 3 - 2 - i;
                if (index % 2 === 0) {
                    markers[2 - i][11 - j].setVisible(true, false);
                    setChipPosition(markers[0][9].position.x + numWidth / 2, markers[0][9].position.y - numHeight - numWidth / 2);
                }
            }
        }
        return;
    }
    if (bet.includes('odd')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 0; j < 12; j++) {
                let index = (11 - j) * 3 - 2 - i;
                if (index % 2 !== 0) {
                    markers[2 - i][11 - j].setVisible(true, false);
                    setChipPosition(markers[0][3].position.x + numWidth / 2, markers[0][3].position.y - numHeight - numWidth / 2);
                }
            }
        }
        return;
    }
    if (bet.includes('red')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 0; j < 12; j++) {
                let index = (j + 1) * 3 - i;
                if (colors[index - 1][1] == "red") {
                    markers[2 - i][11 - j].setVisible(true, false);
                    setChipPosition(markers[0][7].position.x + numWidth / 2, markers[0][3].position.y - numHeight - numWidth / 2);
                }
            }
        }
        return;
    }
    if (bet.includes('black')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 0; j < 12; j++) {
                let index = (j + 1) * 3 - i;
                if (colors[index - 1][1] == "black") {
                    markers[2 - i][11 - j].setVisible(true, false);
                    setChipPosition(markers[0][5].position.x + numWidth / 2, markers[0][3].position.y - numHeight - numWidth / 2);
                }
            }
        }
        return;
    }
    if (bet.includes('firsthalf')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 0; j < 6; j++) {
                markers[2 - i][11 - j].setVisible(true, false);
                setChipPosition(markers[0][11].position.x + numWidth / 2, markers[0][3].position.y - numHeight - numWidth / 2);
            }
        }
        return;
    }
    if (bet.includes('lasthalf')) {
        for (let i = 0; i < 3; i++) {
            for (let j = 6; j < 12; j++) {
                markers[2 - i][11 - j].setVisible(true, false);
                setChipPosition(markers[0][1].position.x + numWidth / 2, markers[0][3].position.y - numHeight - numWidth / 2);
            }
        }
        return;
    }
    if (bet == 'zero') {
        zeroMarkers[0].setVisible(true, false);
        setChipPosition(markers[0][11].position.x - 1.2 * numWidth, markers[0][11].position.y + numWidth / 2);
        return;
    }
    if (bet == 'dzero') {
        zeroMarkers[1].setVisible(true, false);
        setChipPosition(markers[2][11].position.x - 1.2 * numWidth, markers[2][11].position.y - numWidth / 2);
        return;
    }
});

function detectBet(x, y) {
    let distx;
    let disty;
    for (let i = 0; i < 3; i++) {
        //-------cornerbets-----
        // for(let j = 0; j < 12; j++) {
        //     distx = Math.abs(screenNumbers[i][j][0] - x);
        //     disty = Math.abs(screenNumbers[i][j][1] - y);
        //     if (j < 11 && i < 2){
        //         distx2 = Math.abs(screenNumbers[i][j+1][0] - x);
        //         disty2 = Math.abs(screenNumbers[i][j+1][1] - y);
        //         distx3 = Math.abs(screenNumbers[i+1][j][0] - x);
        //         disty3 = Math.abs(screenNumbers[i+1][j][1] - y);
        //         if (Math.abs(distx - distx2) < 0.004 && Math.abs(disty - disty3) < 0.005){
        //             return [i,j,i,j+1,i+1,j,i+1,j+1];
        //         }
        //     }
        //     // if (j > 0 && i > 0){
        //     //     distx2 = Math.abs(screenNumbers[i][j-1][0] - x);
        //     //     disty2 = Math.abs(screenNumbers[i][j-1][1] - y);
        //     //     if (Math.abs(distx - distx2) < 0.004 && disty == disty2 && disty < numScreenHeight){
        //     //         return [i,j,i,j-1];
        //     //     }
        //     // }            
        // }
        //------numbers----------
        for (let j = 0; j < 4; j++) {
            distx = Math.abs(screenNumbers[i][j][0] - x);
            disty = Math.abs(screenNumbers[i][j][1] - y);
            if (distx < numScreenWidth - 0.014 * ratioFactor && disty < numScreenHeight) {
                return [i, j];
            }
        }

        for (let j = 4; j < 8; j++) {
            distx = Math.abs(screenNumbers[i][j][0] - x);
            disty = Math.abs(screenNumbers[i][j][1] - y);
            if (distx < numScreenWidth - 0.014 && disty < numScreenHeight) {
                return [i, j];
            }
        }
        for (let j = 8; j < 12; j++) {
            distx = Math.abs(screenNumbers[i][j][0] - x);
            disty = Math.abs(screenNumbers[i][j][1] - y);
            if (distx < numScreenWidth - 0.014 && disty < numScreenHeight) {
                return [i, j];
            }
            // //double
            // distx = Math.abs(screenNumbers[i][j][0] - 0.018 - x);
            // disty = Math.abs(screenNumbers[i][j][1] - y);
            // if (j < 11){
            //     distx2 = Math.abs(screenNumbers[i][j+1][0] - x);
            //     disty2 = Math.abs(screenNumbers[i][j+1][1] - y);
            //     if (distx - distx2 < 0.004 && distx - distx2 > 0 && disty == disty2 && disty < numScreenHeight){
            //         return [i,j,i,j+1];
            //     }
            // }
            // if (j > 0){
            //     distx2 = Math.abs(screenNumbers[i][j-1][0] - 0.018 - x);
            //     disty2 = Math.abs(screenNumbers[i][j-1][1] - y);
            //     if (distx - distx2 < 0.004 && distx - distx2 > 0 && disty == disty2 && disty < numScreenHeight){
            //         return [i,j,i,j-1];
            //     }                
            // }
            // if (i < 2){
            //     distx2 = Math.abs(screenNumbers[i+1][j][0] - 0.018 - x);
            //     disty2 = Math.abs(screenNumbers[i+1][j][1] - y);
            //     if (Math.abs(disty - disty2) < 0.006 && distx == distx2 && distx < numScreenWidth + 0.014){
            //         return [i,j,i+1,j];
            //     }
            // }
            // if (i > 0){
            //     distx2 = Math.abs(screenNumbers[i-1][j][0] - 0.018 - x);
            //     disty2 = Math.abs(screenNumbers[i-1][j][1] - y);
            //     if (Math.abs(disty - disty2) < 0.006 && distx == distx2 && distx < numScreenWidth + 0.014){
            //         return [i,j,i-1,j];
            //     }
            // }
        }
        //-----------------
        distx = Math.abs(screenDozens[i][0] - 0.018 - x);
        disty = Math.abs(screenDozens[i][1] - y);
        if (distx < 4 * numScreenWidth - 0.014 && disty < numScreenWidth / 2) {
            switch (i) {
                case 0:
                    return 'firstdozen';
                case 1:
                    return 'seconddozen';
                case 2:
                    return 'thirddozen';
            }
        }
        distx = Math.abs(screenRows[i][0] - x);
        disty = Math.abs(screenRows[i][1] - y);
        if (distx < numScreenWidth - 0.001 && disty < numScreenHeight) {
            switch (i) {
                case 0:
                    return 'firstrow';
                case 1:
                    return 'secondrow';
                case 2:
                    return 'thirdrow';
            }
        }
    }
    for (let k = 0; k < 6; k++) {
        distx = Math.abs(screenBottoms[k][0] - 0.014 - x);
        disty = Math.abs(screenBottoms[k][1] - y);
        if (distx < 2 * numScreenWidth - 0.014 && disty < numScreenWidth) {
            switch (k) {
                case 0:
                    return 'firsthalf';
                case 1:
                    return 'even';
                case 2:
                    return 'red';
                case 3:
                    return 'black';
                case 4:
                    return 'odd';
                case 5:
                    return 'lasthalf';
            }
        }
    }
    distx = Math.abs(screenZeros[0][0] - x);
    disty = Math.abs(screenZeros[0][1] - y);
    if (distx < 2 * numScreenWidth - 0.01 && disty < numScreenHeight - 0.03) {
        return 'dzero';
    }
    distx = Math.abs(screenZeros[1][0] - x);
    disty = Math.abs(screenZeros[1][1] - y);
    if (distx < 2 * numScreenWidth - 0.01 && disty < numScreenHeight - 0.03) {
        return 'zero';
    }
}

function fillBets() {
    fillNumberBets();
    fillBottomBets();
    fillDozens();
    fillRowBets();

    fillMarkers();
    fillScreenBets()
}

let n_3 = [-0.05700, 0.134];
let numWidth = 0.08;
let numHeight = 0.14;
let betsNum = [
    [],
    [],
    []
];

let n_1_18 = [-0.0155, -0.3684]
let bottomBetHeight = numWidth;
let bottomBetWidth = numHeight;
let betsBottom = [];

let betsZeros = [
    [-0.1434, 0.0941],
    [-0.1434, -0.1476]
];
let zerosBetHeight = numHeight * 1.5;
let zerosBetwidth = numWidth * 2;

let firstDozen = [0.0693, -0.2953];
let dozenBetWidth = 2 * bottomBetWidth;
let dozenBetHeight = 0.0568;
let betsDozen = [];

let firstRowBet = [0.9045, 0.1305];
let rowHeight = numHeight;
let rowWidth = numWidth;
let betsRow = [];
let markers = [
    [],
    [],
    []
];
let zeroMarkers = [];

function fillMarkers() {
    for (let i = 0; i < 3; i++) {
        for (let j = 0; j < 12; j++) {
            let v3;
            if (i == 0) v3 = new mp.Vector3(boardPivotPoint.x - betsNum[i][j][0] + 0.77, boardPivotPoint.y - betsNum[i][j][1] - 0.062, boardPivotPoint.z + boardHeight);
            else if (i == 1) v3 = new mp.Vector3(boardPivotPoint.x - betsNum[i][j][0] + 0.77, boardPivotPoint.y - betsNum[i][j][1] - 0.03, boardPivotPoint.z + boardHeight);
            else if (i == 2) v3 = new mp.Vector3(boardPivotPoint.x - betsNum[i][j][0] + 0.77, boardPivotPoint.y - betsNum[i][j][1] - 0.01, boardPivotPoint.z + boardHeight);
            createMarker(i, j, v3.x, v3.y);
        }
    }
    let mark = mp.objects.new(mp.game.joaat('vw_prop_vw_marker_01a'), new mp.Vector3(boardPivotPoint.x - 0.137, boardPivotPoint.y - 0.147, boardPivotPoint.z + boardHeight),
        {
            rotation: new mp.Vector3(0, 0, global.boardObjects[playingGameId].rotation.z),
            alpha: 255,
            dimension: dimension
        });
    zeroMarkers.push(mark);
    mark = mp.objects.new(mp.game.joaat('vw_prop_vw_marker_01a'), new mp.Vector3(boardPivotPoint.x - 0.137, boardPivotPoint.y + 0.10, boardPivotPoint.z + boardHeight),
        {
            rotation: new mp.Vector3(0, 0, global.boardObjects[playingGameId].rotation.z),
            alpha: 255,
            dimension: dimension
        });
    zeroMarkers.push(mark);
    zeroMarkers.forEach(x => x.setCollision(false, false));
    zeroMarkers.forEach(x => x.setVisible(false, false));
}

function createMarker(i, j, x, y) {
    let mark = mp.objects.new(mp.game.joaat('vw_prop_vw_marker_02a'), new mp.Vector3(x, y, boardPivotPoint.z + boardHeight),
        {
            rotation: new mp.Vector3(0, 0, global.boardObjects[playingGameId].rotation.z),
            alpha: 255,
            dimension: dimension
        });
    mark.setCollision(true, false);
    //mark.setVisible(false, false);
    markers[i][j] = mark;
}

function calc([x, y], mX, mY) {
    return [(x + (numWidth * mX)).toFixed(3), (y - (numHeight * mY)).toFixed(3)];
}

function screenCalc([x, y], mX, mY) {
    return [(x + (numScreenWidth * mX)), (y + (numScreenHeight * mY))];
}

function fillNumberBets() {
    for (let i = 0; i < 3; i++) {
        for (let j = 0; j < 12; j++) {
            betsNum[i][j] = calc(n_3, j, i);
        }
    }
}

function fillBottomBets() {
    for (let i = 0; i < 6; i++) {
        betsBottom[i] = [n_1_18[0] + (i * bottomBetWidth), n_1_18[1]];
    }
}

function fillDozens() {
    for (let i = 0; i < 3; i++) {
        betsDozen[i] = [firstDozen[0] + (i * dozenBetWidth), firstDozen[1]];
    }
}

function fillRowBets() {
    for (let i = 0; i < 3; i++) {
        betsRow[i] = [firstRowBet[0], firstRowBet[1] - (i * rowHeight)];
    }
}

function exit() {
    if (!isAtTable) return;
    isPlacing = false;
    stopReaction();
    mp.game.cam.renderScriptCams(false, true, 10, true, false);
    global.showCursor(false)
    mp.events.callRemote('player:leftRoulette');
    global.gui.close();
    clearHighlight();
    destroyChipModels();
    global.gui.setData('casino/setData', JSON.stringify({ seconds: 0 }));
    playingGameId = -1;
    resetBets();
    deleteMarkers();
    isAtTable = false;
    mp.events.callRemote('player:requestBank');
    standUp();
}

let bets = [];
let betsChips = [];
let currentChipSet = [0, 0, 0, 0, 0];
let allChipModels = [];

function placeBet(betName) {
    //if (bets.find(b => b == etName) != undefined) return;
    let totalChips = 0;
    for (let i = 0; i < 5; i++) {
        totalChips += chipsFromServer[i];
    }
    if (totalChips == 0) return;
    for (let i = 0; i < 5; i++) {
        if (chipsFromServer[i] < currentChipSet[i]) {
            return;
        }
    }
    let placedBetIndex = bets.lastIndexOf(betName);
    let array = [
        [],
        [],
        [],
        [],
        []
    ];
    if (placedBetIndex != -1) {//если такая ставка уже стояла
        for (let i = 0; i < 5; i++) {
            for (let j = 0; j < currentChipSet[i]; j++) {
                if (j >= 8) continue;//больше 8 вверх не растет
                if (allChipModels[placedBetIndex][i][j] == undefined) {
                    let chip = mp.objects.new(mp.game.joaat(chipImplementations[i]), new mp.Vector3(chipModels[i][j].position.x, chipModels[i][j].position.y, chipModels[i][j].position.z));
                    array[i].push(chip);
                }
                else {
                    let chip = mp.objects.new(mp.game.joaat(chipImplementations[i]), new mp.Vector3(allChipModels[placedBetIndex][i][j].position.x, allChipModels[placedBetIndex][i][j].position.y, allChipModels[placedBetIndex][i][j].position.z + j * chipZoffset));
                    array[i].push(chip);
                }
            }
        }
        allChipModels.push(array);
    }

    bets.push(betName);
    betsChips.push(currentChipSet);

    if (placedBetIndex == -1)//real shit
    {
        for (let i = 0; i < 5; i++) {
            for (let j = 0; j < currentChipSet[i]; j++) {
                if (j >= 8) continue;//больше 8 вверх не растет
                let chip = mp.objects.new(mp.game.joaat(chipImplementations[i]), new mp.Vector3(chipModels[i][j].position.x, chipModels[i][j].position.y, chipModels[i][j].position.z));
                array[i].push(chip);
            }
        }
        allChipModels.push(array);
    }

    mp.events.callRemote('player:placedBet', betName, currentChipSet[0], currentChipSet[1], currentChipSet[2], currentChipSet[3], currentChipSet[4]);
    mp.events.callRemote('player:requestBank');
    //currentChipSet = [0,0,0,0,0];
    //destroyChipModels();
}

function cancelLastBet() {
    if (currentChipSet.filter(b => b > 0).length > 0) {
        destroyChipModels();
        mp.events.callRemote('player:requestBank');
        return;
    }

    if (bets.length == 0) return;
    bets.pop();
    currentChipSet = betsChips.pop();

    let cModels = allChipModels.pop();
    cModels.forEach(x => x.forEach(c => c.destroy()));

    updateChipModels();
    mp.events.callRemote('player:canceledBet');
    mp.events.callRemote('player:requestBank');
}

function resetBets() {
    currentChipSet = [0, 0, 0, 0, 0];
    bets = [];
    betsChips = [];
    allChipModels.forEach(c => c.forEach(x => x.forEach(b => b.destroy())));
    allChipModels = [];
}

function updateChipModels() {
    for (let i = 0; i < 5; i++) {
        if (currentChipSet[i] > 0) {
            for (let j = 0; j < currentChipSet[i]; j++) {
                createChipModel(i);
            }
        }
    }
}

function deleteMarkers() {
    markers.forEach(x => x.forEach(
        m => m.destroy()
    ));
    markers = [
        [],
        [],
        []
    ];
    zeroMarkers.forEach(x => x.destroy());
    zeroMarkers = [];
}

mp.events.add('roulette:exit:pressed', () => {
    exit();
})

mp.events.add('roulette:shipset:update', (bet) => {
    if (!isPlacing) return;
    switch (bet) {
        case 'black':
            currentChipSet[0]++;
            global.gui.setData("hud/subChipValue", "black")
            break;
        case 'red':
            currentChipSet[1]++;
            global.gui.setData("hud/subChipValue", "red")
            break;
        case 'blue':
            currentChipSet[2]++;
            global.gui.setData("hud/subChipValue", "blue")
            break;
        case 'green':
            currentChipSet[3]++;
            global.gui.setData("hud/subChipValue", "green")
            break;
        case 'yellow':
            currentChipSet[4]++;
            global.gui.setData("hud/subChipValue", "yellow")
            break;
    }
    updateChipModels();
});

let currentReaction;
mp.events.add('roulette:reaction:play', (reaction) => {
    currentReaction = reaction;
    mp.players.local.taskPlayAnimAdvanced("anim_casino_b@amb@casino@games@shared@player@", currentReaction, chairPoint.x, chairPoint.y, chairPoint.z, 0, 0, chairDirection, -8, 1, -1, 5642, 0, 2, 0);
    mp.events.callRemote("player:playAnim", 5642, "anim_casino_b@amb@casino@games@shared@player@", currentReaction, chairPoint.x, chairPoint.y, chairPoint.z, chairDirection);
});

function syncronizeAnim(play, animName, flag, x, y, z) {
    if (play) {
        // mp.events.callRemote("player:playAnim", flag, "anim_casino_b@amb@casino@games@shared@player@", animName, x, y, z);
    }
    else {
        // mp.events.callRemote("player:stopAnim");
    }
}

function stopReaction() {
    if (currentReaction != null || currentReaction != undefined)
        mp.players.local.stopAnim("anim_casino_b@amb@casino@games@shared@player@", currentReaction, 0.0);
    syncronizeAnim(false, currentReaction);
}

const chipYoffset = 0.02;
const chipXoffset = 0.02;
const chipZoffset = 0.01;
let chipModels = [
    [],
    [],
    [],
    [],
    []
];
let chipImplementations = ['vw_prop_chip_10kdollar_x1', 'vw_prop_chip_100dollar_x1', 'vw_prop_chip_500dollar_x1', 'vw_prop_chip_5kdollar_x1', 'vw_prop_chip_1kdollar_x1'];

function createChipModel(type) {
    if (currentChipSet[type] >= 8) return;
    let chip = mp.objects.new(mp.game.joaat(chipImplementations[type]), new mp.Vector3(boardPivotPoint.x - 0.35, boardPivotPoint.y - 0.45, boardPivotPoint.z + boardHeight + currentChipSet[type] * chipZoffset), {
        alpha: 120
    });
    chipModels[type].push(chip);
}

function setChipPosition(globalX, globalY) {
    let types = [];
    for (let i = 0; i < 5; i++) {
        if (currentChipSet[i] > 0) {
            types.push(i);
        }
    }
    switch (types.length) {
        case 0:
            return;
        case 1:
            set2dPosition(chipModels[types[0]], globalX, globalY);
            break;
        case 2:
            set2dPosition(chipModels[types[0]], globalX + chipXoffset, globalY);
            set2dPosition(chipModels[types[1]], globalX - chipXoffset, globalY);
            break;
        case 3:
            set2dPosition(chipModels[types[0]], globalX + chipXoffset, globalY);
            set2dPosition(chipModels[types[1]], globalX - chipXoffset, globalY);
            set2dPosition(chipModels[types[2]], globalX - 2 * chipXoffset, globalY);
            break;
        case 4:
            set2dPosition(chipModels[types[0]], globalX + chipXoffset, globalY - chipYoffset);
            set2dPosition(chipModels[types[1]], globalX - chipXoffset, globalY + chipYoffset);
            set2dPosition(chipModels[types[2]], globalX + chipXoffset, globalY + chipYoffset);
            set2dPosition(chipModels[types[3]], globalX - chipXoffset, globalY - chipYoffset);
            break;
        case 5:
            set2dPosition(chipModels[types[0]], globalX + chipXoffset, globalY - chipYoffset);
            set2dPosition(chipModels[types[1]], globalX - chipXoffset, globalY + chipYoffset);
            set2dPosition(chipModels[types[2]], globalX + chipXoffset, globalY + chipYoffset);
            set2dPosition(chipModels[types[3]], globalX - chipXoffset, globalY - chipYoffset);
            set2dPosition(chipModels[types[4]], globalX - 2 * chipXoffset, globalY + chipYoffset / 2);
            break;
    }
}

function set2dPosition(array, relX, relY) {
    array.forEach(a => a.position = new mp.Vector3(relX, relY, a.position.z));
}

function destroyChipModels() {
    if (chipModels.filter(c => c.length != 0).length == 0) return;
    chipModels.forEach(c => c.forEach(x => x.destroy()));
    chipModels = [
        [],
        [],
        [],
        [],
        []
    ];
    currentChipSet = [0, 0, 0, 0, 0];
}