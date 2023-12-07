mp.events.add('setadminlvl', (newLvl) => {
	global.LOCAL_ADMIN_LVL = newLvl;
});

let maxScale = 0.33;
let minScale = 0.26
let playerLevelExcretion = 3;
let adminLvlHide = 7;

let sendingExcept = false;

mp.keys.bind(global.Keys.Key_F12, false, function () {
	if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1 && global.localplayer.getVariable('IS_MEDIAHELPER') !== true) return;
	// myalvl = global.localplayer.getVariable('ALVL');
	if (global.esptoggle == 3) global.esptoggle = 0;
	else global.esptoggle++;
	//if(global.esptoggle == 0) mp.game.graphics.notify('ESP: ~r~Disabled');
	//else if(global.esptoggle == 1) mp.game.graphics.notify('ESP: ~g~Only Players');
	//else if(global.esptoggle == 2) mp.game.graphics.notify('ESP: ~g~Only Vehicles');
	//else if(global.esptoggle == 3) mp.game.graphics.notify('ESP: ~g~Players & Vehicles');
});

mp.events.add('render', () => {
	if (!global.loggedin || global.getVariable(mp.players.local, 'ALVL', 0) < 1) return;
	if (global.esptoggle >= 1) {
		try {
			let position;
			let pos = mp.players.local.position;
			let distance;
			if (global.esptoggle == 1 || global.esptoggle == 3) {
				mp.players.forEachInStreamRange(player => {
					if (player.handle !== 0 && player !== mp.players.local) {
						let invis = global.getVariable(player, 'INVISIBLE', false);
						let admlvl = global.getVariable(player, 'ALVL', 0);
						if (!invis || admlvl < adminLvlHide || global.getVariable(mp.players.local, 'ALVL', false) >= admlvl)
						{
							position = player.position;
							distance = mp.game.gameplay.getDistanceBetweenCoords(pos.x, pos.y, pos.z, position.x, position.y, position.z, true);
							let correctScale = maxScale - distance / 2000;
							if (correctScale < minScale)
								correctScale = minScale;
	
							let text = '';
							let lvl = global.getVariable(player, 'lvl', 0);
							let color = getColorNameTag(player);
							let voiceColor = player.isVoiceActive ? (lvl >= playerLevelExcretion ? '~g~' : '~HUD_COLOUR_REDLIGHT~') : color;
							let hpNarmor = `[${player.getHealth()}|${player.getArmour()}]`;
							let fraction = global.getVariable(player, 'fraction', 0);
							let family = global.getVariable(player, 'familyname', '-');
							let id = global.getVariable(player, 'C_ID', `${player.remoteId}~`);
							if (global.getVariable(player, 'InDeath', false))
								text += (global.getVariable(player, 'ALVL', 0) > 0 ? '~p~' : '~r~') + `Dead `;
							text += voiceColor + `#${id} \n`;
							text += `Fr: ${fraction}, Fam: ${family}\n`;
							text += color + player.name + ` ${hpNarmor}`;
	
							mp.game.graphics.drawText(text, [position.x, position.y, position.z + 1.2], {
								scale: [correctScale, correctScale],
								outline: true,
								color: [255, 255, 255, 255],
								font: 4
							});
						}
					}
				});
			}
			if (global.esptoggle == 2 || global.esptoggle == 3) {
				mp.vehicles.forEachInStreamRange(vehicle => {
					if (vehicle.handle !== 0 /*&& vehicle !== mp.players.local*/) {
						position = vehicle.position;
						distance = mp.game.gameplay.getDistanceBetweenCoords(pos.x, pos.y, pos.z, position.x, position.y, position.z, true);
						let correctScale = maxScale - distance / 2000;
						if (correctScale < minScale)
							correctScale = minScale;
						mp.game.graphics.drawText(mp.game.vehicle.getDisplayNameFromVehicleModel(vehicle.model) + ` (${vehicle.getNumberPlateText()} | ${vehicle.remoteId})` + `\n${global.getVariable(vehicle, 'HOLDERNAME', '')}`, [position.x, position.y, position.z - 0.5], {
							scale: [correctScale, correctScale],
							outline: true,
							color: [255, 255, 255, 255],
							font: 4
						});
					}
				});
			}
		} catch (e) {
			if (global.sendException && !sendingExcept) {
				sendingExcept = true;
				mp.serverLog(`Error in adminesp.render: ${e.name}\n${e.message}\n${e.stack}`);
			} 
		}
	}
});


mp.events.add('render', () => {
	if (!global.loggedin || global.localplayer.getVariable('IS_MEDIAHELPER') !== true || global.getVariable(mp.players.local, 'ALVL', 0) > 0) return;
	if (global.esptoggle >= 1) {
		try {
			let position;
			let pos = mp.players.local.position;
			let distance;
			if (global.esptoggle == 1 || global.esptoggle == 3) {
				mp.players.forEachInStreamRange(player => {
					if (player.handle !== 0 && player !== mp.players.local) {
						let invis = global.getVariable(player, 'INVISIBLE', false);
						let admlvl = global.getVariable(player, 'ALVL', 0);
						if (!invis || admlvl < adminLvlHide)
						{
							position = player.position;
							distance = mp.game.gameplay.getDistanceBetweenCoords(pos.x, pos.y, pos.z, position.x, position.y, position.z, true);
							let correctScale = maxScale - distance / 2000;
							if (correctScale < minScale)
								correctScale = minScale;
	
							let color = getColorNameTag(player);
							let voiceColor = player.isVoiceActive ? '~HUD_COLOUR_REDLIGHT~' : color;
							let text = voiceColor + `#${player.remoteId}`;
	
							mp.game.graphics.drawText(text, [position.x, position.y, position.z + 1.2], {
								scale: [correctScale, correctScale],
								outline: true,
								color: [255, 255, 255, 255],
								font: 4
							});
						}
					}
				});
			}
		} catch (e) {
			if (global.sendException && !sendingExcept) {
				sendingExcept = true;
				mp.serverLog(`Error in adminesp.mediahelper: ${e.name}\n${e.message}\n${e.stack}`);
			} 
		}
	}
});

function getColorNameTag(player) {
	if (global.getVariable(player, 'ALVL', 0) > 0)
		return '~r~';
	if (global.getVariable(player, 'IS_MEDIA', false))
		return '~b~';
	if (global.getVariable(player, 'IS_MEDIAHELPER', false))
		return '~HUD_COLOUR_NET_PLAYER23~';
	let lvl = global.getVariable(player, 'lvl', 0);
	if (lvl < playerLevelExcretion)
		return '~HUD_COLOUR_NET_PLAYER31~';
	return '~w~';
}

