class CustomCamera{
    constructor() {       
        this.dist = 0;
        this.point = {
            pos: null,
            offsetX: 0,
            offsetY: 0,
            offsetZ: 0
        }
        this.self = {
            pos: null,
            offsetX: 0,
            offsetY: 0,
            offsetZ: 0
        }
        this.camera = mp.cameras.new('default',new mp.Vector3(0, 10, 90) , new mp.Vector3(-95, 19, 0), 50);
    }

    update(){      
        try {
            if(this.self.pos){
                this.camera.setCoord(
                    this.self.pos.x + +this.self.offsetX,
                    this.self.pos.y + +this.self.offsetY,
                    this.self.pos.z + +this.self.offsetZ
                );
            }        
            if(this.point.pos){                
                this.camera.pointAtCoord(
                    this.point.pos.x + +this.point.offsetX,
                    this.point.pos.y + +this.point.offsetY,
                    this.point.pos.z + +this.point.offsetZ
                )
            }
        } catch (error) {
            mp.serverLog(`Update error ${error.message}`);
        }
    }

    moveAngleX(angle){
        try {
            const rad = angle * Math.PI/180;
            this.self.offsetX = this.dist * Math.cos(rad);
            this.self.offsetY = this.dist * Math.sin(rad);
        } catch (error) {            
            mp.serverLog(`moveAngleX error ${error.message}`);
        }
    }

    movePointZ(offset){
        this.point.offsetZ = offset;
    }

    moveCamX(offset){
        this.self.offsetX = offset;
    }
    
    moveCamZ(offset){
        this.self.offsetZ = offset;
    }

    setDist(dist){
        this.dist = dist;
    }

    setPoint(pos){
        this.point.pos = pos;
    }

    setPos(pos){
        this.self.pos = pos;
    }

    reserOffsets(){
        this.self.offsetX = 0;
        this.self.offsetY = 0;
        this.self.offsetZ = 0;
        this.point.offsetX = 0;
        this.point.offsetY = 0;
        this.point.offsetZ = 0;
    }

    switchOn(time, posCam, posPoint){
        if(posCam) this.setPos(posCam);
        if(posPoint) this.setPoint(posPoint);
        this.update();
        this.camera.setActive(true);
        mp.game.cam.renderScriptCams(true, (time > 0), time, true, false);
    }

    switchOff(time){
        this.reserOffsets();
        this.camera.setActive(false);
        mp.game.cam.renderScriptCams(false, true, time, true, false);
    }
}

global.customCamera = new CustomCamera();

mp.events.add("camMoveAngleX", (val)=>{
    //mp.serverLog(`camMoveAngleX ${val}`);
    global.customCamera.moveAngleX(val);
    global.customCamera.update();
});
mp.events.add("camMovePointZ", (val)=>{
    //mp.serverLog(`camMovePointZ ${val}`);
    global.customCamera.movePointZ(val);
    global.customCamera.update();
});
mp.events.add("camMoveCamZ", (val)=>{
    //mp.serverLog(`camMoveCamZ ${val}`);
    global.customCamera.moveCamZ(val);
    global.customCamera.update();
});
mp.events.add("camMoveCamX", (val)=>{
    //mp.serverLog(`camMoveCamX ${val}`);
    global.customCamera.moveCamX(val);
    global.customCamera.update();
});
mp.events.add("camSetDist", (val, angle)=>{
    //mp.serverLog(`camSetDist ${val}`);
    global.customCamera.setDist(val);
    global.customCamera.moveAngleX(angle)
    global.customCamera.update();
}); 
