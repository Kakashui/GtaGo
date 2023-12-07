const OverlayDTO = require('./OverlayDTO')
const ClothesDTO = require('./ClothesDTO')
const CustomizationCamera = require('./CustomizationCamera')
const ParentsDTO = require('./ParentsDTO')
const FaceFeaturesDTO = require('./FaceFeaturesDTO')
const HairsDTO = require('./HairsDTO')


class Customization{
    constructor() {
        this.slot = -1;
        this.gender = true;
        this.eyeColor = 0;
        this.overlayData = {};
        this.features = {};
        
        this.clothes = new ClothesDTO(14, 0, 1);
        this.parents = new ParentsDTO(0, 21, .5, .5);
        this.hairs = new HairsDTO(0,0,0);
        this.customizationCamera = new CustomizationCamera();

        this.initOverlays();
        this.initFeatures();
        this.loadEvents();
    }

    loadEvents(){
        mp.events.add('customization:gender:change', gender=> this.changeGender(gender));
        mp.events.add('customization:update', (param, value) => this.updateCustomization(param, value));
        mp.events.add('customization:save', (firstName, lastName) => this.saveCharacter(firstName, lastName));
        mp.events.add("customization:destroy", ()=>this.customizationCamera.destroy());
        mp.events.add("customization:create", (slot)=>this.create(slot));
        mp.events.add("customization:camera:switch", (isCloth)=>this.switchCamera(isCloth));
    }

    create(slot){
        this.slot = slot;
        global.gui.setData("customization/isNotFirstCreate", this.slot < 0 );
        global.gui.setData("customization/resetGender");
        mp.game.cam.doScreenFadeOut(0);
        global.gui.setData("setBackground", "0");
        mp.players.local.freezePosition(true);
        global.characterEditor = true;
        global.gui.close();
        setTimeout(()=>{
            this.customizationCamera.loadEditorCamera();            
            this.changeGender(true);
            global.gui.close();
            global.gui.openPage("Customization");
            global.gui.playSound("editor_bg", .02, true);
            mp.game.cam.doScreenFadeIn(1000);
        }, 700)
    }

    switchCamera(isCloth){
        this.customizationCamera.switch(isCloth)
        this.clothes.update(isCloth);
    }

    initOverlays(){
        for (let index = 0; index < 13; index++) {
            this.overlayData[index] = new OverlayDTO(index, -1, 0, 0, 1);
        }
    }

    initFeatures(){
        for (let i = 0; i < 20; i++){
            this.features[i] = new FaceFeaturesDTO(i, 0.0);
        } 
    }

    changeGender(gender){
        this.gender = gender;
        this.clothes.changeGender(this.gender);
        if (this.gender) {
            mp.players.local.model = mp.game.joaat('mp_m_freemode_01');           
        }
        else {
            mp.players.local.model = mp.game.joaat('mp_f_freemode_01');
        }
    
        this.parents.apply();
        this.updateOverlays();
        this.clothes.setDefault();
        this.hairs.apply();
        this.updateFeatures();
    }    

    updateOverlays() {
        Object.values(this.overlayData).forEach(o=>o.apply())   
        mp.players.local.setEyeColor(this.eyeColor);
    }

    updateFeatures(){
        for (let i = 0; i < 20; i++) 
            this.features[i].apply();
    }

