
let soundVolume = 1.0;
mp.events.add('playSoundForCall', (sound) => {
    PlaySound(sound);
});

function PlaySound(soundName) {
    global.gui.playSound(soundName, soundVolume, false);
}