//let global.boardObjects = [];
let maxPlayers = 6;
let loadedPoker = false;
let _currentBet = 0;
let MinBetInDollars = 0;
//player data
let pokerPlayers = {
    name: [" ", " ", " ", " ", " ", " "],
    cards: [
        [],
        [],
        [],
        [],
        [],
        []
    ],
    wincards: [
        [],
        [],
        [],
        [],
        [],
        []
    ],
    money: [0, 0, 0, 0, 0, 0],
    move: [" ", " ", " ", " ", " ", " "],
    currBet: [0, 0, 0, 0, 0, 0],
    isWinner: [false, false, false, false, false, false],
    images: [null, null, null, null, null, null],
}
//table data
let TableCards = []
let TableCardsWins = []
let Bank = []
let activePlayer = null;
//self data
let FullBank = 0
let stateMyMove = false;
let stateBuyChips = true;
let myBet = 0;
let autoCheckActive = false;
let playerBetAutoCheck = false;
let CardDisable = false;


let myPlace = -1;
let pokerCam = null;


global.updatePokerLable = (handle) => {
    global.boardObjects.forEach(board => {
        if (board.handle == handle) {
            mp.game.graphics.drawText(board.info, [board.position.x, board.position.y, board.position.z + 2.1], {
                font: 7,
                color: [255, 255, 255, 185],
                scale: [0.3, 0.3],
                outline: true
            });
        }
    });
}

mp.events.add('poker:enterPoker', (id, balance, members, place, minBet) => {
    if (!global.gui.openPage('Poker')) return;
    clearAllPlayers();
    playingGameId = id;
    myPlace = place;
    MinBetInDollars = minBet;
    let pos = global.boardObjects[id].position;
    let rot = global.boardObjects[id].rotation;
    let play = JSON.parse(members);
    for (let player of play) {
        pushOnPlayers(player)
    }
    FullBank = balance;
    global.gui.setData('poker/setCurrentPlayerChipsBalance', JSON.stringify(pokerPlayers.money[myPlace])); //меню докупки
    global.gui.setData('poker/setCurrentMoveTime', JSON.stringify(60)); //таймер хода
    global.gui.setData('poker/setMinChipsValue', JSON.stringify(MinBetInDollars * 2)); //минимальный баланс для игры


    global.gui.setData("hud/setIsChipsList", false) //прячем худ казино
    global.gui.setData('poker/resetRangeSlider');

    //камера
    pokerCam = mp.cameras.new('casino', new mp.Vector3(pos.x, pos.y, pos.z + 2), new mp.Vector3(-90, 0, rot.z), 70);
    //pokerCam.pointAtCoord(pos.x, pos.y, pos.z + 2);
    pokerCam.setActive(true);
    mp.game.cam.renderScriptCams(true, true, 1000, true, true);

    global.showCursor(true);
    isAtTable = true;
    autoCheckActive = false;
    playerBetAutoCheck = false;
    CardDisable = false;
    synchronization();
    sit();
});

mp.events.add('poker:TableCard', (_tableCards) => {
    let Cards = JSON.parse(_tableCards);
    TableCards = [];
    TableCardsWins = [];
    if (Cards != null)
        for (let i = 0; i < 5; i++) {
            if (Cards[i] != null) {
                TableCards.push(getCard(Cards[i]));
                if (Cards[i].Wins)
                    TableCardsWins.push(i);
            }
        }
    synchronization();
});

mp.events.add('poker:WinCombins', (_tableCards, listWinsThisStep, sumWins, currentBank) => {
    global.gui.setData('poker/setWinCount', JSON.stringify(0)); //сбрасываем
    let Cards = JSON.parse(_tableCards);
    let winners = JSON.parse(listWinsThisStep);
    let BankList = JSON.parse(currentBank);
    TableCardsWins = [];
    for (let i = 0; i < 5; i++) {
        if (Cards[i] != null) {
            if (Cards[i].Wins)
                TableCardsWins.push(i);
        }
    }

    pokerPlayers.wincards = [
        [],
        [],
        [],
        [],
        [],
        []
    ];
    pokerPlayers.isWinner = [false, false, false, false, false, false];
    synchronization();
    for (let player of winners) {
        pokerPlayers.wincards[player._place] = [];
        if (player.openCard[0].Wins)
            pokerPlayers.wincards[player._place].push(0);
        if (player.openCard[1].Wins)
            pokerPlayers.wincards[player._place].push(1);
        pokerPlayers.isWinner[player._place] = true;
        pokerPlayers.move[player._place] = "+" + sumWins;
        pokerPlayers.money[player._place] = player.Balance;
    }
    Bank = BankList;
    synchronization();
    setTimeout(function () {
        global.gui.setData('poker/setWinCount', JSON.stringify(sumWins));
    }, 100);
});