    updateCustomization(param, value){
        switch (param) {
            case "similar":
                this.parents.updateSimilarity(value);
                break;
            case "skin":
                this.parents.updateSkin(value);
                break;
            case "noseWidth": 
                this.features[0].setValue(value)
                break;
            case "noseHeight": 
                this.features[1].setValue(value);
                break;
            case "noseLength": 
                this.features[2].setValue(value); 
                break;
            case "noseBridge":
                this.features[3].setValue(value);
                break;
            case "noseTip":
                this.features[4].setValue(value);
                break;
            case "noseBridgeShift":
                this.features[5].setValue(value); 
                break;
            case "browHeight":
                this.features[6].setValue(value);
                break;
            case "browWidth": 
                this.features[7].setValue(value); 
                break;
            case "cheekboneHeight": 
                this.features[8].setValue(value);
                break;
            case "cheekboneWidth": 
                this.features[9].setValue(value);
                break;
            case "cheekWidth": 
                this.features[10].setValue(value);
                break;
            case "eyes": 
                this.features[11].setValue(value);
                break;
            case "lips": 
                this.features[12].setValue(value); 
                break;
            case "jawWidth": 
                this.features[13].setValue(value);
                break;
            case "jawHeight": 
                this.features[14].setValue(value);
                break;
            case "chinLength": 
                this.features[15].setValue(value);
                break;
            case "chinPosition":                 
                this.features[16].setValue(value);
                break;
            case "chinWidth": 
                this.features[17].setValue(value); 
                break;
            case "chinShape": 
                this.features[18].setValue(value); 
                break;
            case "neckWidth": 
                this.features[19].setValue(value);
                break;
            case "father":
                this.parents.setFather(value);
                break;
            case "mother":
                this.parents.setMother(value);
                break;
                    //Hair colors
            case "hair":
                this.hairs.setHair(value);
                break;
            case "hairColor1":                
                this.hairs.setColor1(value);
                break;
            case "hairColor2":
                this.hairs.setColor2(value);
                break;
            case "eyebrows":
                this.overlayData[2].updateIndex(value);
                break;
            case "eyebrowsColor1":
                this.overlayData[2].updateColor1(value);
                break;
            case "eyebrowsColor2":
                this.overlayData[2].updateColor2(value);
                break;            
            case "beard":
                let overlay = (value == 0) ? 255 : value - 1;
                this.overlayData[1].updateIndex(overlay);
                break;
            case "beardColor1":
                this.overlayData[1].updateColor1(value);
                break;
            case "beardColor2":
                this.overlayData[1].updateColor2(value);
                break;
            case "chest":
                this.overlayData[10].updateIndex(value);
                break;
            case "chestColor1":
                this.overlayData[10].updateColor1(value);
                break;
            case "chestColor2":
                this.overlayData[10].updateColor2(value);
                break;
                    //Makeup colors
            case "makeup":            
                this.overlayData[4].updateIndex(value);
                break;
            case "makeupColor1":
                this.overlayData[4].updateColor1(value);
                break;
            case "makeupColor2":
                this.overlayData[4].updateColor2(value);
                break;
            case "makeupOpacity":
                this.overlayData[4].updateOpacity(value);
                break;
            case "blush":
                this.overlayData[5].updateIndex(value);
                break;
            case "blushColor1":
                this.overlayData[5].updateColor1(value);
                break;
            case "blushColor2":
                this.overlayData[5].updateColor2(value);
                break;
            case "blushOpacity":
                this.overlayData[5].updateOpacity(value);
                break;
            case "lipstick":
               this.overlayData[8].updateIndex(value);
                break;
            case "lipstickColor1":
                this.overlayData[8].updateColor1(value);
                break;            
            case "lipstickColor2":
                this.overlayData[8].updateColor2(value);
                break;
            case "lipstickOpacity":
                this.overlayData[8].updateOpacity(value);
                break;

                    //skin
            case "complexion":
                this.overlayData[6].updateIndex(value);
                break;
            case "sunDamage":
                this.overlayData[7].updateIndex(value);
                break;
            case "bodyBlemish":
                this.overlayData[11].updateIndex(value);
                break;
            case "ageing":                
                this.overlayData[3].updateIndex(value);
                break;
            case "blemish":
                this.overlayData[0].updateIndex(value);
                break;            
            case "moles":
                this.overlayData[9].updateIndex(value);
                break;

                    //clothes
            case "top":
                this.clothes.updateShirt(value);
                break;
            case "pants":
                this.clothes.updatePants(value);
                break;
            case "shoes":
                this.clothes.updateShoes(value);
                break;
            case "eyesColor":
                this.eyeColor = value;
                mp.players.local.setEyeColor(this.eyeColor);
                break;
            default: break;
        }        
        
    }

    saveCharacter(firstName, lastName){
        setTimeout(()=>{
            mp.events.callRemote(
                "customization:save", 
                this.slot, firstName, lastName, this.gender, this.eyeColor,                
                JSON.stringify(this.parents),                 
                JSON.stringify(this.hairs), 
                JSON.stringify(Object.values(this.features)), 
                JSON.stringify(this.overlayData), 
                JSON.stringify(this.clothes)
            );                
        }, 800);
    }
}

global.customize = new Customization();