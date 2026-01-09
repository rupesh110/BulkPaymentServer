import { useCallback, useMemo, useState } from 'react'
import type { UploadCsvFailure, UploadCsvState, UploadCsvSuccess } from '../types/uploadCsv.types'
import { uploadCsvUsecase } from '../usecases/uploadCsv.usecase'

type UploadCsvViewModel = {
  state: UploadCsvState
  error: UploadCsvFailure | null
  success: UploadCsvSuccess | null
  canUpload: boolean
  upload: (file: File) => Promise<void>
  reset: () => void
}

export function useUploadCsv(): UploadCsvViewModel {
  const [state, setState] = useState<UploadCsvState>('idle')
  const [error, setError] = useState<UploadCsvFailure | null>(null)
  const [success, setSuccess] = useState<UploadCsvSuccess | null>(null)

  const reset = useCallback(() => {
    setState('idle')
    setError(null)
    setSuccess(null)
  }, [])

  const upload = useCallback(async (file: File) => {
    setState('uploading')
    setError(null)
    setSuccess(null)

    const result = await uploadCsvUsecase(file)

    if (!result.ok) {
      setState('error')
      setError({ message: result.message })
      return
    }

    setState('success')
    setSuccess({ fileName: result.fileName, size: result.size })
  }, [])

  const canUpload = useMemo(() => state !== 'uploading', [state])

  return { state, error, success, canUpload, upload, reset }
}
