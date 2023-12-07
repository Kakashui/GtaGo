export function addHandlersForInvitesClick(ref, eventBus) {
    const urlChilds = Array.from(ref.childNodes).filter(n => n.tagName === 'A');
    urlChilds.forEach(element => {
      const code = element.id;
      element.addEventListener('click', (event) => {
        eventBus.$emit('phone::msg::clickInviteUrl', code);
        event;
      });
    });
}