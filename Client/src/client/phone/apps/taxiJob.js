const { getFullAddressByPosition } = require('../zonesNames');

let pedModels =
    [
        42647445,
        793439294,
        1752208920,
        117698822,
        -1422914553,
        1074457665,
        -67533719,
        1830688247,
        -412008429,
        588969535,
        2114544056,
        1068876755,
        2120901815,
        -1280051738,
        1650288984,
        1182012905,
        -872673803,
        2010389054,
        653289389,
        348382215,
        -886023758,

    ]

let Ped = null;
let pedBlip = null;
let pedEnterVehicleTimeout = null;
let intervalDestroy = null;


mp.events.add({
    // Server events
    "phone:taxijob:sendOrder": (order) => {
        order = JSON.parse(order);
        order.Address = getFullAddressByPosition(order.Destination);
        mp.events.call('gui:setData', 'smartphone/taxiPage/taxijob_addOrder', JSON.stringify(order));
    },

    "phone:taxijob:sendOrders": (orders) => {
        orders = JSON.parse(orders);
        orders.forEach(order => {
            order.Address = getFullAddressByPosition(order.Destination);
        });

        mp.events.call('gui:setData', 'smartphone/taxiPage/taxijob_setOrders', JSON.stringify(orders));
    },

    "phone:taxijob:setCurrentOrder": (status, position, sum, isCardPayment) => {
        mp.game.ui.setNewWaypoint(position.x, position.y);

        const dto = {
            status: status,
            location: getFullAddressByPosition(position),
            sum,
            isCardPayment
        };

        mp.events.call('gui:dispatch', 'smartphone/taxiPage/taxijob_setCurrentOrder', JSON.stringify(dto));
    },

    "phone:taxijob:createPed": (position) => {
        DestroyPed();
        DestroyPedBlip();
        let pedModel = pedModels[Math.floor(Math.random() * (pedModels.length - 1))]
        Ped = mp.peds.newValid(
            pedModel,
            position,
            0,
            0
        );
        pedBlip = mp.blips.new(0, position,
            {
                name: "Checkpoint",
                scale: 1,
                color: 49,
                alpha: 255,
                drawDistance: 100,
                shortRange: false,
                rotation: 0,
                dimension: 0,
            });
    },

    "phone:taxijob:pedEnterVehicle": () => {
        if (Ped) {
            for (let i = 0; !Ped.doesExist() && i < 250; i++)
                mp.game.wait(0);
            if (Ped.doesExist()) {
                Ped.freezePosition(false);
                pedEnterVehicleTimeout = setInterval(TrySeatInVehicle, 3000);
            }
        }
    },

    "phone:taxijob:pedLeaveVehicle": (position) => {
        if (Ped && Ped.doesExist()) {
            Ped.taskLeaveAnyVehicle(0, 0);
            Ped.taskGoStraightToCoord(position.x, position.y, position.z, 3, 10000, 0, 0);
            intervalDestroy = setTimeout(() => {
                DestroyPed();
                DestroyPedBlip();

            }, 10000);
        }
    },

    "phone:taxijob:destroyPed": () => {
        DestroyPed();
        DestroyPedBlip();
        StopEnterVehicleTimeout();
    }
});



function TrySeatInVehicle() {
    if (Ped == null || !Ped.doesExist()) {
        StopEnterVehicleTimeout();
        return false;
    }
    Ped.freezePosition(false);
    if (!mp.players.local.vehicle)
        return false;
    let res = global.seatVehicleOnClearPlace(mp.players.local.vehicle, Ped, 7000, 15);
    if (res) {
        setTimeout(() => {
            mp.events.callRemote('phone::taxijob::pedEnterVehicle')
        }, 7000);
        DestroyPedBlip();
        StopEnterVehicleTimeout();
    }
    return res;
}

function StopEnterVehicleTimeout() {
    if (pedEnterVehicleTimeout != null) {
        clearInterval(pedEnterVehicleTimeout);
        pedEnterVehicleTimeout = null;
    }
}

function DestroyPed() {
    if (Ped != null) {
        if (Ped.doesExist())
            Ped.destroy();
        Ped = null;
    }
    if (intervalDestroy != null) {
        clearInterval(intervalDestroy);
        intervalDestroy = null;
    }
}

function DestroyPedBlip() {
    if (pedBlip != null) {
        pedBlip.destroy();
        pedBlip = null;
    }
}
