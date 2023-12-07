const validImgs = [
    "eco",
    "recycle",
    "atom"
];
const invalidImgs = [
    "biohazard",
    "radiation",
    "pete",
    "danger"
];
const tempImg =  "thermometer";

const bgByType = {
    "valid": "green_bg",
    "invalid": "red_bg",
    "tempUp": "green_bg_t",
    "tempDown": "red_bg_t"
}

export default class Element{
    constructor(type, scale = -1, variant = -1) {
        this.type = type;
        this.scale = scale < 0 ? Math.max(.75, Math.random() * 1.5).toFixed(2) : scale;
        this.bg = `/img/miniGames/metalPlant/elements/${bgByType[type]}.svg`;
        if(this.type === "tempUp" || this.type === "tempDown")
            this.img = `/img/miniGames/metalPlant/elements/${tempImg}.svg`;
        else{

            if(this.type === "valid") 
                this.img = `/img/miniGames/metalPlant/elements/${validImgs[variant] ? validImgs[variant] : validImgs[Math.floor(Math.random() * validImgs.length)]}.svg`;
            else
                this.img = `/img/miniGames/metalPlant/elements/${invalidImgs[variant] ? invalidImgs[variant] : invalidImgs[Math.floor(Math.random() * invalidImgs.length)]}.svg`;
        }
        this.x = 0;
        this.y = 0;
    }

    initPositions(x, y){
        this.x = x;
        this.y = y;
    }

    initSpeed(min, max){
        this.speed = Math.floor((Math.random() * (max - min)) + min);
    }

    update(){
        this.y += this.speed;
    }
            
}