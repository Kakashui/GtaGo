let InstructorIsOn = true;

let Instructors =
{
	[0]: 42647445,
	[1]: 793439294,
	[2]: 1752208920,
	[3]: 117698822,
	[4]: -1422914553,
	[5]: -1422914553,
};

// 0 - Moto
// 1 - Car,
// 2 - Truck,
// 3 - Boat,
// 4 - Helicopter,
// 5 - Plane,

let pedPositions = {
	[0]: new mp.Vector3(-808.306, -1314.248, 4.88027),
	[1]: new mp.Vector3(-808.306, -1314.248, 4.88027),
	[2]: new mp.Vector3(-808.306, -1314.248, 4.88027),
	[3]: new mp.Vector3(-729.6624, -1318.266, 2.0809747),
	[4]: new mp.Vector3(-731.6271, -1438.907, 4.880671),
	[5]: new mp.Vector3(1722.593, 3286.659, 41.04894),
}

let currTypeExam = 0;

let _schoolVehicle = null;

let Instructor = null;

let _onSchool = false;

let _spawnedCones;

let listCones = [];

let totalTime = 0;

let startTime = Date.now();

let _currPoint;

let _nextPointShape;
let _nextPointMarker;
let _nextPointBlip;


let currDimension = 0;

let StartPedPosition = new mp.Vector3(0, 0, 0);
let shapeStartExam = null;
let markerStartExam = null;
let blipStartExam = null;

let enableKeys = false;

let rightAutoStart = true;

let playSoundStop = Date.now();

let oldHealth = 1000;


mp.game.ped.setPedDensityMultiplierThisFrame(0);
mp.game.streaming.setPedPopulationBudget(0);
mp.game.streaming.setVehiclePopulationBudget(0);

mp.events.add('school:setStartPosition', (position, type) => {
	let pos = JSON.parse(position);
	if (shapeStartExam != null && mp.colshapes.exists(shapeStartExam))
		shapeStartExam.destroy();
	if (markerStartExam != null && mp.markers.exists(markerStartExam))
		markerStartExam.destroy();
	if (blipStartExam != null && mp.blips.exists(blipStartExam))
		blipStartExam.destroy();
	currTypeExam = type;
	mp.game.ui.setNewWaypoint(pos.x, pos.y);
	shapeStartExam = mp.colshapes.newCircle(pos.x, pos.y, 2, 0);
	markerStartExam = mp.markers.new(27, pos, 2,
		{
			direction: new mp.Vector3(0, 0, 0),
			rotation: new mp.Vector3(0, 0, 0),
			color: [232, 228, 33, 255],
			visible: true,
			dimension: 0
		});
	blipStartExam = mp.blips.new(1, pos, {
		alpha: 255,
		color: 33,
		dimension: currDimension,
		scale: 1
	})
});

mp.events.add('school:startLearnTask', (type, dimension, veh, vehPos) => {
	let pedModel = Instructors[type];
	let vehiclePosition = JSON.parse(vehPos);
	StartLearnTask(type, pedModel, pedPositions[type], dimension, veh, vehiclePosition);
});

function GetDistance(vector1, vector2) {
	return mp.game.gameplay.getDistanceBetweenCoords(vector1.x, vector1.y, vector1.z, vector2.x, vector2.y, vector2.z, true);
}

// Создаем пропсы для трассы
function SpawnProps(allProps, dim) {
	try {
		if (!listCones)
			listCones = [];
		else {
			listCones.forEach(obj => {
				obj.objectCone.destroy();
			});
			listCones = [];
		}
		_spawnedCones = false;
		let i = 0;
		allProps.forEach(props => {
			for (i = 0; i < props.Positions.length; i++) {
				let obj = mp.objects.new(mp.game.joaat(props.Model), props.Positions[i], {
					rotation: props.Rotations[i],
					alpha: 255,
					dimension: dim
				});
				listCones.push({ objectCone: obj, pos: props.Positions[i], rotation: obj.getRotation(1), damage: false, freeze: props.FreezeProp });
			}
		});
	} catch (e) {
		if (global.sendException) mp.serverLog(`Error in school.SpawnProps: ${e.name}\n${e.message}\n${e.stack}`);
	}
}