mp.events.add('poker:newPlayer', (player) => {
    let play = JSON.parse(player);
    pushOnPlayers(play);
    synchronization();
});

mp.events.add('poker:playersCard', (players, cards) => {
    let play = JSON.parse(players);
    let mycards = JSON.parse(cards);
    for (let player of play) {
        if (player.openCard[0] != null) {
            pokerPlayers.cards[player._place][0] = getCard(player.openCard[0]);
            pokerPlayers.cards[player._place][1] = getCard(player.openCard[1]);
        } else if (player.IsOnDealing)
            pokerPlayers.cards[player._place] = ["back", "back"];
    }
    if (mycards != null && mycards[0] != null) {
        pokerPlayers.cards[myPlace][0] = getCard(mycards[0]);
        pokerPlayers.cards[myPlace][1] = getCard(mycards[1]);
    }
    synchronization();
});

mp.events.add('poker:giveMyCards', (place, cards) => {
    let mycards = JSON.parse(cards);
    if (mycards != null && mycards[0] != null) {
        pokerPlayers.cards[place][0] = getCard(mycards[0]);
        pokerPlayers.cards[place][1] = getCard(mycards[1]);
    }
    synchronization();
});

mp.events.add('poker:playerMove', (move, place, balance, isOnDealing, currBetPlayer) => {
    pokerPlayers.move[place] = move;
    pokerPlayers.money[place] = balance;
    pokerPlayers.currBet[place] = currBetPlayer;
    _currentBet = currBetPlayer;
    if (!isOnDealing && place != myPlace)
        pokerPlayers.cards[place] = [];
    else if (!isOnDealing && place == myPlace) {
        CardDisable = true;
        stateBuyChips = true;
        makeMove(); //вызываем на случай, если сброс был по таймеру
    }
    if (move == "Wait") {
        pokerPlayers.move[place] = "client_39";
        if (place != myPlace) {
            activePlayer = (place - myPlace + maxPlayers) % maxPlayers;
            global.gui.setData('poker/setCurrentMoveTime', JSON.stringify(0)); //таймер хода
            global.gui.setData('poker/setCurrentMoveTime', JSON.stringify(60)); //таймер хода
        }
    } else
        activePlayer = null;
    activeAutoCheck();
    synchronization();
});

mp.events.add('poker:NewDealing', () => {
    pokerPlayers.move = [" ", " ", " ", " ", " ", " "];
    pokerPlayers.cards = [
        [],
        [],
        [],
        [],
        [],
        []
    ];
    pokerPlayers.wincards = [
        [],
        [],
        [],
        [],
        [],
        []
    ];
    pokerPlayers.isWinner = [false, false, false, false, false, false];
    pokerPlayers.currBet = [0, 0, 0, 0, 0, 0];
    Bank = [];
    CardDisable = false;
    global.gui.setData('poker/setAllBets', JSON.stringify(Bank)); //обнуляем банк

    TableCards = [];
    stateBuyChips = true;
    synchronization();
});

mp.events.add('poker:NewRound', () => {
    pokerPlayers.move = [" ", " ", " ", " ", " ", " "];
    pokerPlayers.currBet = [0, 0, 0, 0, 0, 0];
    _currentBet = 0;
    global.gui.setData('poker/setRatesDone', JSON.stringify(false)); //анимация фишек
    activeAutoCheck();
    synchronization();
});

mp.events.add('poker:EndRound', (bank) => {
    let allBank = JSON.parse(bank);
    Bank = allBank;
    global.gui.setData('poker/setAllBank', JSON.stringify(Bank));
    global.gui.setData('poker/setRatesDone', JSON.stringify(true)); //анимация фишек

});

mp.events.add('poker:changeBank', (place, balance, MyFullBank, currBet) => {
    pokerPlayers.money[place] = balance;
    pokerPlayers.currBet[place] = currBet;
    if (place == myPlace)
        FullBank = MyFullBank;

    synchronization();
});

mp.events.add('poker:AllFoldButPlayer', (_place, currentWinSUm) => {
    TableCardsWins = [];
    global.gui.setData('poker/setWinCount', JSON.stringify(0)); //сбрасываем
    pokerPlayers.isWinner[_place] = true;
    pokerPlayers.move[_place] = "+" + currentWinSUm;
    pokerPlayers.money[_place] = pokerPlayers.money[_place] + currentWinSUm;
    Bank = [];
    synchronization();
    setTimeout(function () {
        global.gui.setData('poker/setWinCount', JSON.stringify(currentWinSUm));
    }, 100);
});


