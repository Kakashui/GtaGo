const speed = 0.015;

class ObjectMover {
    constructor(hash, position, dim) {
        this.enabled = false
        this.obj = mp.objects.new(hash, position, { dimension: dim })
    }

    enable() {
        this.enabled = true
        mp.events.add('render', () => {
            this.onTick(this.obj)
        });
    }

    disable() {
        this.enabled = false
    }

    addCallback(func) {
        this.callback = func
    }
    onTick(obj) {
        if (!this.enabled) return;
        if (mp.game.controls.isDisabledControlJustPressed(0, 241)) {
            obj.setRotation(0, 0, obj.getRotation(0).z - 4.2, 0, false);
        }
        if (mp.game.controls.isDisabledControlJustPressed(0, 242)) {
            obj.setRotation(0, 0, obj.getRotation(0).z + 4.2, 0, false);
        }
        if (mp.keys.isDown(global.Keys.Key_Z)) {
            obj.position = new mp.Vector3(obj.position.x, obj.position.y, obj.position.z + speed);
        }
        if (mp.keys.isDown(global.Keys.Key_X)) {
            obj.position = new mp.Vector3(obj.position.x, obj.position.y, obj.position.z - speed);
        }
        if (mp.keys.isDown(global.Keys.Key_UP)) {
            obj.position = new mp.Vector3(obj.position.x + speed, obj.position.y + speed, obj.position.z);
        }
        if (mp.keys.isDown(global.Keys.Key_RIGHT)) {
            obj.position = new mp.Vector3(obj.position.x + speed, obj.position.y - speed, obj.position.z);
        }
        if (mp.keys.isDown(global.Keys.Key_LEFT)) {
            obj.position = new mp.Vector3(obj.position.x - speed, obj.position.y + speed, obj.position.z);
        }
        if (mp.keys.isDown(global.Keys.Key_DOWN)) {
            obj.position = new mp.Vector3(obj.position.x - speed, obj.position.y - speed, obj.position.z);
        }
        if (mp.keys.isDown(global.Keys.Key_RETURN)) {
            this.disable()
            this.callback(obj.position, obj.getRotation(0))
            this.obj.destroy()
            global.gui.setOpened(false)
        }
    }
}
module.exports = ObjectMover;