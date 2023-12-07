let soundLength = {
    ['welcomeServer']: 36000,
    ['callGiveVehicle']: 22000,
    ['meetingFriendTrevor']: 21000,
    ['callStealCar']: 14000,
    ['callStealSecondCar']: 12000,
    ['police1FirstSay']: 4000,
    ['police2FirstSay']: 5000,
    ['police1SecondSay']: 10000,
    ['judgeAdjudicates']: 11000,
    ['courtSounds']: 18000,
}
let playSoundStop = Date.now();
let soundVolume = 1.0;


global.PlaySoundQuest = (soundName) => {
    if (playSoundStop > Date.now())
        return;
    global.gui.playSoundLang(soundName, mp.storage.data.language, soundVolume, false);
    if (soundLength[soundName] !== undefined)
        playSoundStop = Date.now() + soundLength[soundName];
}