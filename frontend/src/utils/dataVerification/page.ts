/**
 * Duplicate inspection details
 * @param detail Array to be verified
 * @param checkKey Collection of items to be checked in the array object
 * @returns Repeat array
 */
export function checkDetailRepeatGetArry(detail: any[], checkKey: string[]): any[] {
  const repeatArray: any[] = []
  for (const item of detail) {
    let repeat = true
    for (const key of checkKey) {
      if (detail.filter(df => df[key] === item[key]).length === 1) {
        repeat = false
      }
    }
    if (repeat) {
      repeatArray.push(item)
    }
  }
  return repeatArray
}
/**
 * Duplicate inspection details
 * @param detail Array to be verified
 * @param checkKey Collection of items to be checked in the array object
 * @returns Duplicate or not
 */
export function checkDetailRepeatGetBool(detail: any[], checkKey: string[]): boolean {
  for (const item of detail) {
    let repeat = true
    for (const key of checkKey) {
      if (detail.filter(df => df[key] === item[key]).length === 1) {
        repeat = false
      }
    }
    if (repeat) {
      return true
    }
  }
  return false
}