mp.events.add('poker:changeImage', (_place, image) => {
    pokerPlayers.images[_place] = image;
    synchronization();
});



//player make bet
mp.events.add('poker:requestMakingBet', (currentBet, myCurBet) => {
    global.gui.setData('poker/setCurrentMoveTime', JSON.stringify(0)); //таймер хода
    _currentBet = currentBet;
    stateMyMove = true;
    stateBuyChips = false;
    activePlayer = 0;
    myBet = myCurBet;
    let check = _currentBet - myCurBet;
    if (check > pokerPlayers.money[myPlace])
        check = pokerPlayers.money[myPlace];
    let minValueToRaise = _currentBet * 2;
    if (minValueToRaise == 0)
        minValueToRaise = MinBetInDollars * 2;
    global.gui.setData('poker/setCurrentPlayerMoveValues', JSON.stringify({ checkValue: check, minValue: minValueToRaise, stepValue: MinBetInDollars * 2 }));


    global.gui.setData('poker/setCurrentMoveTime', JSON.stringify(60)); //таймер хода
    synchronization();
    global.gui.setData('poker/resetRangeSlider');
    global.gui.setData('poker/initRangeSlider');
    if (playerBetAutoCheck && _currentBet == myBet) {
        makeMove();
        mp.events.callRemote('player:pokerBet', 'Check', 0);
    }
});


//player make bet
mp.events.add('poker:leavePlayer', (place) => {
    clearPlayer(place);
    synchronization();

});

mp.events.add('poker:buyChips', (count) => {
    if (count > FullBank) return;
    if (!stateBuyChips) return;
    mp.events.callRemote('player:pokerBuyChips', count);
});

mp.events.add('poker:fold', () => {
    stateBuyChips = true;
    makeMove();
    mp.events.callRemote('player:pokerBet', 'Fold', 0);
});

mp.events.add('poker:check', () => {
    makeMove();
    let bet = 'Check';
    if (_currentBet > myBet)
        bet = 'Call';
    mp.events.callRemote('player:pokerBet', bet, 0);
});

mp.events.add('poker:raise', (amount) => {
    makeMove();
    mp.events.callRemote('player:pokerBet', 'Raise', amount);
});

mp.events.add('poker:allIn', () => {
    makeMove();
    mp.events.callRemote('player:pokerBet', 'AllIn', 0);
});

mp.events.add('poker:exit', () => {
    exitPoker();
});

mp.events.add('poker:autoCheck', (value) => {
    playerBetAutoCheck = value;
});



function makeMove() {
    global.gui.setData('poker/setCurrentMoveTime', JSON.stringify(0)); //таймер хода
    stateMyMove = false;
    activePlayer = null;
    pokerPlayers.move[myPlace] = " ";
    global.gui.setData('poker/resetRangeSlider');
    synchronization();
}

