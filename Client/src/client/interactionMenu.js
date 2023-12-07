let opened = false
let currentCursorTarget
let currentInteractionType = -1

mp.events.add('intMenu:open', (menuData) => {
    global.gui.setData("interactionMenu/setItems", menuData);    
    opened = global.gui.openPage('InteractionMenu');
})

mp.keys.bind(global.Keys.Key_ESCAPE, false, closePageIfOpened)

mp.events.add('playerInteractedLocal', (target, interactionType) => {
    currentInteractionType = interactionType
    currentCursorTarget = target
    mp.events.callRemote("intMenu:opened", interactionType, target)
})

mp.events.add('intMenu:selected', (key) => {
    if (currentCursorTarget == null) return
    if (currentInteractionType === -1) return
    closePageIfOpened()
    if (!interactLocallyIfPossible(key)) 
        mp.events.callRemote("intMenu:selected", currentCursorTarget, key, currentInteractionType)
})

function closePageIfOpened() {
    if (opened) {
        opened = false
        global.gui.close()
    }
}

const dogInteractionMatrix = ["pet_0", "pet_1", "pet_2", "pet_3"]
function interactLocallyIfPossible(interactionKey) {
    if (currentInteractionType === 2) {
        global.changeControlledPetState(dogInteractionMatrix.indexOf(interactionKey))
        return true;
    }
    
    return false;
}