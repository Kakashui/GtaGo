const DEFAULT_MARKER_COLOR = [0, 0, 0, 255];
const interactionShapes = [];

let lastInteractionShapeId = -1;

function createInteractionShape(position, range, height, callback, withMarker = true, dimension = 0) {
    const id = lastInteractionShapeId--;

    const colshape = mp.colshapes.newCircle(position.x, position.y, range);
    colshape.isInteraction = true;

    colshape.callback = callback;
    colshape.helpKeys = [{ Key: 0x45, Text: "interact_1" }];

    if (withMarker) {
        mp.events.call('interact:loadMarkers', JSON.stringify({
            ID: id,
            Type: 27,
            Position: position,
            Scale: range,
            Color: DEFAULT_MARKER_COLOR,
            Dimension: dimension
        }));
    }

    colshape.interactionId = id;
    interactionShapes[id] = colshape;

    return id;
}

function destroyInteractionShape(interactionShapeId) {
    if (interactionShapes[interactionShapeId]) {
        const interactionShape = interactionShapes[interactionShapeId];
        mp.events.call('interact:destroyMarker', interactionShape.id);
    }
}

mp.events.add({
    "playerEnterColshape": (shape) => {
        if (shape.isInteraction) {

        }
    }
});

global.interaction = { };