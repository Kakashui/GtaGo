const { pointsConfig, plantsConfig, pitsConfig, fertilizersConfig, productConfigs } = require('./config.js');

let garbage = [];

class Plant {
    constructor(plantData) {
        this.FarmId = plantData.FarmId
        this.PointId = plantData.PointId
        this.UUID = plantData.UUID
        this.PlantName = plantData.PlantName
        this.PlantingTime = plantData.PlantingTime
        this.WateringTime = plantData.WateringTime
        this.Fertilizer = plantData.Fertilizer
		this.BuffedByPet = plantData.BuffedByPet
        this.obj = null;
        this.objModel = null;
        this.Position = null;
        this.UpdateObject();
    }

    UpdateData(plantData) {
        this.FarmId = plantData.FarmId
        this.PointId = plantData.PointId
        this.UUID = plantData.UUID
        this.PlantName = plantData.PlantName
        this.PlantingTime = plantData.PlantingTime
        this.WateringTime = plantData.WateringTime
        this.Fertilizer = plantData.Fertilizer
		this.BuffedByPet = plantData.BuffedByPet
        this.UpdateObject();
    }

    DeleteObject() {
        if (this.objModel != null || this.obj != null) {
            if (this.obj.doesExist())
                this.obj.destroy();
            else
                garbage.push(this.obj);
            this.objModel = null;
            this.obj = null;
        }
    }

    DeletePlant() {
        this.DeleteObject()
        this.PlantName = -1;
    }

    UpdateObject() {
        let plant = this.GetCurrentStatus();
        if (this.obj != null)
        {
            this.obj.position = this.Position;
            this.obj.setRotation(0, 0, 0, 0, true);
        }
        //уничтожаем старый объект, если есть необходимость
        if (this.objModel != null && plant != null && this.objModel == plant.model)
            return;
        this.DeleteObject();
        if (plant != null && plant.model != null) {
            this.objModel = plant.model;
            this.obj = mp.objects.new(mp.game.joaat(plant.model), this.Position, {
                rotation: new mp.Vector3(0, 0, 0),
                dimension: 0
            });
            this.obj.position = this.Position;
            this.obj.setRotation(0, 0, 0, 0, true);
            
        }
    }

    //Получаем данные для создания модели
    GetCurrentStatus() {
        let configPlant = plantsConfig[this.PlantName]
        if (this.PlantName < 0 || !configPlant)
            return null;
        let currDate = Date.now();
        let configPoint = pointsConfig[this.FarmId][this.PointId];
        if (this.Position === null)
        {
            this.Position = configPoint.Position;
            // const zCoord = mp.game.gameplay.getGroundZFor3dCoord(this.Position.x, this.Position.y, 1000, 0.0, false);
            // this.Position.z = zCoord;
        }
        if (!pointsConfig[this.FarmId] || !pointsConfig[this.FarmId][this.PointId])
            return null;
        let timePassed = currDate / 1000 - this.PlantingTime; //Времени прошло с момента посадки
        let isWatering = this.WateringTime > this.PlantingTime && (this.WateringTime - this.PlantingTime) < configPlant.SecondBeforeWatering; //Полито ли растение
        let coeffRipeningTime = 1 - pitsConfig[configPoint.PitType].TimeCoeff - fertilizersConfig[this.Fertilizer].TimeCoeff; //получаем коэффициент времени созревания
		if (this.BuffedByPet) coeffRipeningTime /= 2;
        let currentRipeningTime = configPlant.RipeningTime * coeffRipeningTime; //Получаем время созревания со всеми плюшками
        let currentStage = timePassed / currentRipeningTime; // получаем стадию растения
        if ((!isWatering && timePassed > configPlant.SecondBeforeWatering) || (timePassed > currentRipeningTime + configPlant.WitheringTime)) //если растение не полито и время полива закончилось, либо если уже завяло
            currentStage = 333;
        let modelName = configPlant.Stages.filter(item => item.Time > currentStage)[0].ModelName;
        return {
            plantName: configPlant.Name,
            model: modelName, //Модель растения
            isWatering: isWatering, //Полито ли растение
            isFertilizering: this.Fertilizer, //Удобрено ли растение
            timeToRipening: currentRipeningTime - timePassed, //время до созревания
            currentRipeningTime: currentRipeningTime, //полное время роста
            witheringTime: configPlant.WitheringTime, //время увядания
            currentStage: currentStage, //стадия созревания
        }
    }
}

module.exports = Plant;