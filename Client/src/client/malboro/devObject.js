
const mouseSates = {
	"release": 0,
	"left": 1,
	"right": 2
}

module.exports = {
    object: null,
    modelName: "",
    speed: .1,
    position: null,
    rotation: null,
    //vw_prop_casino_roulette_01
    createObject(model){
        if(this.object !== null) this.deleteObject();
        const hash = mp.game.joaat(model);
        if(!mp.game.streaming.isModelValid(hash)) return;
        this.modelName = model;
        if(!mp.game.streaming.hasModelLoaded(hash)){
            mp.game.streaming.requestModel2(hash);
            while (!mp.game.streaming.hasModelLoaded(hash)) {
                mp.game.wait(0)
            }
        }
        this.position = mp.players.local.getOffsetFromInWorldCoords(0, 1, 0);
        this.rotation = new mp.Vector3();
        this.object = mp.objects.new(hash, mp.players.local.getOffsetFromInWorldCoords(0, 1, 0), {dimension: mp.players.local.dimension, alpha: 200, rotation:  this.rotation} );
        this.object.setCollision(false, false);
        global.showCursor(true);
    },
    deleteObject(){
        if(this.object === null) return;
        mp.events.callRemote("dev:obj:save", 
        `${this.modelName}, new Vector3(${this.position.x.toFixed(4)},${this.position.y.toFixed(4)},${this.position.z.toFixed(4)}), new Vector3(${this.rotation.x},${this.rotation.y},${Math.floor(this.rotation.z)})`);
        //this.object.destroy();
        this.object = null;
        this.modelName = "";
    },
    update(){
        this.object.setCoordsNoOffset(this.position.x,this.position.y, this.position.z,true, true, true);
        this.object.setRotation(this.rotation.x, this.rotation.y, this.rotation.z, 2, true);
    },
    onMouseMove(mouse){
        if(this.object === null) return;
        if(mouse.state === mouseSates.right){
            this.rotate(mouse);
        }else if(mouse.state === mouseSates.left){
            if(mp.keys.isDown(global.Keys.Key_ALT)){
                this.speed = .1;
            } else {
                this.speed = .025;
            }
            if(mp.keys.isDown(global.Keys.Key_CONTROL))
                this.moveZ(mouse);			
            else 
                this.moveXY(mouse);
        }
    },
    moveXY(mouse){
        const camDir = global.gameplayCam.getDirection();
        const right = global.GetOffsetPosition(new mp.Vector3(), 90, camDir);
        const dirX = mouse.dirrectX;
        const dirY = mouse.dirrectY;
        this.position.x += right.x * dirX * this.speed;
		this.position.y += right.y * dirX * this.speed;
		this.position.x += camDir.x * dirY * this.speed;
		this.position.y += camDir.y * dirY * this.speed;
        this.update();
    },
    moveZ(mouse){
        this.position.z += mouse.dirrectY * this.speed;
        this.update();
    },
    rotate(mouse){
        this.rotation.z +=  mouse.dirrectX * this.speed * 50
        this.update();
    }
}