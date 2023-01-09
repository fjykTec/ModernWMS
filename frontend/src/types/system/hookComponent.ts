// Dialog modal options
export interface DialogOptions {
  confirmText?: string // confirm btn text
  cancleText?: string // cancle btn text
  handleConfirm?(): void // confirm trigger method
  content: string // dialog content text
  title?: string // dialog title text
  dialogWidth?: string | number // dialog width pixel or percentage
}

// System message options
export interface MessageOptions {
  type?: 'success' | 'info' | 'warning' | 'error'
  variant?: 'flat' | 'elevated' | 'tonal' | 'outlined' | 'text' | 'plain'
  content: string
  border?: boolean | 'top' | 'end' | 'bottom' | 'start'
  shutDelay?: number
  color?: string
  messageWidth?: string | number // message width pixel or percentage
}

// Component list
export interface Components {
  // eslint-disable-next-line no-unused-vars
  $message(prop: MessageOptions): void
  // eslint-disable-next-line no-unused-vars
  $dialog(prop: DialogOptions): void
}
