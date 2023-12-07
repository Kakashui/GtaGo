class CustomCamera{
    
    constructor() {
        this.dist = 1.2;
        this.isActive = false;
        this.speed = .025;
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
        this.temp = {
            pointX: 0,
            pointY: 0,
            pointZ: 0,
            x: 0,
            y: 0,
            z: 0,
            dist: 0
        }
        this.camera = mp.cameras.new('default',new mp.Vector3(0, 10, 90) , new mp.Vector3(-95, 19, 0), 50);
    }

    update(){      
        try {
            if(this.self.pos){
                this.camera.setCoord(
                    this.self.pos.x + this.self.offsetX + this.temp.x,
                    this.self.pos.y + this.self.offsetY + this.temp.y,
                    this.self.pos.z + this.self.offsetZ + this.temp.z
                );
            }        
            if(this.point.pos){                
                this.camera.pointAtCoord(
                    this.point.pos.x + this.point.offsetX + this.temp.pointX,
                    this.point.pos.y + this.point.offsetY + this.temp.pointY,
                    this.point.pos.z + this.point.offsetZ + this.temp.pointZ
                )
            }
        } catch (error) {
            mp.serverLog(`Update error ${error.message}`);
        }
    }

    moveAngleX(angle){
        try {
            const rad = (angle) * Math.PI/180;
            this.temp.x = (this.dist) * Math.cos(rad);
            this.temp.y = (this.dist) * Math.sin(rad);
        } catch (error) {            
            mp.serverLog(`moveAngleX error ${error.message}`);
        }
    }

    movePointZ(offset){
        this.temp.pointZ = offset;
        this.update();
    }

    moveCamX(offset){
        this.temp.x = offset;
        this.update();
    }
    
    moveCamZ(offset){
        this.temp.z = offset;
        this.update();
    }

    setDist(dist){
        this.temp.dist = dist;
        this.update();
    }

    setPoint(pos){
        this.point.pos = pos;
        this.update();
    }

    setPos(pos){
        this.self.pos = pos;
        this.update();
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
        this.isActive = true;
    }

    switchOff(time){
        this.reserOffsets();
        this.camera.setActive(false);
        mp.game.cam.renderScriptCams(false, true, time, true, false);
        this.isActive = false;
    }
    mouseMove(x, y){
        this.moveAngleX(x * this.speed * 50);
        this.moveCamZ(y * this.speed * -1)
    }

    mouseUp(isLeft){
        if(isLeft){
            this.self.offsetX += this.temp.x;
            this.self.offsetY += this.temp.y;
            this.self.offsetZ += this.temp.z;
            // this.point.offsetX += this.temp.pointX;
            // this.point.offsetY += this.temp.pointY;
            // this.point.offsetZ += this.temp.pointZ;
            //this.dist += this.temp.dist;
            this.temp.x = 0;
            this.temp.y = 0;
            this.temp.z = 0;
            // this.temp.pointX = 0;
            // this.temp.pointY = 0;
            // this.temp.pointZ = 0;
            this.temp.dist = 0;
            this.update();
        }        
    }
}

module.exports = new CustomCamera()