mp.events.add('render', () => {
	if (!_onSchool)
		return;
	if (mp.players.local.vehicle && mp.players.local.vehicle.getPedInSeat(-1) == mp.players.local.handle)
		mp.game.controls.disableControlAction(27, 75, true);
	if (enableKeys || !rightAutoStart) {
		mp.game.controls.disableControlAction(27, 71, true);
		mp.game.controls.disableControlAction(27, 72, true);
	}
	if (_spawnedCones) {
		let i = 0;
		while (i < listCones.length) {
			if (!listCones[i].freeze) {
				let distSubRotation = GetDistance(listCones[i].objectCone.getRotation(1), listCones[i].rotation);
				listCones[i].rotation = listCones[i].objectCone.getRotation(1);
				if (distSubRotation > 0.2 && GetDistance(listCones[i].pos, global.localplayer.position) < 2) {
					if (!listCones[i].damage)
						DamageCone();
					listCones[i].damage = true;
				}
			}
			i++;
		}
	}
	let vehicle = mp.players.local.vehicle;
	if (vehicle != null) {
		if (vehicle.getHealth() < oldHealth)
			GetDamage();
		oldHealth = vehicle.getHealth();
	}
});


// Начало выполнения экзамена
async function StartLearnTask(type, pedModel, pedPos, dimension, veh, vehiclePosition) {
	currDimension = dimension;
	currTypeExam = type;
	StartPedPosition = pedPos;
	_schoolVehicle = veh;
	if (InstructorIsOn)
	{
		if (Instructor != null)
			Instructor.destroy();
		Instructor = mp.peds.newValid(
			pedModel,
			pedPos,
			0,
			currDimension
		);
	}
	blipStartExam = mp.blips.new(1, vehiclePosition, {
		alpha: 255,
		color: 33,
		dimension: currDimension,
		scale: 1
	})
}

mp.events.add('school:finishPractic', (result) => {
	EndOfExam(result)
});


//Конец экзамена
function EndOfExam(completeMiss) {
	_onSchool = false;
	totalTime = (Date.now() - startTime) / 1000;
	let points = GetPointsForExam(_schoolVehicle.getHealth(), listCones.filter(item => item.damage).length);
	SetStreetTraffic(false);
	if (completeMiss) {
		mp.players.local.taskLeaveAnyVehicle(0, 0);
		if(Instructor !== undefined && Instructor.doesExist()){
			Instructor.taskLeaveAnyVehicle(0, 0);
			Instructor.taskGoStraightToCoord(StartPedPosition.x, StartPedPosition.y, StartPedPosition.z, 5, 10000, 0, 0);
		}
	}
	global.goodScreenFadeOut(500, 5000, 500);
	LeaveMission(points, totalTime, GetResultExam(completeMiss, points));

	
	var pos = mp.players.local.position;
	
	
	setTimeout(() => {
        mp.events.call('teleport:newPos', new mp.Vector3(448.7896, 5571.822, 794.8277));
		mp.players.local.freezePosition(true);
		setTimeout(() => {
			mp.players.local.freezePosition(false);
			mp.events.call('teleport:newPos', pos);
		}, 4500);
	}, 500);
}

//Создание очередного чекпойнта
function CreateCheckpoint(number) {
	DestroyCheckpoint(); //удаляем старый чекпойнт
	let points = global.schoolCheckpoints[currTypeExam] //получаем список чекпойнтов текущего вида экзамена
	if (points.Positions.length <= number)
		return;
	_nextPointShape = mp.colshapes.newSphere(points.Positions[number].x, points.Positions[number].y, points.Positions[number].z, points.Radius, currDimension);
	mp.game.ui.setNewWaypoint(points.Positions[number].x, points.Positions[number].y);
	_nextPointMarker = mp.markers.new(points.Model, points.Positions[number], points.Radius,
		{
			direction: new mp.Vector3(0, 0, 0),
			rotation: points.Rotation,
			color: points.MarkerColor,
			visible: true,
			dimension: currDimension
		});

	_nextPointBlip = mp.blips.new(1, points.Positions[number], {
		alpha: 255,
		color: 33,
		dimension: currDimension,
		scale: 1
	})

	if (points.TrafficStatus[number] !== undefined) {
		SetStreetTraffic(points.TrafficStatus[number]);
	}
}

