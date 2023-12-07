const uintMax = 4294967295;

class BaseShape {
    constructor (id, point1, point2, point3, point4, dimension) {
        this.ID = id;
        this.Point1 = point1;
        this.Point2 = point2;
        this.Point3 = point3;
        this.Point4 = point4;
        this.Dimension = dimension;
        this.currState = false;
    }
    IsPointWithin(point) {
    }

    CheckPlayerInShape() {
        if (mp.players.local.dimension != this.Dimension && this.Dimension != uintMax)
            return false;
        return this.IsPointWithin(mp.players.local.position);
    }
    CheckPlayer() {
        let currentCheck = this.CheckPlayerInShape();
        if (this.currState != currentCheck)
        {
            this.currState = currentCheck;
            if (currentCheck)
                mp.events.callRemote('customShape:enterShape', this.ID);
            else
                mp.events.callRemote('customShape:exitShape', this.ID);
        }
    }
}

module.exports = BaseShape;