let lasViewMode = 4;
global.phoneCameraIsOpened = false;


mp.events.add('phone::camera::open', () => {
    global.closeGoPhone();    
    lasViewMode = mp.game.invoke(global.getNative("GET_FOLLOW_PED_CAM_VIEW_MODE"));
    mp.game.invoke(global.getNative("SET_FOLLOW_PED_CAM_VIEW_MODE"), 4);
    global.phoneCameraIsOpened = true;
    global.showHud(false);
});


mp.keys.bind(global.Keys.Key_ESCAPE, false, ()=>{
    if (!global.phoneCameraIsOpened) return;
    global.phoneCameraIsOpened = false;
    global.showHud(true);
    mp.game.invoke(global.getNative("SET_FOLLOW_PED_CAM_VIEW_MODE"), lasViewMode);
})