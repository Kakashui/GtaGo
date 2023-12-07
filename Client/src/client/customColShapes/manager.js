const QuadColShape = require('./models/quadColShape');
const SquareColShape = require('./models/squareColShape');

let currentShapes = [];

let sendingExcept = false;


mp.events.add("customShape:load", (shapesDTO) => {
    let shapes = JSON.parse(shapesDTO);
    shapes.forEach(shape => {
        switch (shape.Type) {
            case 0:
                let quadShape = new QuadColShape(shape.ID, shape.Point1, shape.Point2, shape.Point3, shape.Point4, shape.Dimension);
                currentShapes.push(quadShape);
                break;
            case 1:
                let squareShape = new SquareColShape(shape.ID, shape.Center, shape.Width, shape.Rotation, shape.Dimension);
                currentShapes.push(squareShape);
                break;
            default:
                let defaultShape = new QuadColShape(shape.ID, shape.Point1, shape.Point2, shape.Point3, shape.Point4, shape.Dimension);
                currentShapes.push(defaultShape);
                break;
        }
    });
    setInterval(CheckSHapes, 100);
});
mp.events.add("customShape:add", (shapeDTO) => {
    let shape = JSON.parse(shapeDTO);
    switch (shape.Type) {
        case 0:
            let quadShape = new QuadColShape(shape.ID, shape.Point1, shape.Point2, shape.Point3, shape.Point4, shape.Dimension);
            currentShapes.push(quadShape);
            break;
        case 1:
            let squareShape = new SquareColShape(shape.ID, shape.Center, shape.Width, shape.Rotation, shape.Dimension);
            currentShapes.push(squareShape);
            break;
        default:
            let defaultShape = new QuadColShape(shape.ID, shape.Point1, shape.Point2, shape.Point3, shape.Point4, shape.Dimension);
            currentShapes.push(defaultShape);
            break;
    }
});

function CheckSHapes() {
    currentShapes.forEach(shape => {
        shape.CheckPlayer();
    });
}

let viewShapes = false;

mp.events.add('loadCustomShapes', (state) => {
    viewShapes = state;
});

let col = { r: 255, g: 0, b: 0, a: 255 };

let height = [
    20,
    30,
    40,
    50,
    60,
    70,
    80,
    90,
    100,
    110,
    120,
    130,
    140,
    150,
    160,
    170,
    180,
];
mp.events.add('render', () => {
    try {
        if (!global.loggedin || !viewShapes) 
            return;
        currentShapes.forEach(s => {
            height.forEach(h => {
                mp.game.graphics.drawLine(s.Point1.x, s.Point1.y, h, s.Point2.x, s.Point2.y, h, col.r, col.g, col.b, col.a);
                mp.game.graphics.drawLine(s.Point2.x, s.Point2.y, h, s.Point3.x, s.Point3.y, h, col.r, col.g, col.b, col.a);
                mp.game.graphics.drawLine(s.Point3.x, s.Point3.y, h, s.Point4.x, s.Point4.y, h, col.r, col.g, col.b, col.a);
                mp.game.graphics.drawLine(s.Point4.x, s.Point4.y, h, s.Point1.x, s.Point1.y, h, col.r, col.g, col.b, col.a);
            });
            mp.game.graphics.drawLine(s.Point1.x, s.Point1.y, 20, s.Point1.x, s.Point1.y, 180, col.r, col.g, col.b, col.a);
            mp.game.graphics.drawLine(s.Point2.x, s.Point2.y, 20, s.Point2.x, s.Point2.y, 180, col.r, col.g, col.b, col.a);
            mp.game.graphics.drawLine(s.Point3.x, s.Point3.y, 20, s.Point3.x, s.Point3.y, 180, col.r, col.g, col.b, col.a);
            mp.game.graphics.drawLine(s.Point4.x, s.Point4.y, 20, s.Point4.x, s.Point4.y, 180, col.r, col.g, col.b, col.a);
        });
    } catch (e) {
        if (global.sendException && !sendingExcept) {
            sendingExcept = true;
            mp.serverLog(`customColSHapes.manager.render: ${e.name}\n${e.message}\n${e.stack}`);
        } 
        viewShapes = false;
    }
});