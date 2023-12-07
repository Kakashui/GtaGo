module.exports = class TestClass{
    constructor() {
        
        mp.events.add("render", this.tick);
    }

    destroy(){        
        mp.events.remove("render", this.tick);
    }

    tick(){
        mp.gui.chat.push("check 1");
    }
}