const joinUrl = 'messenger.go/join?code=';
const urlFindRegex = /messenger.go\/join\?code=(?<code>[a-zA-Z0-9]{1,10})\b/g

export function getInviteUrl(inviteCode) {
    return joinUrl + inviteCode;
}

export function replaceUrlsOnRefs(str) {
    if (str === null)
        return str;
    
    return str.replace(urlFindRegex, (match, code) => {
        return `<a id="${code}" style="text-decoration: underline">${match}</a>`;
    });
}