module.exports = {
    size: {
        x: 35,
        y: 35
    },
    showIcons:[false, false, false, false],
    values:[
        {//LEFT X
            value: 160,
            min: 0,
            max: 320,
            step: 5,
            invert: false,
            enabled: true,
            callback: "wshop:move:z"
        },
        {//LEFT Y
            value: 0,
            min: -35,
            max: 35,
            step: 5,
            invert: false,
            enabled: true,
            callback: "wshop:move:x"
        },
        {//RIGHT X
            value: 2,
            min: 1,
            max: 3,
            step: .1,
            invert: false,
            enabled: false,
            callback: ""
        },
        {//RIGHT Y
            value: 0,
            min: -1,
            max: 1,
            step: .05,
            invert: true,
            enabled: false,
            callback: "camMovePointZ"
        },
        { //WHEELE
            value: 1,
            min: .6,
            max: 1.2,
            step: .2,
            invert: false,
            enabled: false,
            callback: "camSetDist"
        }
    ]
}
