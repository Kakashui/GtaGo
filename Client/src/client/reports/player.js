mp.events.add("report:player:answer", (id, name, text)=>{
    global.gui.setData("optionsMenu/addAnswer", JSON.stringify({id,name,text}));
    global.gui.setData('sounds/play','{"name":"reportMsg","volume":0.2}');
})