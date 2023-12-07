module.exports = class Console{
    constructor() {
        this.opened = false;
        mp.keys.bind(global.Keys.Key_F2, false, ()=>{
            if(this.opened){
                global.showCursor(false);
                global.chatActive = false;
                global.gui.setData('devTools/consoleVisible', false);
                this.opened = false;
            }else{
                global.showCursor(true);
                global.chatActive = true;
                global.gui.setData('devTools/consoleVisible', true);
                this.opened = true;
            }
        })
    }

    log(msg){
        global.gui.setData('devTools/addConsoleMessage', JSON.stringify({msg, output: false}));
    }    
  
}