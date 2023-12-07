const moveSettings = {
	size: {
		x: 60,
		y: 35
	},
	position: {
		left: '85%',
		top: '78%',
	},
	showIcons: [false, false, false, false],
	values: [
		{//LEFT X
			value: 160,
			min: -1800,
			max: 1800,
			step: 5,
			invert: true,
			enabled: true,
			callback: "camMoveAngleX"
		},
		{//LEFT Y
			value: 1,
			min: -0.6,
			max: 2.6,
			step: .2,
			invert: false,
			enabled: true,
			callback: "camMoveCamZ"
		},
		{//RIGHT X
			value: 2,
			min: 1,
			max: 3,
			step: .1,
			invert: false,
			enabled: false,
			callback: ""
		},
		{//RIGHT Y
			value: 0,
			min: -1,
			max: 1,
			step: .1,
			invert: true,
			enabled: false,
			callback: "camMovePointZ"
		},
		{ //WHEELE
			value: 4,
			min: .5,
			max: 5,
			step: .5,
			invert: false,
			enabled: true,
			callback: "camSetDist"
		}
	]
}



let opened = false;

let MileageToOilChange = 1000;
let MileageToTransmissionService = 2000;
let MileageToBrakeBroken = 2000;

let pedMehanic = null;
let pedModel = -261389155;

mp.events.add('repair:openMenu', (name, mileage, mileageOilChange, mileageTransmissionService, mileageBrakePadsChange, pricePart, partsForRepair, engineOilPrice, transmissionOilPrice, brakesPrice, pedPos) => {

	let vehicle = mp.players.local.vehicle;
	if (vehicle == null || vehicle == undefined)
		return;
	opened = global.gui.openPage('VehicleRepairsServices');
	if(!opened) return;	
	
	global.gui.setData('vehicleRepairsServices/setAutoState', JSON.stringify({ name: name, Mileage: Math.round(mileage * 100) / 100 }));

	let doorBreak = vehicle.getVariable('veh:doorBreak');
	if ((typeof doorBreak) === 'undefined' || doorBreak == null)
		doorBreak = 0;
	let brokenDoor = 0;
	for (let index = 0; index < 8; index++) {
		if ((doorBreak >> index) % 2 == 1)
			brokenDoor++;
	}
	global.gui.setData('vehicleRepairsServices/setPriceRepair', JSON.stringify((brokenDoor + 1) * pricePart));


	let engineHealth = vehicle.getVariable('veh:engineHealth');
	if ((typeof engineHealth) === 'undefined' || engineHealth == null)
		engineHealth = 1000;
	let engineState = Math.round(engineHealth / 10);
	let engineRepairPrice = (100 - engineState) * partsForRepair * pricePart;

	global.gui.setData('vehicleRepairsServices/setEngineState', JSON.stringify({ StateEngine: engineState, PriceRepairEngine: engineRepairPrice }));

	let oilChangeDist = Math.round((MileageToOilChange - mileageOilChange) * 100) / 100;
	global.gui.setData('vehicleRepairsServices/setEngineOilState', JSON.stringify({ Mileage: oilChangeDist < 0 ? 0 : oilChangeDist, State: Math.round((oilChangeDist < 0 ? 0 : oilChangeDist / MileageToOilChange) * 100), Price: engineOilPrice }));

	let oilTransmChangeDist = Math.round((MileageToTransmissionService - mileageTransmissionService) * 100) / 100;
	global.gui.setData('vehicleRepairsServices/setTransmOilState', JSON.stringify({ Mileage: oilTransmChangeDist < 0 ? 0 : oilTransmChangeDist, State: Math.round((oilTransmChangeDist < 0 ? 0 : oilTransmChangeDist / MileageToTransmissionService) * 100), Price: transmissionOilPrice }));

	let brakeChangeDist = Math.round((MileageToBrakeBroken - mileageBrakePadsChange) * 100) / 100;
	global.gui.setData('vehicleRepairsServices/setBrakeState', JSON.stringify({ Mileage: brakeChangeDist < 0 ? 0 : brakeChangeDist, State: Math.round((brakeChangeDist < 0 ? 0 : brakeChangeDist / MileageToBrakeBroken) * 100), Price: brakesPrice }));

	//camera settings
	const pos = global.localplayer.position;
	global.customCamera.setPos(new mp.Vector3(pos.x, pos.y, pos.z + .6));
	global.customCamera.setPoint(new mp.Vector3(pos.x, pos.y, pos.z + .6));
	global.customCamera.moveCamZ(0);
	global.customCamera.setDist(4);
	global.customCamera.moveAngleX(160);
	global.customCamera.switchOn(0);
	global.gui.setData('mouseMove/setSettings', JSON.stringify(moveSettings));
	global.gui.setData('mouseMove/setEnebled', true);


	if (pedMehanic != null) {
		pedMehanic.destroy();
		pedMehanic = null;
	}
	pedMehanic = mp.peds.newValid(
		pedModel,
		pedPos,
		0,
		0
	);

	if(pedMehanic == null) return; 
	pedMehanic.freezePosition(false);
	let posHood = vehicle.getOffsetFromInWorldCoords(0, 2.5, 0);
	//let boneIndex = vehicle.getBoneIndexByName('bonnet');
	//let pos = vehicle.getWorldPositionOfBone(boneIndex);
	//pedMehanic.taskGoToCoordAnyMeans(pos.x, pos.y, pos.z, 2, 1, true, 1, 1);
	pedMehanic.taskSlideToCoord(posHood.x, posHood.y, posHood.z, 0, 1)
	//pedMehanic.taskOpenVehicleDoor(vehicle.handle, 7000, 4, 2);
	setTimeout(() => {
		if (pedMehanic == null || mp.players.local.vehicle == null)
			return;
		vehicle.setDoorOpen(4, false, false);
		//pedMehanic.taskLookAt(vehicle.handle, -1, 2048, 3);
		pedMehanic.taskTurnToFace(vehicle.handle, -1);
		//pedMehanic.taskAchieveHeading(180, 5000);		
	}, 4000);
});

