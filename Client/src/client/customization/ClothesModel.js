module.exports = class ClothesModel{
    constructor(type ,drawableId, bodyId, undershitId) {
        this.type = type;
        this.drawable = drawableId;
        this.body = bodyId;
        this.undershit = undershitId;
    }    
}