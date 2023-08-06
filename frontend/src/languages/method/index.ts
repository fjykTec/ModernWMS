// Get different languages according to the key
export function getSelcectedLang(lang: string) {
  switch (lang) {
    case 'zh':
      return 'zh_CN'
    case 'en':
      return 'en_US'
    default:
      return 'en_US'
  }
}
