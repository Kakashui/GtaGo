global.IsPlayingDM = false; // default param
global.IsFreezeDM = false;
global.soundVolumeDM = 0.15;
global.HasArenaData = false;
global.IsVote = false;
//global.IsSpectating = false;

function closeMenuIfOpened(){
    if (global.HasArenaData){
        global.gui.close();
        global.HasArenaData = false;
        mp.events.callRemote('ARENA:CLOSE')
    }
}

mp.keys.bind(global.Keys.Key_ESCAPE, false, closeMenuIfOpened)

//#region Событие приходит от VUE

mp.events.add('arena:showRating', () => {
    mp.events.callRemote('arenarating');
    global.gui.close();
})

// Добавляем игрока в команду
mp.events.add('ARENA::JOIN::LOBBY::VUE', (team, lobbyId) => {
    mp.events.callRemote("ARENA::JOIN::LOBBY::CLIENT", team, lobbyId);
})

// Создаем игру
mp.events.add('ARENA::ADD::LOBBY::VUE', (mode, map, maxPlayers, weapon, rounds, bet) => {
    mp.events.callRemote("ARENA::ADD::LOBBY::CLIENT", mode, map, maxPlayers, weapon, rounds, bet);
})

// Запускаем игру
mp.events.add('ARENA::START::LOBBY::VUE', (lobbyId) => {
    mp.events.callRemote("ARENA::START::LOBBY::CLIENT", lobbyId);
})

// Сетим игрока, чтобы работали функции кика и запуска игры у создателя
mp.events.add('ARENA::SET::PLAYER::NAME::VUE', () => {
    global.gui.setData("arenaMenu/setCurrentPlayer", JSON.stringify(global.localplayer.name));
    //mp.events.call('notify', 4, 9, global.localplayer.name, 15000);
})

// Игрок отсоединяется от лобби, в котором заплатил налоги
mp.events.add('ARENA::LOBBY::LEAVE::VUE', (lobbyId) => {
    mp.events.callRemote("ARENA::LOBBY::LEAVE::CLIENT", lobbyId);
    global.gui.setData("arenaMenu/setCurrentTab", JSON.stringify('MainTab'));
    // Меняем вкладку на главную
    //global.gui.setData("arenaMenu/setCurrentTab", 'MainTab');
})

// Игрок пытается присоединиться к лобби по ID
mp.events.add('ARENA::JOIN::LOBBY::BY::ID::VUE', (lobbyId) => {
    global.gui.setData("arenaMenu/setCurrentLobbieId", lobbyId);
})

// Игрок пытается кикнуть игрока
mp.events.add('ARENA::KICK::LOBBY::VUE', (playerName, lobbyId) => {
    mp.events.callRemote("ARENA::LOBBY::KICK::CLIENT", playerName, lobbyId);
})

//#endregion

//#region Событие приходит от Сервера

// Пользователь встал на кулшейп арены и нажал E
mp.events.add('ARENA::OPEN::GUI::SERVER', (battles) => {
    // if (battles != null) {
    //     global.gui.setData("arenaMenu/setData", battles);
    // }
    global.gui.setData("arenaMenu/setCurrentTab", JSON.stringify('MainTab')); 
    global.gui.setData("arenaMenu/setData", battles);
    global.HasArenaData = global.gui.openPage("ArenaMenu");
})

// Открываем у создателя лобби, которое он создал
mp.events.add('ARENA::OPEN::LOBBY::SERVER', (lobbyId) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/setCurrentLobbieId", lobbyId);
})

mp.events.add('ARENA:DEACTIVATE::LOBBY::SERVER', (lobbyId) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/setLobbyIsStarted", JSON.stringify({ lobbyId: lobbyId, value: true }));
})


// Добавляем созданную на сервере игру в UI
mp.events.add('ARENA::ADD::LOBBY::SERVER', (lobby) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/createLobbiesItem", lobby);
    global.gui.setData("arenaMenu/setIsCreate", false);
})

// Удаляем начавшуюся на сервере игру в UI
mp.events.add('ARENA::REMOVE::LOBBY::SERVER', (roomId) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/removeLobbie", roomId);
})

// Событие приходит со стороны сервера - добавляем игрока в команду
mp.events.add('ARENA::PLAYER::SWITCH::TEAM::SERVER', (player) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/switchPlayerTeam", player);
})

// Событие приходит со стороны сервера - меняем команду игрока
mp.events.add('ARENA::JOIN::LOBBY::SERVER', (player) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/joinLobbieTeam", player);
})

// Событие приходит со стороны сервера - игрок попал в матч. Устанавливаем переменные
mp.events.add('ARENA::CHANGE::STATE::SERVER', (isPlaying) => {
    global.IsPlayingDM = isPlaying;
})

// Закрываем главное окно на клиенте (события получаем из VUE - "Закрыть главное окно")
mp.events.add('ARENA::CLOSE::GUI::SERVER', closeMenuIfOpened)

