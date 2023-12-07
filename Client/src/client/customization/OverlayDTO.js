module.exports = class OverlayDTO{
    constructor(overlayId, index, color1, color2, opacity) {
        this.OverlayId = parseInt(overlayId);
        this.Index = parseInt(index);
        this.Color1 = parseInt(color1);
        this.Color2 = parseInt(color2);
        this.Opacity = parseInt(opacity);
        
        if(this.OverlayId === 4 || this.OverlayId === 5 || this.OverlayId === 8 ){
            this.ColorType = 2;        
        }            
        else if(this.OverlayId === 1 || this.OverlayId === 2 || this.OverlayId === 10 ){
            this.ColorType = 1;
        }else
            this.ColorType = 0;
    }

    updateIndex(value){
        this.Index = parseInt(value);
        this.apply();
    } 
    
    updateColor1(value){
        this.Color1 = parseInt(value);
        this.apply();
    }
    
    updateColor2(value){
        this.Color2 = parseInt(value);
        this.apply();
    }
    
    updateOpacity(value){
        this.Opacity = parseInt(value);
        this.apply();
    }

    apply(){      
        mp.players.local.setHeadOverlay(this.OverlayId, this.Index, this.Opacity, 0, 0);
        mp.players.local.setHeadOverlayColor(this.OverlayId, this.ColorType, this.Color1, this.Color2);
    }
}