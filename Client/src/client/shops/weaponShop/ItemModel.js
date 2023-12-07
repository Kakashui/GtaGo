const SET_ENTITY_ROTATION = "0x8524A8B0171D5E07";

module.exports = class ItemModel{
    constructor(player, id, model, position = undefined, heading = undefined) {         
        this.id = id;   
        this.model = +model;   
        this.player = player;   
        this.position = position;
        this.rotateX = 0;
        this.rotateY = 0;
        this.rotateZ = heading;
        this.createModel();   
        this.updateRotation();     
    }
    
    createModel(){
        if(!mp.game.streaming.isModelValid(this.model)) return;
        if(!mp.game.streaming.hasModelLoaded(this.model)){
            mp.game.streaming.requestModel2(this.model);
            for (let index = 0;!mp.game.streaming.hasModelLoaded(this.model) && index < 250; index++) {
                mp.game.wait(0);
            };
        }

        const pos = this.position === undefined ? 
            new mp.Vector3(this.player.position.x, this.player.position.y, this.player.position.z) : 
            this.position;       
        this.object = mp.game.object.createObject(this.model, pos.x, pos.y, pos.z,true, true,true);
        if(this.rotateZ !== undefined) this.updateRotation();      
        return true;
    }

    updateRotation(){
        mp.game.invoke(SET_ENTITY_ROTATION, this.object, +this.rotateX + .01, +this.rotateY + .01, +this.rotateZ + .01, 1, true)
    }

    removeComponent(a, b){

    }

    setComponent(a,b){
        
    }

    destroy(){
        mp.game.object.deleteObject(this.object);
    }
}