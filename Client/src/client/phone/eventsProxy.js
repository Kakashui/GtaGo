function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

const createProxyCallback = (eventName, callback) => {
    if (eventName === 'render' || eventName === 'entityStreamIn' || eventName === 'entityStreamIn' || eventName === 'click')
        return callback;

    return new Proxy(callback, {
        apply: (target, thisArg, args) => {
            mp.game.mobile.moveFinger(getRandomInt(1, 5));
            target.apply(thisArg, args);
        }
    });
}


mp.events.addPhone = (event, callback) => {
    if (typeof(event) === 'object') {
        for (eventName in event) {
            mp.events.add(eventName, createProxyCallback(eventName, event[eventName]));
        }

        return;
    }

    const newCallback = createProxyCallback(event, callback)
    mp.events.add(event, newCallback);
}