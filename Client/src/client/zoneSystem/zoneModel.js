const coeffZoneRange = 1.981; //коэффициент размеров маркера к радиусу
let zoneId = 1;
class ZoneModel {
    constructor(height, startPosition, startRange, endPosition, endRange, zoneStartTime, timeoutZone, zoneConstrictionTime, damageHP, damageTime) {
        this.id = zoneId++;
        this.height = height;
        this.timeoutZone = timeoutZone; //время от создания зоны до полного сужения
        this.zoneStartTime = zoneStartTime; //время начала зоны
        this.zoneConstrictionTime = zoneConstrictionTime; //время сужения зоны
        this.currentZone =
        {
            Center: startPosition,
            Range: startRange,
        };
        this.nextZone =
        {
            Center: endPosition,
            Range: endRange,
        };

        this.zoneDamage = damageHP;
        this.zoneDamageInterval = damageTime;

        this.inZone = true;

        this.damageInterval = null;
    }

    updateParams(endPosition, endRange, timeoutZone, zoneConstrictionTime) {
        this.currentZone = this.nextZone;
        this.zoneConstrictionTime = zoneConstrictionTime;
        this.timeoutZone = timeoutZone
        this.nextZone =
        {
            Center: endPosition,
            Range: endRange,
        }
    }

    drawZone() {
        let color = this.getColor();
        let zone = this.getZone();
        mp.game.graphics.drawMarker(
            1,
            zone.Center.x, zone.Center.y, zone.Center.z,
            0, 0, 0,
            0, 0, 0,
            zone.Range * coeffZoneRange, zone.Range * coeffZoneRange, this.height,
            color, 255 - color, 50, 156,
            false, false, 2,
            false, null, null, false
        );
        if (this.zoneDamage <= 0)
            return;
        let dist = mp.game.gameplay.getDistanceBetweenCoords(zone.Center.x, zone.Center.y, zone.Center.z, mp.players.local.position.x, mp.players.local.position.y, zone.Center.z, true)
        if (this.inZone != dist < zone.Range)
            this.OnPlayerChangeInCircle(dist < zone.Range);
    }
    getColor() {
        let color = 255;
        if (this.zoneStartTime > Date.now())
            return color;
        if (this.timeoutZone > 0) {
            color = Math.round(this.getTimeCoeff(this.zoneStartTime, this.timeoutZone) * 255);
        }
        return color;
    }

    getZone() {
        let length = this.getTimeCoeff(this.zoneStartTime + (this.timeoutZone - this.zoneConstrictionTime) * 1000, this.zoneConstrictionTime);
        let zone =
        {
            Center: new mp.Vector3(this.currentZone.Center.x + (this.nextZone.Center.x - this.currentZone.Center.x) * length, this.currentZone.Center.y + (this.nextZone.Center.y - this.currentZone.Center.y) * length, this.currentZone.Center.Z),
            Range: this.nextZone.Range + (this.currentZone.Range - this.nextZone.Range) * (1 - length)
        }
        return zone;
    }

    getTimeCoeff(startTime, timeLength) {
        if (timeLength == 0)
            return 0;
        let value = (Date.now() - startTime) / (timeLength * 1000);
        if (value > 1)
            return 1;
        if (value < 0)
            return 0;
        return value;
    }

    OnPlayerChangeInCircle(toggle) {
        this.inZone = toggle;
        if (this.damageInterval != null) {
            clearInterval(this.damageInterval);
            this.damageInterval = null;
        }
        if (!this.inZone) {
            this.damageInterval = setInterval(() => {
                this.DamagePlayer()
            }, 1500);

        }
    }

    DamagePlayer() {
        mp.game.graphics.setTimecycleModifier('damage');
        mp.players.local.applyDamageTo(this.zoneDamage, true);
        setTimeout(() => {
            mp.game.graphics.setTimecycleModifier('default');
        }, this.zoneDamageInterval);
    }

    destroy() {

        if (this.damageInterval != null) {
            clearInterval(this.damageInterval);
            this.damageInterval = null;
        }
    }
}

module.exports = ZoneModel;