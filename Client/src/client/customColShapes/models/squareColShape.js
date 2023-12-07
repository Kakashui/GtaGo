const BaseShape = require('./baseShape');
const Line2D = require('./line2D');
const _twoRoot = Math.sqrt(2);

class SquareColShape extends BaseShape {
    constructor (id, center, width, rotation, dimension) {
        let r = width / 2;
        let xOffset = Math.cos(rotation / 180 * Math.PI) * r * _twoRoot;
        let yOffset = Math.sin(rotation / 180 * Math.PI) * r * _twoRoot;
        let point1 = new mp.Vector3(center.x + xOffset, center.y + yOffset, center.z);
        let point2 = new mp.Vector3(center.x - yOffset, center.y + xOffset, center.z);
        let point3 = new mp.Vector3(center.x - xOffset, center.y - yOffset, center.z);
        let point4 = new mp.Vector3(center.x + yOffset, center.y - xOffset, center.z);
        super(id, point1, point2, point3, point4, dimension)
        this.line1 = new Line2D(this.Point1, this.Point2);
        this.line2 = new Line2D(this.Point4, this.Point3);
        this.line3 = new Line2D(this.Point1, this.Point4);
        this.line4 = new Line2D(this.Point2, this.Point3);
    }
    
    IsPointWithin(point) {
        let check1 = this.line1.CheckPointOnLine(point);
        let check2 = this.line2.CheckPointOnLine(point);
        let check3 = this.line3.CheckPointOnLine(point);
        let check4 = this.line4.CheckPointOnLine(point);
        return (check1 >= 0 && check2 <= 0 || check1 <= 0 && check2 >= 0) && (check3 >= 0 && check4 <= 0 || check3 <= 0 && check4 >= 0);
    }
}

module.exports = SquareColShape;