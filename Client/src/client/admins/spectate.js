
mp.events.add("spmode", (target, toggle) => {
	try {
		mp.players.local.freezePosition(toggle == true);;
		if (toggle == true) {
			if (target && mp.players.exists(target)) {
				global.spectating = true;
				mp.players.local.attachTo(target.handle, -1, -1.5, -1.5, 2, 0, 0, 0, true, false, false, false, 0, false);
			} else mp.events.callRemote("UnSpectate");
		} else {
			mp.players.local.detach(true, true);
			global.spectating = false;
		}		
	} catch (e) {
		if(global.sendException)mp.serverLog(`spmode: ${e.name }\n${e.message}\n${e.stack}`);
	}
});