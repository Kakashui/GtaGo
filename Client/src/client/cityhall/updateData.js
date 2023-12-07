mp.events.add('government:setVote', (vote) => {
    global.gui.setData('cityHallWeb/vote/setCurrentVote', JSON.stringify(vote));
});

mp.events.add('government:loadPhilanthropistsList', (data) => {
    global.gui.setData('cityHallWeb/donations/setDonationList', data);
});

mp.events.add('government:sendNotify', (title, message) => {
    global.gui.setData('cityHallWeb/openModal', JSON.stringify({ title:title, desc: message }));
});
