class PlayersBlipTracker {
    constructor(playersRemoteIds) {
        this.blips = []
        this.remoteIds = playersRemoteIds
    }
    
    startTracking() {
        mp.players.forEach(p => {
            if (this.remoteIds.indexOf(p.remoteId) !== -1) {
                let blip = mp.blips.new(0, p.position, {dimension: mp.players.local.dimension})                
                this.blips.push(blip)
            }
        })
        this.timer = setInterval(() => {this.blipUpdateHandler()}, 100)
    }
   
    stop() {
        clearInterval(this.timer)
        setTimeout(() => {
            this.blips.forEach(b =>{
                if(b && b.doesExist()) b.destroy()
            })
            delete this
        }, 110)
    }
    
    blipUpdateHandler() {
        for (let i = 0; i < this.blips.length; i++) {
            if (mp.players.atRemoteId(this.remoteIds[i]))
                this.blips[i].setCoords(mp.players.atRemoteId(this.remoteIds[i]).getCoords(true))
        }
    }
}
module.exports = PlayersBlipTracker