function DestroyCheckpoint() {
	if (_nextPointShape != null)
		_nextPointShape.destroy();
	_nextPointShape = null;
	if (_nextPointMarker != null)
		_nextPointMarker.destroy();
	_nextPointMarker = null;
	if (_nextPointBlip != null)
		_nextPointBlip.destroy();
	_nextPointBlip = null;

}

mp.events.add("playerEnterColshape", playerEnterColshapeHandler);

function playerEnterColshapeHandler(shape) {
	try {
		if (shape == shapeStartExam) {
			if (shapeStartExam != null && mp.colshapes.exists(shapeStartExam))
				shapeStartExam.destroy();
			if (markerStartExam != null && mp.markers.exists(markerStartExam))
				markerStartExam.destroy();
			if (blipStartExam != null && mp.blips.exists(blipStartExam))
				blipStartExam.destroy();
			mp.events.callRemote('school:createSchoolVehicle', currTypeExam);
		}
		if (!_onSchool)
			return;
		if (_nextPointShape == shape) {
			CheckSoundInCheckpoint(_currPoint);
			if (global.schoolCheckpoints[currTypeExam].Positions.length == _currPoint + 1) {
				_currPoint++;
				DestroyCheckpoint();
				EndOfExam(true);
			}
			else {
				if (global.schoolCheckpoints[currTypeExam].UnfreezeIndex.findIndex(item => item == _currPoint) > -1) {
					listCones.forEach(obj => {
						if (!obj.freeze) {
							_spawnedCones = false;
							obj.objectCone.freezePosition(false);
							obj.objectCone.setDynamic(true);
						}
					});
					setTimeout(() => {
						_spawnedCones = true;
					}, 2000);
				}
				_currPoint++;
				CreateCheckpoint(_currPoint);
			}
		}
	} catch (e) {
		if (global.sendException) mp.serverLog(`Error in school.playerEnterColshapeHandler: ${e.name}\n${e.message}\n${e.stack}`);
	}
}

function SetStreetTraffic(trafficOn) {
	if (trafficOn) {
		if(!mp.storage.data.mainSettings.trafficOff) return;
		mp.game.ped.setPedDensityMultiplierThisFrame(3);
		mp.game.streaming.setPedPopulationBudget(3);
		mp.game.streaming.setVehiclePopulationBudget(3);
	}
	else {
		mp.game.ped.setPedDensityMultiplierThisFrame(0);
		mp.game.streaming.setPedPopulationBudget(0);
		mp.game.streaming.setVehiclePopulationBudget(0);
	}
}

function GetPointsForExam(health, destroyProps) {
	return (health - destroyProps * 50) / 10;
}

function GetResultExam(finaly, point) {
	if (finaly && point > 80)
		return true;
	return false
}

function LeaveMission(points, totalTime, completeMiss) {
	_schoolVehicle = null;
	if (Instructor) {
		Instructor.destroy();
		Instructor = null;
	}

	let dto = { show: true, items: [] }
	global.gui.setData('hud/setPromptData', JSON.stringify(dto));
	let result = {
		time: `${Math.floor(totalTime / 60)}:${Math.floor(totalTime % 60)}`,
		quality: points,
		result: completeMiss
	}
	mp.events.callRemote("cancelmiss", currTypeExam, JSON.stringify(result), completeMiss);
}

