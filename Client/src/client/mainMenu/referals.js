function setReferalsData(total, code) {
    global.gui.setData("optionsMenu/updateReferalsData", JSON.stringify({key: "total", value: total}))
    global.gui.setData("optionsMenu/updateReferalsData", JSON.stringify({key: "code", value: code}))
}
function updateReferalsData(total) {    
    global.gui.setData("optionsMenu/updateReferalsData", JSON.stringify({key: "total", value: total}))
}

mp.events.add("mmenu:referals:set", setReferalsData)
mp.events.add("mmenu:referals:update", updateReferalsData)