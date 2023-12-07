
mp.events.add('voteMenu:sendVote', (vote) => {
    mp.events.callRemote('government:sendVote', vote);
});

mp.events.add('cityHallWeb:sendFormComplaintPage', (fraction, name, text) => {
    mp.events.callRemote('government:createComplaint', fraction, name, text);
});

mp.events.add('cityHallWeb:sendFormBillPage', (currentName, currentUrl) => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
});

mp.events.add('cityHallWeb:sendFormDebatePage', () => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
});

mp.events.add('cityHallWeb:sendMessageToCityhall', () => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
});

mp.events.add('cityHallWeb:sendFormNotaryPage', (currentService, currentProxy, currentNumber, currentMail) => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
});

mp.events.add('cityHallWeb:sendFormRecordingPage', (employee, text) => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
    return;//TODO
    mp.events.callRemote('government:createRecord', employee, text);
});

mp.events.add('cityHallWeb:sendFormNameEditPage', (name, payType) => {
    mp.events.callRemote('government:changeName', name, payType);
});

mp.events.add('cityHallWeb:sendFormRelationshipsReg', (name, payType) => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
    return;
    mp.events.callRemote('government:', name, payType);
});

mp.events.add('cityHallWeb:sendFormRelationshipsDivorce', (partner, dispute) => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
    return;
});

mp.events.add('cityHallWeb:sendFormCertificatePage', (currentSertificate, currentReason) => {
    sendNotify("cl:cityhall:web:1", "cl:cityhall:web:6");
});

mp.events.add('cityHallWeb:sendFormAppointmentPage', () => {
    mp.game.ui.setNewWaypoint(354.0953, -599.1022);
    sendNotify("cl:cityhall:web:3", "cl:cityhall:web:4");
});

mp.events.add('cityHallWeb:sendFormTaxPage', (foundersList, currentCommunity, currentName, currentNation, currentReason, currentPayment) => {
    mp.events.callRemote('government:createFamily', currentName,  currentPayment, foundersList, currentNation);
});

mp.events.add('cityHallWeb:sendFormLetterPage', (letter) => {
    sendNotify("cl:cityhall:web:5", "cl:cityhall:web:6");
});

mp.events.add('cityHallWeb:sendPayDonat', (amount, type) => {
    mp.events.callRemote('government:donateToGov', amount, type, true);
});

mp.events.add('cityHallWeb:sendFormLicensesPage', (currentLicense, currentDesc, currentPayment) => {
    //mp.serverLog(`${currentLicense}, ${currentDesc}, ${currentPayment}`)
    //sendNotify("cl:cityhall:web:1", "cl:cityhall:web:2");
    mp.events.callRemote('government:buyLicense', currentLicense, currentPayment);
});


function sendNotify(title, message) {
    global.gui.setData('cityHallWeb/openModal', JSON.stringify({ title:title, desc: message }));
};

