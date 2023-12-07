module.exports = class KeyAction{
    constructor(keyCode) {
        this.keyCode = keyCode;
        this.actionsOnDown = [];
        this.actionsOnUp = [];
        this.pressed = false;
    }

    tick(){
        if(this.pressed){
            if(mp.keys.isUp(this.keyCode)){
                this.actionsOnUp.forEach(callback => {
                    callback();
                });
                this.pressed = false;
            }
            
        }else{
            if(mp.keys.isDown(this.keyCode)){
                this.actionsOnDown.forEach(callback => {
                    callback();
                });
                this.pressed = true;
            }            
        }
    }

    onPress(){
        try {
            if(this.pressed) return;
            this.actionsOnDown.forEach((callback) => {
                callback();
            });
            this.pressed = true;
        } catch (e) {
            if(global.sendException)mp.serverLog(`action.onPress: ${e.name }\n${e.message}\n${e.stack}`);
        }
    } 

    onUp(){
        try {
            this.actionsOnUp.forEach(callback => callback());
            this.pressed = false;
        } catch (e) {
            if(global.sendException)mp.serverLog(`action.onUp: ${e.name }\n${e.message}\n${e.stack}`);
        }
        
    }

    changeKey(keyCode){
        this.keyCode = keyCode;
    }

    subscribe(action, isDown = false){        
        const actions = isDown ? this.actionsOnDown : this.actionsOnUp;
        actions.push(action);
    }

    unsubscribe(action, isDown = false){
        const actions = isDown ? this.actionsOnDown : this.actionsOnUp;
        const index = actions.findIndex(a=>a === action);
        if(index !== -1) actions.splice(index, 1);
    }
}