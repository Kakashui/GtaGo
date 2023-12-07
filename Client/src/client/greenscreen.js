
let _scale;
let _targetToRender = -1
let _fixPlayer = false;
let _gender = true;

let greenscreenMenuOpened = false;
let objects = [];

mp.events.add('greenscreen:openedMenu', (toggle) => {
    objects.forEach(element => {
        element.destroy();
    });
    objects = [];
    if (toggle == true) {
        if (!mp.game.graphics.hasStreamedTextureDictLoaded('mpweaponsgang0'))
            mp.game.graphics.requestStreamedTextureDict('mpweaponsgang0', true);
        greenscreenMenuOpened = true;
        _targetToRender = -1
    }
    else {
        greenscreenMenuOpened = false;
    }
});

mp.events.add('render', () => {
    if (!greenscreenMenuOpened)
        return;
    RenderThings(_targetToRender);
});

mp.keys.bind(global.Keys.Key_NUMPAD5, false, function () {
    if (global.checkIsAnyActivity()) return;
    if (!greenscreenMenuOpened)
        return;
    Init();
});

function Init() {
    if (!greenscreenMenuOpened)
        return;
    mp.game.graphics.setFarShadowsSuppressed(true);
    var pos = mp.players.local.position;
    var rot = mp.players.local.getRotation(0);
    _scale = mp.game.graphics.requestScaleformMovie('cellphone_ifruit');
    offsetList.forEach(offset => {
        x = CreateModel('xm_prop_orbital_cannon_table', global.GetOffsetPosition(pos, rot.z, offset.pos), new mp.Vector3(rot.x + offset.rotX, rot.y, rot.z));
        id = CreateRenderTarget('orbital_table', 'xm_prop_orbital_cannon_table');
        if (id != -1)
            _targetToRender = id;
        objects.push(x);
    });
}

