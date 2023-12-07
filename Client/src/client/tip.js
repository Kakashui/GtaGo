const TIP_TIME = 6500
let waitingTips = []
let active = false
let usedTips = []
 
global.sendTip = (tipName) => {
    if (showTip(tipName))
        mp.events.callRemote('tipUsed', tipName)
}

global.sendTipNotification = (localizationStr) => {
    _showTip(localizationStr);
}

mp.events.add({
    'showTip': showTip,
    'loadTip': (tips) => {
        usedTips = JSON.stringify(tips)
    },
    'tips:showTipNotification': global.sendTipNotification
})

function showTip(text) {
    if (usedTips.includes(text)) return false
    usedTips.push(text)
    if (active) waitingTips.push(text)
    else _showTip(text)
    
    return true
}

function _showTip(text) {
    active = true
    mp.game.audio.playSoundFrontend(-1 , 'Menu_Accept', 'Phone_SoundSet_Default', true)
    global.gui.setData('hud/setTipText', JSON.stringify({text: text}))
    global.gui.setData('hud/toggleTip', true)
    setTimeout(callback, TIP_TIME)
}

function callback() {
    if (waitingTips.length === 0) {
        global.gui.setData('hud/toggleTip', false)
        active = false
    }
    else _showTip(waitingTips.pop())
}