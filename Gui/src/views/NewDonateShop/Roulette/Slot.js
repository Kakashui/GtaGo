let effect = new Image(11400, 1200);
effect.src = "/img/newDonateShop/roulette/effects/win_sprite.png";
const frame = {
    index: 0,
    x: 0,
    y: 0,
    size: 600,
    frame: 1000 / 38,
    lastUpdate: 0,
    next(){
        if(frame.lastUpdate > Date.now()) return;
        this.lastUpdate = Date.now() + this.frame;
        this.index++;
        if(this.index >= 38)this.index = 0;
        this.y = Math.floor(this.index / 19) * this.size;
        this.x = this.index % 19 * this.size;
    }   
}
export default class Slot{    
    constructor(ctx, item, size, bg, offset) {
        this.ctx = ctx;
        this.padding = 20;
        this.imgScale = .8;
        this.size = size;
        this.color = item.color;
        this.posX = offset;       

        this.bg = {
            img: bg,
            y: this.padding,
            size: this.size - this.padding * 2
        }
        this.item = {
            img: item.img,
            blure: item.blure,
            y: this.padding,
            size: this.size - this.padding * 2,
        },
        this.inMove = false
    }

    draw(){
        this.ctx.lineWidth = 3;
        this.ctx.strokeStyle = this.color;
        this.ctx.shadowBlur = 20;
        this.ctx.shadowColor = this.color;
        this.ctx.drawImage(this.bg.img, this.bgLocalOffset(), this.bg.y, this.bg.size, this.bg.size);
        this.ctx.drawImage(this.item.img, this.imgLocalOffset(), this.item.y, this.item.size, this.item.size);
        this.ctx.beginPath();
        this.ctx.arc(this.localCenter(), this.size / 2, this.bg.size / 2, 0, 2 * Math.PI); 
        this.ctx.stroke();
    }
    drawEffect(){
        this.ctx.shadowColor = "#EA472E";
        this.ctx.drawImage(effect, frame.x, frame.y, frame.size, frame.size, this.posX-this.padding / 2, 0, this.size, this.size);
        frame.next();
    }

    fixPos(fix){
        this.x -= fix;
    }

    moveX(speed){
        this.inMove = speed > 50;
        this.posX -= speed;
    }

    imgLocalOffset(){
        return this.posX + this.padding / 2;
    }
    canDelete(){
        return this.posX < -this.size;
    }

    bgLocalOffset(){
        return this.posX + this.padding / 2;
    }

    localCenter(){
        return this.posX + (this.size - this.padding) / 2 ;
    }
    
}