
const words = [
    "ниг","нег","пиратка","пидр","пидор","педик","педрил","гомик","гей","хохол","хач","талибан","игил","гидра","даун","аутист","пизд","конч","инцел","девствен","симп","nig","neg","piratka","pidr","pido","pedik","pedr","gomik","gey","hohol","hohl","hach","talib","igil","gidra","daun","autist","pizda","koncha","incel","devstven","simp","Пидар", "Пидарас", "Педарас", "пидараз", "pidaras", "pidaraz", "churka", "nacist", "nacik", "нацист", "hidjap", "холокост", "holocaust", "schutzstaffeln", "гитлер", "цыган", "cigan", "gipsy", "хидж", "хедж","hidj","hedj", "pussy"
]

const rplcSymbol = "**************************************************";

function build(){
    var result = [];

    words.forEach(word => {
        const reg = new RegExp(word, 'gi');
        const rplc = rplcSymbol.substring(0, word.length);
        result.push({reg , rplc});
    });
    return result;
}

module.exports = build();