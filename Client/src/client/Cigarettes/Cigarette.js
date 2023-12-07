module.exports = class Cigarette{
  constructor(player) {
      this.obj = null;
      this.model = "ng_proc_cigarette01a";
      this.boneId = 64097;
      this.offsetX = 0;
      this.offsetY = .025;
      this.offsetZ = 0; 
      this.rotateX = 95;
      this.rotateY = -25;
      this.rotateZ = 80;
      this.create(player);
  }

  create(player){
    if(this.obj == null){
      const model = mp.game.joaat(this.model);
      if(!mp.game.streaming.isModelValid(model)) return;
      if(!mp.game.streaming.hasModelLoaded(model)){
        mp.game.streaming.requestModel2(model);
        for (let index = 0;!mp.game.streaming.hasModelLoaded(model) && index < 250; index++) {
          mp.game.wait(0);          
        }
      }
      this.obj = mp.objects.new(model, player.position);
      for (let index = 0; !this.obj.doesExist() && !this.obj.handle !== 0 && index < 250; index++) {
        mp.game.wait(0);          
      }        
      this.attach(player.handle);
    }
  }
  
  attach(handle){
    if(this.obj == null && handle != 0) return;
    this.obj.attachTo(handle, mp.players.local.getBoneIndex(this.boneId), this.offsetX,this.offsetY,this.offsetZ, this.rotateX,this.rotateY,this.rotateZ,  false, true, false, false, 0, true);
  }
  
  destroy(){
    if(this.obj == null) return;
    this.obj.detach(true, true);
    this.obj.destroy();
  }
}