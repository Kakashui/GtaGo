export default class {
    constructor(adress, item, clientX, clientY) {
        this.adress = adress;
        this.item = item;
        this.clientX = clientX;
        this.clientY = clientY
        this.overAdress = "";
        this.alt = false;
        this.dragStart = false;
    }

    mouseMoveHandle(e){
        if(!this.dragStart){
            if(Math.abs(this.clientX - e.clientX) > 10 || Math.abs(this.clientY - e.clientY) > 10) 
                this.startDrag(e.clientX, e.clientY)
        }else{
            this.clientX = e.clientX;
            this.clientY = e.clientY;
            const els = document.elementsFromPoint(this.clientX, this.clientY);
            if(els[0].className.indexOf("nopoint") !== -1) {
                this.overAdress = null;
                return;
            }
            const el = els.find(el=>el.id == "drop");
            if(this.el == el) return;
            this.el = el;            
            if(this.el == undefined) this.overAdress = null;
            else{
                const over = this.el.getAttribute("adress");
                const overSplit = over.split('_');
                if(overSplit[0] == "access"){
                    this.overAdress = (this.item.isUsable() || (this.item.isWeapon())  ? over : null);
                }else
                    this.overAdress = over;
            } 
        }
    }

    startDrag(clientX, clientY){
        this.clientX = clientX;
        this.clientY = clientY;
        this.dragStart = true;
    }
}