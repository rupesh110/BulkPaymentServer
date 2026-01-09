import { UploadCsvForm } from '../components/UploadCsvForm'
import { useUploadCsv } from '../hooks/useUploadsv'

export function UploadCsvPage() {
  const { state, error, success, canUpload, upload, reset } = useUploadCsv()

  return (
    <div style={{ padding: 24, fontFamily: 'system-ui, sans-serif' }}>
      <h1>Upload CSV</h1>
      <p style={{ opacity: 0.8 }}>
        Enterprise-style slice: UI → Hook → Usecase → Domain → Infrastructure(logging)
      </p>

      <UploadCsvForm disabled={!canUpload} onUpload={upload} />

      <div style={{ marginTop: 16 }}>
        {state === 'uploading' && <p>Uploading…</p>}
        {state === 'error' && <p style={{ color: 'crimson' }}>{error?.message}</p>}
        {state === 'success' && (
          <p style={{ color: 'green' }}>
            Uploaded: <b>{success?.fileName}</b> ({success?.size} bytes)
          </p>
        )}
      </div>

      <div style={{ marginTop: 16 }}>
        <button type="button" onClick={reset}>
          Reset
        </button>
      </div>
    </div>
  )
}