mp.events.add("playerEnterVehicle", (entity, seat) => {
	try {
		if (seat == -1) {
			if (_schoolVehicle == null)
				return;
			if (_schoolVehicle == entity) {
				if (Instructor)
				{
					Instructor.freezePosition(false);
					Instructor.taskEnterVehicle(_schoolVehicle.handle, 4000, 0, 7, 0, 0);
				}
				if (!_onSchool) {
					_onSchool = true;
					if (blipStartExam != null && mp.blips.exists(blipStartExam))
						blipStartExam.destroy();
					if (global.schoolCones[currTypeExam])
						SpawnProps(global.schoolCones[currTypeExam], currDimension);
					_currPoint = 0;
					rightAutoStart = true;
					enableKeys = true;
					setTimeout(() => {
						InitExam();
					}, 4100);
				}
			}
		}
	} catch (e) {
		if (global.sendException) mp.serverLog(`Error in school.playerEnterVehicle: ${e.name}\n${e.message}\n${e.stack}`);
	}
});

global.RefreshHints = () => {
	try {
		if (!_onSchool)
			return;
		if (_schoolVehicle == null)
			return;
		let dto = { show: true, items: [] }
		enableKeys = false;
		let engineState = global.getVariable(_schoolVehicle, 'veh:engineStatus', false);
		if (mp.players.local.getConfigFlag(32, true)) {
			dto.items.push({ key: 'J', text: 'AutoSchool_2' });
			enableKeys = true;
		}
		if (!engineState) {
			dto.items.push({ key: '2', text: 'AutoSchool_1'});
			if (enableKeys) //когда ремень не пристегнуть и авто не заведено
				rightAutoStart = true;
			enableKeys = true;
		}
		else if (enableKeys && currTypeExam == 1) //когда ремень не пристегнуть, но авто заведено
		{
			rightAutoStart = false;
		}
		dto.show = enableKeys;
		setTimeout(() => {
			if (mp.players.local.vehicle)
			{
				mp.events.callRemote('VehStream_RadioChange', mp.players.local.vehicle, 255);
				mp.game.audio.setRadioToStationName("OFF");
			}
		}, 300);
		global.gui.setData('hud/setPromptData', JSON.stringify(dto));
		if (Date.now() < startTime)
		{
			mp.players.local.setConfigFlag(32, false);
			enableKeys = true;
			return;
		}
		if (_currPoint == 0) {
			switch (currTypeExam) {
				case 0:
					if (!enableKeys)
						PlaySound('motoGo');
					break;
				case 1:
					if (!rightAutoStart)
						PlaySound('autoBelt');
					else if (!enableKeys)
						PlaySound('autoStart');
					break;
				case 4:
					if (!enableKeys)
						PlaySound('planeScary');
					break;
				case 5:
					if (!enableKeys) {
						PlaySound('planeScary');
						setTimeout(() => {
							PlaySound('planeChassis');
						}, 3000);
					}
					break;
			}
		}
	}
	catch (e) {
		if (global.sendException) mp.serverLog(`Error in school.RefreshHints: ${e.name}\n${e.message}\n${e.stack}`);
	}
};

let soundVolume = 0.15;

function InitExam() {
	let sound = '';
	let timeOut = 0;
	switch (currTypeExam) {
		case 0:
			sound = 'motoWelcome';
			break;
		case 1:
			sound = 'autoWelcome';
			break;
		case 2:
			sound = 'truckWelcome';
			break;
		case 3:
			sound = 'boatWelcome';
			break;
		case 4:
			sound = 'heliWelcome';
			break;
		case 5:
			sound = 'planeWelcome';
			break;
	}
	if (soundLength[sound] !== undefined)
		timeOut = soundLength[sound];
	PlaySound(sound);
	if (currTypeExam == 5) {
		timeOut = 15000;
		setTimeout(() => {
			PlaySound('planeStart');
			setTimeout(() => {
				PlaySound('planeStartSecond');
			}, 5000);
		}, 5000);
	}
	RefreshHints();
	startTime = Date.now() + timeOut;
	setTimeout(() => {
		CreateCheckpoint(_currPoint);
	}, timeOut);
}

function GetDamage() {
	switch (currTypeExam) {
		case 0:
			PlaySound('motoLookOut');
			break;
		case 1:
			PlaySound('autoLookOut');
			break;
		case 4:
			PlaySound('planeOneLife');
			break;
		case 5:
			PlaySound('planeOneLife');
			break;
	}
}

