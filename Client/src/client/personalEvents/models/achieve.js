class Achieve {
    constructor (level, reward, date, achieveConf) {
        this.CurrentLevel = level;
        this.GivenReward = reward;
        this.DateCompleated = date;
        this.AchieveConfig = achieveConf;
        if (this.AchieveConfig.IsClient)
        {
            global.personalEventsSubscribe(this, this.AchieveConfig.PlayerActionName);
        }
        this.SendToCef();
    }
    UpdateProgress(level, reward, date) {
        this.CurrentLevel = level;
        this.GivenReward = reward;
        this.DateCompleated = date;
        this.SendToCef();
    }
    AchieveProgress(point) {
        if (!this.AchieveConfig.IsClient)
            return;
        if (this.CurrentLevel < this.AchieveConfig.MaxLevel && this.CurrentLevel + point >= this.AchieveConfig.MaxLevel){
            mp.events.call("personalEvents:needSync", this.AchieveConfig.PlayerActionName)
        }
        this.CurrentLevel += point
        this.SendToCef();
    }
    SendToCef() {
        global.gui.setData('optionsMenu/updateAchievements', JSON.stringify({
            key: this.AchieveConfig.AchieveName, 
            value: {
                CurrentLevel: this.CurrentLevel,
                GivenReward: this.GivenReward,
                DateCompleated: this.DateCompleated,
            }
        }));        
    }
}

module.exports = Achieve;