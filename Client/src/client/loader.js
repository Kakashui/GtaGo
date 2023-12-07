class AsyncLoader{
    constructor() {
        this.iterations = 50;
        this.delayTime = 5;
    }

    async waitAsync(condition){
        if(typeof condition !== 'function') return false;
        for (let index = 0; index < this.iterations; index++) {
            //mp.serverLog(`waitAsync ${index}`);
            await this.delay(this.delayTime);
            if(condition()) return true;
        }
        return false;
    }

    delay(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}

global.loader = new AsyncLoader();
