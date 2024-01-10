// Acquisition unit
export function GetUnit(type: 'weight' | 'length' | 'volume', val: number) {
  switch (type) {
    case 'length':
      return ['mm', 'cm', 'dm', 'm'][val]
    case 'volume':
      return ['cm³', 'dm³', 'm³'][val]
    case 'weight':
      return ['mg', 'g', 'kg'][val]
  }
}