let offsetList = [

    { pos: new mp.Vector3(0, 0, 0), rotX: 0 },
    { pos: new mp.Vector3(2, 0, 0), rotX: 0 },
    { pos: new mp.Vector3(4, 0, 0), rotX: 0 },
    { pos: new mp.Vector3(6, 0, 0), rotX: 0 },
    { pos: new mp.Vector3(-2, 0, 0), rotX: 0 },
    { pos: new mp.Vector3(-4, 0, 0), rotX: 0 },
    { pos: new mp.Vector3(-6, 0, 0), rotX: 0 },
    { pos: new mp.Vector3(0, -2, 0), rotX: 0 },
    { pos: new mp.Vector3(2, -2, 0), rotX: 0 },
    { pos: new mp.Vector3(4, -2, 0), rotX: 0 },
    { pos: new mp.Vector3(6, -2, 0), rotX: 0 },
    { pos: new mp.Vector3(-2, -2, 0), rotX: 0 },
    { pos: new mp.Vector3(-4, -2, 0), rotX: 0 },
    { pos: new mp.Vector3(-6, -2, 0), rotX: 0 },
    { pos: new mp.Vector3(0, -4, 0), rotX: 0 },
    { pos: new mp.Vector3(2, -4, 0), rotX: 0 },
    { pos: new mp.Vector3(4, -4, 0), rotX: 0 },
    { pos: new mp.Vector3(6, -4, 0), rotX: 0 },
    { pos: new mp.Vector3(-2, -4, 0), rotX: 0 },
    { pos: new mp.Vector3(-4, -4, 0), rotX: 0 },
    { pos: new mp.Vector3(-6, -4, 0), rotX: 0 },
    { pos: new mp.Vector3(0, -6, 0), rotX: 0 },
    { pos: new mp.Vector3(2, -6, 0), rotX: 0 },
    { pos: new mp.Vector3(4, -6, 0), rotX: 0 },
    { pos: new mp.Vector3(6, -6, 0), rotX: 0 },
    { pos: new mp.Vector3(-2, -6, 0), rotX: 0 },
    { pos: new mp.Vector3(-4, -6, 0), rotX: 0 },
    { pos: new mp.Vector3(-6, -6, 0), rotX: 0 },
    { pos: new mp.Vector3(0, -8, 0), rotX: 0 },
    { pos: new mp.Vector3(2, -8, 0), rotX: 0 },
    { pos: new mp.Vector3(4, -8, 0), rotX: 0 },
    { pos: new mp.Vector3(6, -8, 0), rotX: 0 },
    { pos: new mp.Vector3(-2, -8, 0), rotX: 0 },
    { pos: new mp.Vector3(-4, -8, 0), rotX: 0 },
    { pos: new mp.Vector3(-6, -8, 0), rotX: 0 },
    { pos: new mp.Vector3(0, -10, 0), rotX: 0 },
    { pos: new mp.Vector3(2, -10, 0), rotX: 0 },
    { pos: new mp.Vector3(4, -10, 0), rotX: 0 },
    { pos: new mp.Vector3(6, -10, 0), rotX: 0 },
    { pos: new mp.Vector3(-2, -10, 0), rotX: 0 },
    { pos: new mp.Vector3(-4, -10, 0), rotX: 0 },
    { pos: new mp.Vector3(-6, -10, 0), rotX: 0 },
    { pos: new mp.Vector3(0, 0, 1), rotX: 90 },
    { pos: new mp.Vector3(2, 0, 1), rotX: 90 },
    { pos: new mp.Vector3(4, 0, 1), rotX: 90 },
    { pos: new mp.Vector3(6, 0, 1), rotX: 90 },
    { pos: new mp.Vector3(-2, 0, 1), rotX: 90 },
    { pos: new mp.Vector3(-4, 0, 1), rotX: 90 },
    { pos: new mp.Vector3(-6, 0, 1), rotX: 90 },
    { pos: new mp.Vector3(0, 0, 3), rotX: 90 },
    { pos: new mp.Vector3(2, 0, 3), rotX: 90 },
    { pos: new mp.Vector3(4, 0, 3), rotX: 90 },
    { pos: new mp.Vector3(6, 0, 3), rotX: 90 },
    { pos: new mp.Vector3(-2, 0, 3), rotX: 90 },
    { pos: new mp.Vector3(-4, 0, 3), rotX: 90 },
    { pos: new mp.Vector3(-6, 0, 3), rotX: 90 },
]

function CreateModel(model, pos, rot) {
    if (!mp.game.streaming.hasModelLoaded(mp.game.joaat(model)))
        mp.game.streaming.requestModel(mp.game.joaat(model));
    
    let obj = mp.objects.new(mp.game.joaat(model), pos, {
        rotation: rot,
        alpha: 255,
        dimension: mp.players.local.dimension
    });
    for (let index = 0; !obj.doesExist() && index < 250; index++) {
        mp.game.wait(0);
    }
    return obj;
}

function CreateRenderTarget(name, model) {
    if (!mp.game.ui.isNamedRendertargetRegistered(name))
        mp.game.ui.registerNamedRendertarget(name, false);
    if (!mp.game.ui.isNamedRendertargetLinked(mp.game.joaat(model)))
        mp.game.ui.linkNamedRendertarget(mp.game.joaat(model));
    if (mp.game.ui.isNamedRendertargetRegistered(name))
        return mp.game.ui.getNamedRendertargetRenderId(name);
    return -1;
}

function RenderThings(id) {
    mp.game.ui.setTextRenderId(id);
    mp.game.graphics.set2dLayer(100);
    //mp.game.graphics.drawRect(0, 0, 10, 10, 0, 255, 0, 255);
    mp.game.graphics.drawSprite('mpinventory', 'in_world_circle', 0.5, 0.5, 1, 1, 0, 0, 255, 0, 255);
    //Graphics.DrawScaleformMovie(_scale, 0.5F, 0.5F, 10, 10, 255, 100, 100, 255, 0);
    mp.game.ui.setTextRenderId(1);
}