mp.events.add('repair:buyService', (state) => {
	mp.events.callRemote('repair:buy', state);
});


mp.events.add('repair:updateInfo', (mileageOilChange, mileageTransmissionService, mileageBrakePadsChange, pricePart, partsForRepair, engineOilPrice, transmissionOilPrice, brakesPrice) => {

	let vehicle = mp.players.local.vehicle;
	if (vehicle == null || vehicle == undefined)
		return;

	let doorBreak = vehicle.getVariable('veh:doorBreak');
	if ((typeof doorBreak) === 'undefined' || doorBreak == null)
		doorBreak = 0;
	let brokenDoor = 0;
	for (let index = 0; index < 8; index++) {
		if ((doorBreak >> index) % 2 == 1)
			brokenDoor++;
	}
	global.gui.setData('vehicleRepairsServices/setPriceRepair', JSON.stringify((brokenDoor + 1) * pricePart));


	let engineHealth = vehicle.getVariable('veh:engineHealth');
	if ((typeof engineHealth) === 'undefined' || engineHealth == null)
		engineHealth = 1000;
	let engineState = Math.round(engineHealth / 10);
	let engineRepairPrice = (100 - engineState) * partsForRepair * pricePart;

	global.gui.setData('vehicleRepairsServices/setEngineState', JSON.stringify({ StateEngine: engineState, PriceRepairEngine: engineRepairPrice }));

	let oilChangeDist = Math.round((MileageToOilChange - mileageOilChange) * 100) / 100;
	global.gui.setData('vehicleRepairsServices/setEngineOilState', JSON.stringify({ Mileage: oilChangeDist < 0 ? 0 : oilChangeDist, State: Math.round((oilChangeDist < 0 ? 0 : oilChangeDist / MileageToOilChange) * 100), Price: engineOilPrice }));

	let oilTransmChangeDist = Math.round((MileageToTransmissionService - mileageTransmissionService) * 100) / 100;
	global.gui.setData('vehicleRepairsServices/setTransmOilState', JSON.stringify({ Mileage: oilTransmChangeDist < 0 ? 0 : oilTransmChangeDist, State: Math.round((oilTransmChangeDist < 0 ? 0 : oilTransmChangeDist / MileageToTransmissionService) * 100), Price: transmissionOilPrice }));

	let brakeChangeDist = Math.round((MileageToBrakeBroken - mileageBrakePadsChange) * 100) / 100;
	global.gui.setData('vehicleRepairsServices/setBrakeState', JSON.stringify({ Mileage: brakeChangeDist < 0 ? 0 : brakeChangeDist, State: Math.round((brakeChangeDist < 0 ? 0 : brakeChangeDist / MileageToBrakeBroken) * 100), Price: brakesPrice }));
});




mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
	if (opened)
		CloseMenu();
});

function CloseMenu() {
	if (!opened)
		return;
	mp.events.callRemote('repair:closemenu');
	global.gui.setData('mouseMove/setEnebled', false);
	global.gui.close();
	opened = false;

	global.customCamera.switchOff(0);

	if (pedMehanic != null) {
		pedMehanic.destroy();
		pedMehanic = null;
	}
	let vehicle = mp.players.local.vehicle;
	if (vehicle != null && vehicle != undefined)
		vehicle.setDoorShut(4, false);
}
