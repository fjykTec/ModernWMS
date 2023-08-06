// Get different languages according to the key
export function getSelcectedLangForVuetify(lang: string) {
  switch (lang) {
    case 'zh':
      return 'zhHans'
    case 'en':
      return 'en'
    default:
      return 'en'
  }
}
