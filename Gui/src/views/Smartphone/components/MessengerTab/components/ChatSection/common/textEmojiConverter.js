const emojiToTag = {
    '😃': ':em1:',
    '😁': ':em2:',
    '😆': ':em3:',
    '😅': ':em4:',
    '🤣': ':em5:',
    '😂': ':em6:',
    '🙂': ':em7:',
    '🙃': ':em9:',
    '😉': ':em10:',
    '😊': ':em11:',
    '😇': ':em12:',
    '😍': ':em13:',
    '🤩': ':em14:',
    '😘': ':em15:',
    '🤗': ':em16:',
    '🤑': ':em17:',
    '🤪': ':em18:',
    '😜': ':em19:',
    '🤐': ':em20:',
    '🤔': ':em21:',
    '🤫': ':em22:',
    '😏': ':em23:',
    '🤢': ':em24:',
    '🤮': ':em25:',
    '🥶': ':em26:',
    '💩': ':em27:',
    '💋': ':em28:',
    '❤️': ':em29:',
};

export function getTextWithTag(text) {
    if (text == null)
        return null;
    
    for (const emoji in emojiToTag) {
        const regexp = new RegExp(emoji, 'g');
        text = text.replace(regexp, emojiToTag[emoji]);
    }

    return text;
}

export function getTextWithEmoji(text) {
    if (text == null)
        return null;
    
    for (const emoji in emojiToTag) {
        const regexp = new RegExp(emojiToTag[emoji], 'g');
        text = text.replace(regexp, emoji);
    }

    return text;
}