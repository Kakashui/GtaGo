class Line2D {
    constructor (point1, point2) {
        this.A = point2.y - point1.y;
        this.B = point1.x - point2.x;
        this.C = point1.y * (point2.x - point1.x) - point1.x * (point2.y - point1.y);
    }

    CheckPointOnLine(point) {
        return this.A * point.x + this.B * point.y + this.C;
    }
}

module.exports = Line2D;