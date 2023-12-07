const BaseShape = require('./baseShape');
const Line2D = require('./line2D');


class QuadColShape extends BaseShape {
    constructor (id, point1, point2, point3, point4, dimension) {
        super(id, new mp.Vector3(point1.x, point1.y, point1.z), new mp.Vector3(point2.x, point2.y, point2.z), new mp.Vector3(point3.x, point3.y, point3.z), new mp.Vector3(point4.x, point4.y, point4.z), dimension)
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

module.exports = QuadColShape;