//отображает все локальные данные в интерфейсе
function synchronization() {
    if (myPlace > -1) {
        let k = 1;
        for (let i = myPlace + 1; i < maxPlayers; i++) {
            displayPlayer(i, k);
            k++;
        }
        for (let i = 0; i < myPlace; i++) {
            displayPlayer(i, k);
            k++;
        }
        //мои данные
        
        global.gui.setData('poker/setCurrentPlayerData', JSON.stringify({
            currentPlayer: {
                bank: FullBank, //мой банк
                chipsCount: pokerPlayers.money[myPlace], //мои фишки для игры
                cards: pokerPlayers.cards[myPlace], //мои карты
                winCombinations: pokerPlayers.wincards[myPlace], //мои выигрышные карты
                state: pokerPlayers.move[myPlace], //мое действие
                isWinner: pokerPlayers.isWinner[myPlace], //моя ставка
                cardsDisabled: CardDisable,//прозрачность карт, если сбросил
                avatar: pokerPlayers.images[myPlace], //аватарка
            },
            bankAvailable: stateBuyChips, //возможность докупки
            activePlayer: activePlayer, //игрок, который ходит
            grantAutoCheck: autoCheckActive, //autocheck

            cardsDistribution: TableCards,
            cardsDistributionCombinations: TableCardsWins,

        })); //мой банк
        global.gui.setData('poker/setCurrentPlayerMove', JSON.stringify(stateMyMove)); //активность кнопок
        global.gui.setData('poker/setCurrentPlayerCurrentRate', JSON.stringify(pokerPlayers.currBet[myPlace])); //моя ставка
        global.gui.setData('poker/setAllBank', JSON.stringify(Bank));

        // global.gui.setData('poker/setCurrentPlayerBank', JSON.stringify(FullBank)); //мой банк
        // global.gui.setData('poker/setCurrentPlayerChipsCount', JSON.stringify(pokerPlayers.money[myPlace])); //мои фишки для игры
        // global.gui.setData('poker/setCurrentPlayerCards', JSON.stringify(pokerPlayers.cards[myPlace])); //мои карты
        // global.gui.setData('poker/setCurrentPlayerWinCombinations', JSON.stringify(pokerPlayers.wincards[myPlace])); //мои выигрышные карты
        // global.gui.setData('poker/setCurrentPlayerState', JSON.stringify(pokerPlayers.move[myPlace])); //мое действие
        // global.gui.setData('poker/setbankAvailable', JSON.stringify(stateBuyChips)); //возможность докупки
        // global.gui.setData('poker/setActivePlayer', JSON.stringify(activePlayer)); //игрок, который ходит
        // global.gui.setData('poker/setCurrentPlayerIsWinner', JSON.stringify(pokerPlayers.isWinner[myPlace])); //моя ставка
        // global.gui.setData('poker/setGrantAutoCheck', JSON.stringify(autoCheckActive)); //autocheck
        // global.gui.setData('poker/setCurrentPlayerCardsDisabled', JSON.stringify(CardDisable)); //прозрачность карт, если сбросил
        //стол
        // global.gui.setData('poker/setCardsDistribution', JSON.stringify(TableCards));
        // global.gui.setData('poker/setCardsDistributionCombinations', JSON.stringify(TableCardsWins));

    }
}

function displayPlayer(place, relativePlace) {
    if (pokerPlayers.name[place] != " ") {
        global.gui.setData('poker/hideUser', JSON.stringify({ id: relativePlace, show: true }));

        global.gui.setData('poker/setOtherPlayerData', JSON.stringify({ 
            id: relativePlace,
            name: pokerPlayers.name[place],
            chipsCount: pokerPlayers.money[place],
            state: pokerPlayers.move[place],
            cards: pokerPlayers.cards[place],
            winCombinations: pokerPlayers.wincards[place],
            isWinner: pokerPlayers.isWinner[place],
            avatar: pokerPlayers.images[place],
        }));
        global.gui.setData('poker/setOtherPlayerCurrentRate', JSON.stringify({ id: relativePlace, value: pokerPlayers.currBet[place] })); //текущая ставка
        // global.gui.setData('poker/setOtherPlayerName', JSON.stringify({ id: relativePlace, name: pokerPlayers.name[place] })); //имя игрока
        // global.gui.setData('poker/setOtherPlayerChipsCount', JSON.stringify({ id: relativePlace, value: pokerPlayers.money[place] })); //деньги игрока
        // global.gui.setData('poker/setOtherPlayerState', JSON.stringify({ id: relativePlace, value: pokerPlayers.move[place] })); //действие игрока
        // global.gui.setData('poker/setUserCards', JSON.stringify({ id: relativePlace, cards: pokerPlayers.cards[place] })); //карты игрока
        // global.gui.setData('poker/setOtherPlayerWinCombinations', JSON.stringify({ id: relativePlace, combinations: pokerPlayers.wincards[place] })); //выигрышные карты игрока
        // global.gui.setData('poker/setOtherPlayerIsWinner', JSON.stringify({ id: relativePlace, value: pokerPlayers.isWinner[place] }));
    } else {
        global.gui.setData('poker/hideUser', JSON.stringify({ id: relativePlace, show: false }));
    }

}

function exitPoker() {
    if (!isAtTable) return;
    global.gui.close();
    mp.game.cam.renderScriptCams(false, true, 10, true, false);
    global.showCursor(false)
    global.gui.setData("hud/setIsChipsList", true) //прячем худ казино
    mp.events.callRemote('player:leftPoker');
    playingGameId = -1;
    isAtTable = false;
    clearAllPlayers();
    standUp();
}

