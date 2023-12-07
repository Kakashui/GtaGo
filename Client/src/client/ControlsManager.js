class ControlsManager{
    constructor() {
        this.blockSprint = false;
        this.blockJump = false;
        this.blockAll = false;
        this.butEnabled = [];
        mp.events.add("render", this.onEachFrame.bind(this));
    }
    canSprintJump(sprint = false, jump = false){
        this.blockSprint = sprint
        this.blockJump = jump;
    } 
    disableAll(...excludes){
        this.butEnabled = excludes || [];
        this.blockAll = true;
    } 
    enableAll(){
        this.blockAll = false;
        this.butEnabled = [];
    }
    onEachFrame(){
        if(this.blockAll){
            mp.game.controls.disableAllControlActions(0);
            this.butEnabled.forEach(control => {
                mp.game.controls.enableControlAction(0, control, true);                
            });
            return;
        }
        if(this.blockSprint) 
            mp.game.controls.disableControlAction(0, 21, true);

        if(this.blockJump) 
            mp.game.controls.disableControlAction(0, 22, true);
    }
}

module.exports = new ControlsManager();