// Get different languages according to the key
export function getSelectedLang(lang: string) {
  switch (lang) {
    case 'zh':
      return 'zh_CN'
    case 'en':
      return 'en_US'
    case 'tw':
      return 'zh_TW'
    default:
      return 'en_US'
  }
}