// Закрываем главное окно на клиенте (события получаем из VUE - "Закрыть главное окно")
mp.events.add('ARENA::PLAYER::FREEZE::SERVER', (isFreeze) => {
    global.IsFreezeDM = isFreeze;
})

// Событие приходит со стороны сервера - воспроизвести звук
mp.events.add('ARENA::SOUND::PLAY::SERVER', (songName) => {
    global.gui.playSound(songName, global.soundVolumeDM, false);
})

// Событие приходит со стороны сервера - меняем громкость звука
mp.events.add('ARENA::SOUND::VOLUME::SERVER', (volume) => {
    volume /= 10
    global.soundVolumeDM = volume
})

// Событие приходит со стороны сервера - удаляем игрока из лобби
mp.events.add('ARENA::LOBBY::LEAVE::SERVER', (player) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/removePlayerFromLobbie", player);
})

//#endregion

//#region Killstat HUD

global.IsKillogOppened = false;

// Начальная инициализация киллстат state
mp.events.add('ARENA::KILLOG::OPEN::SERVER', (currentDMPlayer, playersDM) => {
    global.gui.setData('hud/setKillstatCurrentUser', currentDMPlayer);
    global.gui.setData('hud/setKillstatItems', playersDM);
    global.gui.setData('hud/setKillstatType', JSON.stringify({value: ''}));
    global.gui.setData('hud/setIsKillStat', true); 
    //global.gui.setData('hud/startKillstatTimer', 600);
})

// Запуск таймера раунда
mp.events.add('ARENA::MAP::TIME::SET::SERVER', (timeLeftSeconds) => {
    global.gui.setData('hud/startKillstatTimer', timeLeftSeconds);
})

// Запуск/Завершение голосования
mp.events.add('ARENA::MAP::VOTE::SET::SERVER', (isVote) => {
    global.IsVote = isVote; // Устанавливаем переменную для голосования
    //global.gui.setData('hud/setIsKillStat', !isVote); // Закрываем/Открываем топ
    global.gui.setData('hud/startKillstatTimer', 20); // Голосование за смену карты - 20 секунд
    global.gui.setData('hud/setIsFullKillStat', isVote); // Открываем/Закрываем Общий стат
    global.gui.setData('hud/setIsVote', isVote); // Открываем/Закрываем карты для голосования
    global.IsFreezeDM = isVote; // Фризим игроков
    global.showCursor(isVote);
})

// Отключаем голосование, если игрок вышел во время голосования!
mp.events.add('ARENA::CHANGE::STATE::VOTE::SERVER', (isVote) => {
    global.IsVote = isVote;
    global.gui.setData('hud/setIsVote', isVote);
})

// Событие приходит со стороны сервера - идет смена карты
mp.events.add('ARENA::CHANGE::ROOM::ISVOTE::SERVER', (roomDTO) => {
    if (!global.HasArenaData) return;
    global.gui.setData("arenaMenu/setLobbieIsMapChange", roomDTO);
})

// Событие приходит со стороны сервера - обновляем данные, которые увидит пользователь, когда включится голосование!
mp.events.add('ARENA::CHANGE::VOTE::ITEMS::SERVER', (maps) => {
    if (!global.HasArenaData) return;
    global.gui.setData("hud/setVoteItems", maps);
})

// Пользователь выбирает карту
mp.events.add('ARENA::CHOOSE::MAP::NAME::VUE', (mapTitle) => {
    mp.events.call('ARENA::SOUND::PLAY::SERVER', 'choice2'); // Воспроизводим звук выбора
    mp.events.callRemote("ARENA::CHOOSE::MAP::NAME::CLIENT", mapTitle); // Передаем выбор на сервер
})

// Закрываем киллстат после выхода
mp.events.add('ARENA::KILLOG::CLOSE::SERVER', () => {
    global.gui.setData('hud/setIsKillStat', false); 
    global.gui.setData('hud/setIsFullKillStat', false);
    //global.gui.setData('hud/startKillstatTimer', 600);
})

// Обновить килл state
mp.events.add('ARENA::KILLOG::UPDATE::SERVER', (playersDM) => {
    global.gui.setData('hud/setKillstatItems', playersDM);
})

// Открыть, закрыть киллстат tab
mp.keys.bind(global.Keys.Key_TAB, false, function () {
    if (!global.IsPlayingDM || global.IsVote) return; 
    // Если не ДМ или идет голосование - блокируем
    if (!IsKillogOppened) {
        global.gui.setData('hud/setIsKillStat', false); 
        global.gui.setData('hud/setIsFullKillStat', true);
        global.showCursor(true);
        IsKillogOppened = true; 
    } else {
        global.gui.setData('hud/setIsKillStat', true); 
        global.gui.setData('hud/setIsFullKillStat', false);
        global.showCursor(false);
        IsKillogOppened = false;
    }
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (!global.IsPlayingDM || global.IsVote) return;
    if (IsKillogOppened) {
        global.gui.setData('hud/setIsKillStat', true);
        global.showCursor(false);
        global.gui.setData('hud/setIsFullKillStat', false);
        IsKillogOppened = false;
    }
});