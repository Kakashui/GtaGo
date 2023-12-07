mp.events.add({
    "phone:calls:incomeCall": (fromNumber) => {
        if (!global.isPhoneOpened && !gui.isOpened()) {
            mp.phone.open(false);
        }

        global.gui.setData('smartphone/incomeCall', fromNumber);
    },

    "phone:calls:setFakeCall": (callerName) => {
        if (!global.isPhoneOpened && !gui.isOpened()) {
            mp.phone.open(false);
        }

        global.gui.setData('smartphone/setFakeCall', JSON.stringify({ callerName }));
    },

    "phone:calls:endFakeCall": () => {
        global.gui.setData('smartphone/endFakeCall');

        setTimeout(() => {
            mp.phone.close();
        }, 1500);
    }
})

mp.events.addPhone({
    "phone::calls::phoneUp": () => {
        mp.phone.setSpeaking();
        // mp.players.local.taskUseMobilePhone(1);
    },

    "phone::calls::phoneDown": () => {
        mp.phone.setTexting();
        // mp.players.local.taskUseMobilePhone(0);
    },
});

// 0.07 0 -0.02 90 90 110

// 0.07 0.035 0 110 -20 0

// amb@code_human_wander_mobile@male@base static
// amb@code_human_wander_texting@male@base static -- move_characters@sandy@texting sandy_text_loop_base