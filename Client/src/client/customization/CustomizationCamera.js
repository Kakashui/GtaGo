module.exports = class CustomizationCamera{
    constructor() {
        this.isClothes = false;
        this.moveSettings = {
            size: {
                x: 45,
                y: 40
            },
            showIcons: [false, false, false, false],
            values:[
                {//LEFT X
                    value: 30,
                    min: -50,
                    max: 110,
                    step: 5,
                    invert: true,
                    enabled: true,
                    callback: "camMoveAngleX"
                },
                {//LEFT Y
                    value: 0.0,
                    min: -.25,
                    max: .25,
                    step: .05,
                    invert: false,
                    enabled: true,
                    callback: "camMoveCamZ"
                },
                {//RIGHT X
                    value: 2,
                    min: 1,
                    max: 3,
                    step: .1,
                    invert: false,
                    enabled: false,
                    callback: ""
                },
                {//RIGHT Y
                    value: 2,
                    min: 1,
                    max: 3,
                    step: .1,
                    invert: true,
                    enabled: false,
                    callback: ""
                },
                { //WHEELE
                    value: -1,
                    min: -1,
                    max: -.5,
                    step: .1,
                    invert: true,
                    enabled: true,
                    callback: "camSetDist"
                }
            ]
        }        
    }

    switch(trigger){
        if(this.isClothes === trigger) return;
        this.isClothes = trigger;
        if(this.isClothes)
            this.loadClothesCamera();
        else
            this.loadEditorCamera();
    }

    loadClothesCamera(){
        const point = mp.players.local.getBoneCoords(0,0.05,0,0);
        const camera = mp.players.local.getBoneCoords(0,0.05,0.2,0);
    
        global.customCamera.setPos(camera );
        global.customCamera.setPoint(point);
        global.customCamera.moveCamZ(0.05);
        global.customCamera.setDist(-2.5);
        global.customCamera.moveAngleX(30);
        global.customCamera.switchOn(0);
        this.disabeCameraMove();
        global.gui.setData('mouseMove/setSettings', JSON.stringify(this.moveSettings));
        global.gui.setData('mouseMove/setEnebled', true);
    }

    loadEditorCamera(){
        const point = mp.players.local.getBoneCoords(12844,0.05,0,0);
        const camera = mp.players.local.getBoneCoords(12844,0.05,0.2,0);
    
        global.customCamera.setPos(camera );
        global.customCamera.setPoint(point);
        global.customCamera.moveCamZ(0.05);
        global.customCamera.setDist(-1);
        global.customCamera.moveAngleX(30);
        global.customCamera.switchOn(0);
        this.enableCameraMove();
        global.gui.setData('mouseMove/setSettings', JSON.stringify(this.moveSettings));
        global.gui.setData('mouseMove/setEnebled', true);
    }

    disabeCameraMove(){
        this.moveSettings.values[0].enabled = false;
        this.moveSettings.values[1].enabled = false;
        this.moveSettings.values[4].enabled = false;
    }

    enableCameraMove(){
        this.moveSettings.values[0].enabled = true;
        this.moveSettings.values[1].enabled = true;
        this.moveSettings.values[4].enabled = true;
    }

    destroy(){
        global.gui.close();
        global.characterEditor = false;
        global.gui.setData('mouseMove/setEnebled', false);
        global.gui.stopSound();
        mp.players.local.freezePosition(false);
        global.customCamera.switchOff(0);

        mp.events.call('showAltTabHint');
        global.showHud(true);
    }
}