function DamageCone() {
	switch (currTypeExam) {
		case 0:
			PlaySound('autoCone');
			break;
		case 1:
			PlaySound('autoCone');
			break;
	}
}

function CheckSoundInCheckpoint(checkpoint) {
	switch (currTypeExam) {
		case 0:
			switch (checkpoint) {
				case 1:
					PlaySound('motoSmellGood')
					break;
				case 2:
					PlaySound('motoMuscular')
					break;
				case 4:
					PlaySound('autoArea')
					break;
				case 26:
					PlaySound('motoThisMyNumberCallMeMaybe')
					break;
			}
			break;
		case 1:
			switch (checkpoint) {
				case 4:
					PlaySound('autoArea')
					break;
				case 21:
					PlaySound('autoGoSchool')
					break;
			}
			break;
		case 2:
			switch (checkpoint) {
				case 15:
					PlaySound('examOver');
					setTimeout(() => {
						PlaySound('resultSurprise');
					}, 2000);
					break;
			}
			break;
		case 3:
			switch (checkpoint) {
				case 147:
					PlaySound('boatOver')
					break;
			}
			break;
		case 4:
			switch (checkpoint) {
				case 51:
					PlaySound('examOver');
					setTimeout(() => {
						PlaySound('goToFlyingschool');
					}, 2000);
					setTimeout(() => {
						PlaySound('resultSurprise');
					}, 2000);
					break;
			}
			break;
		case 5:
			switch (checkpoint) {
				case 88:
					PlaySound('examOver');
					setTimeout(() => {
						PlaySound('goToFlyingschool');
					}, 2000);
					setTimeout(() => {
						PlaySound('resultSurprise');
					}, 2000);
					break;
			}
			break;
	}
}

function PlaySound(soundName) {
	if (playSoundStop > Date.now())
		return;
	global.gui.playSoundLang(soundName, mp.storage.data.language, soundVolume, false);
	if (soundLength[soundName] !== undefined)
		playSoundStop = Date.now() + soundLength[soundName];
}

let soundLength = {
	['autoWelcome']: 18000,
	['autoBelt']: 6000,
	['autoStart']: 2000,
	['autoLookOut']: 6000,
	['autoArea']: 2000,
	['autoCone']: 3000,
	['autoGoSchool']: 4000,
	['motoWelcome']: 7000,
	['motoGo']: 1000,
	['motoLookOut']: 9000,
	['motoSmellGood']: 4000,
	['motoMuscular']: 2000,
	['motoThisMyNumberCallMeMaybe']: 7000,
	['planeWelcome']: 5000,
	['planeStart']: 5000,
	['planeStartSecond']: 5000,
	['planeScary']: 3000,
	['planeChassis']: 3000,
	['planeOneLife']: 4000,
	['heliWelcome']: 5000,
	['goToFlyingschool']: 2000,
	['resultSurprise']: 3000,
	['examOver']: 2000,
	['boatWelcome']: 6000,
	['boatOver']: 6000,
	['truckWelcome']: 6000,
}


//Теоретическая часть
let currType = 0;
let answers = [];
let theoryRes = null;
let practiceRes = null;
let opened = false;

let schoolNames = {
	[0]: { name: 'AutoSchool_26', welcome: 'AutoSchool_32' },
	[1]: { name: 'AutoSchool_27', welcome: 'AutoSchool_33' },
	[2]: { name: 'AutoSchool_28', welcome: 'AutoSchool_34' },
	[3]: { name: 'AutoSchool_29', welcome: 'AutoSchool_35' },
	[4]: { name: 'AutoSchool_30', welcome: 'AutoSchool_36' },
	[5]: { name: 'AutoSchool_31', welcome: 'AutoSchool_37' },
}

let questData = {
	[0]: { number: 1, count: 10 },
	[1]: { number: 1, count: 10 },
	[2]: { number: 1, count: 10 },
	[3]: { number: 2, count: 5 },
	[4]: { number: 3, count: 7 },
	[5]: { number: 3, count: 7 },
}

