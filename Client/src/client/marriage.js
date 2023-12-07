mp.events.add('marriage:invite', () => {
    global.gui.openPage('WeddingMenu');
   global.gui.setData('weddingMenu/setIsWeddingComplete', JSON.stringify(false));
});

mp.events.add('marriage:inputName', (name) => {
    global.gui.close();
    mp.events.callRemote('marriage:callbackInvite', name);
});

mp.events.add('marriage:cancelPropose', () => {
    global.gui.close();
});

mp.events.add('marriage:proposal', (name) => {
    global.gui.openPage('WeddingNotification');
    global.gui.setData('weddingMenu/setWeddingName', JSON.stringify(name));
});

mp.events.add('marriage:apply', () => {
    global.gui.close();
    mp.events.callRemote('marriage:callbackProposal', true);
});

mp.events.add('marriage:decline', () => {
    global.gui.close();
    mp.events.callRemote('marriage:callbackProposal', false);
});

mp.events.add('marriage:complete', (name) => {
    global.gui.openPage('WeddingMenu');
   global.gui.setData('weddingMenu/setIsWeddingComplete', JSON.stringify(true));
   global.gui.setData('weddingMenu/setCongratulationsName', JSON.stringify(name));
});