global.farmAction = (index)=>{
    if(global.farmConfig[index] != undefined && global.farmConfig[index].isgrow){
        mp.events.callRemote("farm::action", index);
    }
}

mp.events.add("farm::destroy", (index)=>{
    deleteProduct(index);
});

mp.events.add("farm::load", (data)=>{
    setTimeout(
        ()=>{
            global.farmConfig.forEach((product, index) => {
                product.isgrow = data[index];
                if(product.isgrow){
                    CreateProduct(product, index);
                }
            });
    }, 0);
})

mp.events.add("playerEnterColshape", (shape)=>{
    if(shape.index != undefined) global.localplayer.farmAction = shape.index;
});

mp.events.add("playerExitColshape", (shape)=>{
    if(shape.index != undefined) global.localplayer.farmAction = -1;
});

function CreateProduct(product, index){
    product.obj = mp.objects.new(mp.game.joaat(product.model), product.pos,
    {            
        dimension: 0
    });
    const csh = mp.colshapes.newCircle(product.pos.x,product.pos.y,.5);
    csh.index = index;
}

function deleteProduct(index){
    if(!product.obj) return;
    const product = global.farmConfig[index];
    product.obj.destroy();
    product.isgrow = false;
    mp.colshapes.forEach(csh=>{
        if(csh.index == index){
            csh.destroy();
        }
    })
}