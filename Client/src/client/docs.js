mp.events.add('passport', (data) => {
    global.gui.close();
    global.gui.openPage('passport');
    global.gui.setData('passport/setPassportData', JSON.stringify(data));
});

mp.events.add('passport:close', () => {
    global.gui.close();
});

mp.events.add('dochide', () => {
    global.gui.close();
});

let certificatesOpen = false;
let certificatesFracName = {
    [6]: 'lsg',
    [7]: 'lspd',
    [8]: 'lses',
    [9]: 'fib',
    [14]: 'lsa',
    [15]: 'lsn',
    [17]: 'lsc',
}

mp.events.add('certificates:show', (dataJSON) => {
    let data = JSON.parse(dataJSON);
    data.key = certificatesFracName[data.frac];
    global.gui.setData('certificate/setData', JSON.stringify(data));
    certificatesOpen = global.gui.openPage("Certificate");
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (certificatesOpen) {
        global.gui.close();
        certificatesOpen = false;
    }
});



let carPassOpened = false;

mp.events.add('veh:technicalCertificate', (data) => {
    global.gui.setData('technicalCertificate/setCarState', data);
    carPassOpened = global.gui.openPage("TechnicalCertificate");
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
    if (carPassOpened) {
        global.gui.close();
        carPassOpened = false;
    }
});

const licConfig = {
    [0]: {
        type: "vehicle",
        name: "A",
        img: 'bike',
    },
    [1]: {
        type: "vehicle",
        name: 'B',
        img: 'car',
    },
    [2]: {
        type: "vehicle",
        name: 'C',
        img: 'truck',
    },
    [3]: {
        type: "vehicle",
        name: 'D',
        img: 'ship',
    },
    [4]: {
        type: "vehicle",
        name: 'E',
        img: 'helicopter',
    },
    [5]: {
        type: "vehicle",
        name: 'F',
        img: 'flight',
    },
    [6]: {
        type: 'weapon',
        name: 'gun',
    },
    [7]: {
        type: 'medical',
        name: 'med',
    },
    [8]: {
        type: 'military',
        name: 'cl:lic:mil',
    },
    [9]: {
        type: 'job',
        name: 'cl:lic:taxi',
        img: 'taxi'
    },
    [10]: {
        type: 'job',
        name: 'cl:lic:mgmw',
        img: 'weapon'
    },
    [11]: {
        type: 'job',
        name: 'cl:lic:miner',
        img: 'iron'
    },
    [12]: {
        type: 'job',
        name: 'cl:lic:hunter',
        img: 'hunting'
    },
    [13]: {
        type: 'job',
        name: 'cl:lic:truckdriver',
        img: 'truck'
    },
    [14]: {
        type: 'job',
        name: 'cl:lic:fish',
        img: 'fishing'
    },
    [15]: {
        type: 'job',
        name: 'cl:lic:metalPlant',
        img: 'iron'
    },
}

let licOpened = false;

mp.events.add('licenses', (licensesJson, playerName, gender, birthday) => {
    try {
        let licens = JSON.parse(licensesJson);
        let data =
        {
            lic: licens.map(item => ({ type: licConfig[item.Name].type, name: licConfig[item.Name].name, img: licConfig[item.Name].img, date: item.DateReceive, dateEnd: item.DateEnd })),
            nickname: playerName.replace('_', ' '),
            gender: gender,
            bithday: birthday,
        }
        global.gui.setData('licenses/setData', JSON.stringify(data));
        licOpened = global.gui.openPage("Licenses");
    } catch (e) {
        if(global.sendException) 
            mp.serverLog(`Error in docs.licenses: ${e.name}\n${e.message}\n${e.stack}`);
    }
});

mp.events.add('Licenses:closeLicenses', () => {
    if (licOpened)
    {
        licOpened=false;
        global.gui.close();
    }
});


// mp.events.add("selectJob", (jobid) => {
//     if (global.lastCheck > Date.now()) return;
//     global.lastCheck = Date.now() + 500;
//     mp.events.callRemote("jobjoin", jobid);
// });