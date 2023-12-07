let updateGameTime = true;

let enviroment = {
    updateWeather: true,
    updateTime: true, 
    time: [0,0],
    date: [29, 7, 2020],
    weather: "EXTRASUNNY"
}

function formatIntZero(num, length) { 
    
    return ("0" + num).slice(length); 
} 

mp.events.add('Enviroment_Time', (data) => {
    if (data == undefined) return;
    enviroment.time = data;
    let time = `${formatIntZero(enviroment.time[0], -2)}:${formatIntZero(enviroment.time[1], -2)}`;
    global.gui.setData('hud/updateData', JSON.stringify({name: 'time', value: time }));
    mp.gui.execute(`chat.stime = '${time}'`)
})

mp.events.add("weather:set:rain", (val)=>{

    mp.game.invoke("0x643E26EA6E024D92", val);
});

mp.events.add('Enviroment_Date', (data) => {
    if (data == undefined) return;
    enviroment.date = data;

    let date = `${formatIntZero(enviroment.date[0], -2)}.${formatIntZero(enviroment.date[1], -2)}.${enviroment.date[2]}`; 
    global.gui.setData('hud/updateData', JSON.stringify({name: 'date', value: date }));
})

mp.events.add('Enviroment_Weather', (weather) => {
    enviroment.weather = weather;
    global.gui.setData('smartphone/weatherPage/setNowWeather', JSON.stringify(enviroment.weather));
})

mp.events.add('Enviroment_Start', (timeData, dateData, weather) => {
    setTimeout(() => {
        enviroment.time = timeData;
        enviroment.date = dateData;
        let time = `${formatIntZero(enviroment.time[0], -2)}: ${formatIntZero(enviroment.time[1], -2)}`;
        global.gui.setData('hud/updateData', JSON.stringify({name: 'time', value: time }));
       
        let date = `${formatIntZero(enviroment.date[0], -2)}.${formatIntZero(enviroment.date[1], -2)}.${enviroment.date[2]}`;   
        global.gui.setData('hud/updateData', JSON.stringify({name: 'date', value: date }));
        enviroment.weather = weather;
        global.gui.setData('smartphone/weatherPage/setNowWeather', JSON.stringify(enviroment.weather));
    }, 3000);
})

let adminTimerStop = true;
mp.events.add('switchTime', (val) => {
    adminTimerStop = (val == true);
})

mp.events.add('stopTime', () => {
    enviroment.updateTime = false;
    enviroment.updateWeather = false;
    mp.game.gameplay.setWeatherTypeNow('EXTRASUNNY');
    mp.game.time.setClockTime(0, 0, 0);
})
mp.events.add('resumeTime', () => {
    enviroment.updateTime = true;
    enviroment.updateWeather = true;
    mp.game.gameplay.setWeatherTypeNow(enviroment.weather);
    mp.game.time.setClockTime(enviroment.time[0], enviroment.time[1], 0);
})

setInterval(() => {
    if (enviroment.updateWeather) mp.game.gameplay.setWeatherTypeNowPersist(enviroment.weather);
    if(!adminTimerStop && enviroment.updateTime) {
        let hour = enviroment.time[0];
        let min = enviroment.time[0];
        if(hour > 20 || hour < 5){
            hour = 21;
            min = 0;
        }
        mp.game.time.setClockTime(hour, min, 0);
    }
}, 1000);