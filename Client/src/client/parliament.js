let currSittings;
let myUUID = 0;
let opened = false;

mp.events.add('parliament:openSitting', (data, uuid) => {
    myUUID=uuid;
    currSittings = JSON.parse(data);
    let sittings = currSittings.map(item => getSitting(item));

    opened = global.gui.openPage('ParliamentPortal');
    if(!opened) return;
    global.gui.setData('parliamentPortal/setMeetings', JSON.stringify(sittings));
    
});


mp.events.add('parliamentPortal:selectVote', (id, vote) => {
    mp.events.callRemote('parliament:setVote', id, vote);
});

mp.events.add('parliament:updateSitting', (jsonData, id) => {
    let data = JSON.parse(jsonData);
    let index = currSittings.findIndex(item => item.ID == id);
    if (index > -1)
        currSittings[index] = data;
    else
        currSittings.push(data);
    global.gui.setData('parliamentPortal/setOneMeeting', JSON.stringify(getSitting(data)));
    setCurrentVoteData(id);
});

mp.events.add('parliamentPortal:selectMeeting', (id) => {
    setCurrentVoteData(id);
});

function setCurrentVoteData(id) {
    let index = currSittings.findIndex(item => item.ID == id);
    if (index > -1)
    {
        var voteIndex = currSittings[index].ChoicesWithName.findIndex(item => item.UUID == myUUID);
        var currVote = null;
        if (voteIndex > -1)
            currVote = currSittings[index].ChoicesWithName[voteIndex].Vote;
        var data = {currentVote: currVote, partiesList: GetCurrentVoteData(currSittings[index])} ;
        global.gui.setData('parliamentPortal/setCurrentVoteData', JSON.stringify(data));
    }
    
}


function getSitting(value) {
    let completed = value.IsArchive ? 1 : 0;
    if (completed == 1)
    {
        let count = 0;
        let countPositive = 0;

        value.ChoicesWithName.forEach(element => {
            count++;
            if (element.Vote == 2)
                countPositive++;
        });
        if (countPositive < count/2 || count == 0)
            completed = 2;
    }
    return {id: value.ID, isCompleted: completed, date:value.date.replace('T', ' '), spicker: value.SpeakerName, topic: value.Topic, desc: value.Description};
}

function GetCurrentVoteData(value) {
    let parties = value.ChoicesWithName.map(item => ({ nickname: item.Name, id: item.UUID, vote: item.Vote}))
    if (parties.length > 0)
        return [{id:0, name:'cl:parl:result', members: parties}];
    return [];
}


//Close menus
mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (opened)
        closeMenu();
});


function closeMenu() {
    global.gui.close();
    opened = false;
}