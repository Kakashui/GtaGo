let prevPosition = new mp.Vector3(0, 0, 0);
let lastMinuteDistance = 0;
let lastMinuteCheck = Date.now();
let last15MinuteDistance = 0;
let last15MinuteCheck = Date.now();

let isAFK = false;

// setInterval(() => {
//     if (!global.loggedin)
//         return;
//     //Пройдено за секунду
//     let currPos = mp.players.local.position;
//     let distance = mp.game.system.vdist(currPos.x, currPos.y, currPos.z, prevPosition.x, prevPosition.y, prevPosition.z);
//     prevPosition = mp.players.local.position;

//     lastMinuteDistance += distance;

//     //раз в минуту обновляем пройденную дистанцию за последние 15 минут
//     if (lastMinuteCheck + 60000 < Date.now()) {
//         last15MinuteDistance += lastMinuteDistance;
//         lastMinuteDistance = 0;
//         lastMinuteCheck = Date.now();
//         //если пройденная дистанция больше 100 м. за 15 минут, то обнуляем 
//         if (last15MinuteDistance > 100) {
//             last15MinuteDistance = 0;
//             last15MinuteCheck = Date.now();
//         }
//     }
//     //если за 15 минут не ходил, то триггерим афк
//     if (checkNoMove() && !isAFK) {
//         isAFK = true;
//         mp.events.callRemote("antiafk:setAfk", isAFK);
//     }
//     else if (!checkNoMove() && isAFK) {
//         isAFK = false;
//         mp.events.callRemote("antiafk:setAfk", isAFK);
//     }
// }, 1000);

function checkNoMove() {
    return last15MinuteCheck + 900000 < Date.now() && last15MinuteDistance + lastMinuteDistance <= 100;
}