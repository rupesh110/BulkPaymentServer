export type UploadCsvState = 'idle' | 'validating' | 'ready' | 'uploading' | 'success' | 'error'

export type UploadCsvFailure = {
  message: string
}

export type UploadCsvSuccess = {
  fileName: string
  size: number
}