mp.events.add('school:openMenu', (theoryResults, practiceResults, typeSchool) => {
	global.gui.setData('autoSchool/setAnswersOnQuestions', theoryResults);
	global.gui.setData('autoSchool/setPracticResults', practiceResults);
	theoryRes = JSON.parse(theoryResults);
	OpenSchoolMenu(typeSchool);
});

function OpenSchoolMenu(typeSchool) {
	opened = global.gui.openPage('AutoSchool');
	currType = typeSchool;
	global.gui.setData('autoSchool/setCurrentSection', JSON.stringify({ section: 'GeneralSection', data: null }));
	global.gui.setData('autoSchool/setCurrentSchoolName', JSON.stringify(schoolNames[typeSchool].name));
	global.gui.setData('autoSchool/setCurrentSchoolWelcome', JSON.stringify(schoolNames[typeSchool].welcome));
}

mp.events.add('school:startExam', (typeExam) => {
	if (typeExam == 'theory') {
		mp.events.callRemote('school:startTheoryExam', currType);
		CloseMenu();
	}
	else if (typeExam == 'practic') {
		mp.events.callRemote('school:startPracticExam', currType);
		CloseMenu();
	}
});

mp.events.add('school:openTheoryMenu', (typeExam) => {
	OpenSchoolMenu(typeExam);
	StartTest(typeExam);
});

mp.events.add('school:sendAnswer', (resultAnswers) => {
	theoryRes = JSON.parse(resultAnswers);
	ViewResultTheory(true);
});

function ViewResultTheory(saveRes) {
	let questions = [];
	let totalQuest = 0;
	let correctQuest = 0;
	theoryRes.forEach(item => {
		let currQuest = {
			quest: GetElementForKey(global.schoolQuestion[questData[currType].number], item.keyQuest).quest,
			myAnswer: GetElementForKey(global.schoolQuestion[questData[currType].number][item.keyQuest].answers, item.keyAnswer).title,
			correctAnswer: GetElementForKey(global.schoolQuestion[questData[currType].number][item.keyQuest].answers, global.schoolQuestion[questData[currType].number][item.keyQuest].correctAnswer).title
		};
		questions.push(currQuest);
		totalQuest++;
		if (currQuest.myAnswer == currQuest.correctAnswer)
			correctQuest++;
	});
	let result = { totalQuest, correctQuest, result: getResultTheory(totalQuest, correctQuest), questions };
	if (saveRes)
		mp.events.callRemote('school:saveTheoryResult', currType, JSON.stringify(result), result.result);
	global.gui.setData('autoSchool/setAnswersOnQuestions', JSON.stringify(result));
}

function GetElementForKey(answers, key) {
	return answers.find(item => item.key == key);
}

function getResultTheory(totalQuest, correctQuest) {
	if ((correctQuest / totalQuest) >= 0.8)
		return true;
	else
		return false;
}

function StartTest(typeQuest) {
	let questions = GetQuestions(typeQuest);
	questions.forEach(item => {
		item.answers = GetRandomElements(item.answers, item.answers.length);
	});
	global.gui.setData('autoSchool/setQuestions', JSON.stringify(questions));
	global.gui.setData('autoSchool/setCurrentSection', JSON.stringify({ section: 'QuestionSection', data: 'theory' }));
}

function GetQuestions(typeQuest) {
	return GetRandomElements(global.schoolQuestion[questData[typeQuest].number], questData[typeQuest].count);
}

function GetRandomElements(array, count) {
	let initialArray = array.map(item => item);
	let outArray = [];
	for (let i = 0; i < count; i++) {
		let index = Math.floor(Math.random() * initialArray.length);
		outArray.push(initialArray[index]);
		initialArray.splice(index, 1);
	}
	return outArray;
}

mp.events.add('school:closeMenu', () => {
	if (opened)
		CloseMenu();
});

mp.keys.bind(global.Keys.Key_ESCAPE, false, function () {
	if (opened)
		CloseMenu();
});

function CloseMenu() {
	global.gui.close();
	opened = false;
}

mp.events.add("ClientTrafficChangeStatus", SetStreetTraffic);