function pushOnPlayers(player) {
    let place = player._place;
    clearPlayer(place);
    pokerPlayers.name[place] = player.clientName;
    if (player.openCard[0] != null) {
        pokerPlayers.cards[place][0] = getCard(player.openCard[0]);
        pokerPlayers.cards[place][1] = getCard(player.openCard[1]);
    } else if (player.IsOnDealing) {
        pokerPlayers.cards[place] = ["back", "back"];
    }
    pokerPlayers.money[place] = player.Balance
    pokerPlayers.move[place] = player._move
    pokerPlayers.currBet[place] = player.currBet;
    pokerPlayers.images[place] = player.Image;
}

function clearPlayer(place) {
    pokerPlayers.name[place] = " ";
    pokerPlayers.cards[place] = [];
    pokerPlayers.money[place] = 0;
    pokerPlayers.wincards[place] = [];
    pokerPlayers.move[place] = " ";
    pokerPlayers.currBet[place] = 0;
    pokerPlayers.isWinner[place] = false;
    pokerPlayers.images[place] = null;
}

function clearAllPlayers() {
    pokerPlayers = {
        name: [" ", " ", " ", " ", " ", " "],
        cards: [
            [],
            [],
            [],
            [],
            [],
            []
        ],
        money: [0, 0, 0, 0, 0, 0],
        wincards: [
            [],
            [],
            [],
            [],
            [],
            []
        ],
        move: [" ", " ", " ", " ", " ", " "],
        currBet: [0, 0, 0, 0, 0, 0],
        isWinner: [false, false, false, false, false, false],
        images: [null, null, null, null, null, null],
    }
    CardDisable = false;
    TableCards = []
}

function activeAutoCheck() {
    if (pokerPlayers.currBet[myPlace] == _currentBet)
        autoCheckActive = true;
    else {
        autoCheckActive = false;
        playerBetAutoCheck = false;
        global.gui.setData('poker/setAutoCheck', JSON.stringify(playerBetAutoCheck)); //обнуляем autocheck
    }
}

function getCard(Card) {
    let resultCard;
    switch (Card.Suit) {
        case 0:
            resultCard = "HH";
            break;
        case 1:
            resultCard = "DD";
            break;
        case 2:
            resultCard = "CC";
            break;
        case 3:
            resultCard = "SS";
            break;
    }
    return resultCard + Card.Value;
}


let chairPoint;
let chairDirection;
let enterAnimName;
let exitAnimName;
let chair;

function sit() {
    chair = myPlace + 1;
    chairPoint = global.boardObjects[playingGameId].getWorldPositionOfBone(global.boardObjects[playingGameId].getBoneIndexByName(`chair_base_0${chair}`));
    chairDirection;
    switch (chair) {
        case 1:
            enterAnimName = "sit_enter_left";
            chairDirection = global.boardObjects[playingGameId].rotation.z - 135;
            break;
        case 2:
            enterAnimName = "sit_enter_left";
            chairDirection = global.boardObjects[playingGameId].rotation.z + 135;
            break;
        case 3:
            enterAnimName = "sit_enter_left";
            chairDirection = global.boardObjects[playingGameId].rotation.z + 90;
            break;
        case 4:
            enterAnimName = "sit_enter_right";
            chairDirection = global.boardObjects[playingGameId].rotation.z + 90;
            break;
        case 5:
            enterAnimName = "sit_enter_right";
            chairDirection = global.boardObjects[playingGameId].rotation.z + 45;
            break;
        case 6:
            enterAnimName = "sit_enter_right";
            chairDirection = global.boardObjects[playingGameId].rotation.z - 45;
            break;
    }
    global.localplayer.taskPlayAnimAdvanced("anim_casino_b@amb@casino@games@shared@player@", enterAnimName, chairPoint.x, chairPoint.y, chairPoint.z, 0, 0, chairDirection, -8, 1, -1, 5642, 0, 2, 0);
}

function standUp() {
    switch (chair) {
        case 1:
            exitAnimName = "sit_exit_left";
            break;
        case 2:
            exitAnimName = "sit_exit_left";
            break;
        case 3:
            exitAnimName = "sit_exit_left";
            break;
        case 4:
            exitAnimName = "sit_exit_right";
            break;
        case 5:
            exitAnimName = "sit_exit_right";
            break;
        case 6:
            exitAnimName = "sit_exit_right";
            break;
    }
    chair = null;
    global.localplayer.stopAnim(enterAnimName, "anim_casino_b@amb@casino@games@shared@player@", 0.0);
    global.localplayer.taskPlayAnimAdvanced("anim_casino_b@amb@casino@games@shared@player@", exitAnimName, chairPoint.x, chairPoint.y, chairPoint.z, 0, 0, chairDirection, -8, 1, -1, 136704, 0, 2, 0);
}