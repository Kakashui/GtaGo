const fs = require('fs');

let readedText = `${fs.readFileSync('clientside.js')}`;
//readedText = readedText.replace(/`/g, '\\`');
let key = "diopqwjodhqwiudiuhegwinxoweodhewpjpx";

function cipher(salt){
    const textToChars = text => text.split('').map(c => c.charCodeAt(0));
    const byteHex = n => ("0" + Number(n).toString(16)).substr(-2);
    const applySaltToChar = code => textToChars(salt).reduce((a,b) => a ^ b, code);

    return text => text.split('')
        .map(textToChars)
        .map(applySaltToChar)
        .map(byteHex)
        .join('');
}
const decipher = salt => {
    const textToChars = text => text.split('').map(c => c.charCodeAt(0));
    const applySaltToChar = code => textToChars(salt).reduce((a,b) => a ^ b, code);
    return encoded => encoded.match(/.{1,2}/g)
        .map(hex => parseInt(hex, 16))
        .map(applySaltToChar)
        .map(charCode => String.fromCharCode(charCode))
        .join('');
}
let mp = {};
mp.clientside = cipher(key)(readedText);
fs.writeFileSync("../client_packages/clientside.js", `mp.clientside = '${mp.clientside}';`);

const dec1 = text => text.split('').map(c => c.charCodeAt(0));const dec2 = code => dec1('diopqwjodhqwiudiuhegwinxoweodhewpjpx').reduce((a, b) => a ^ b, code);let dec = mp.clientside.match(/.{1,2}/g).map(hex => parseInt(hex, 16)).map(dec2).map(charCode => String.fromCharCode(charCode)).join('');

fs.writeFileSync("decrypted.js", `${dec}`);