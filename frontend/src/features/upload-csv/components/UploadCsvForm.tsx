import { useState } from 'react'

type Props = {
  disabled?: boolean
  onUpload: (file: File) => void
}

export function UploadCsvForm({ disabled = false, onUpload }: Props) {
  const [selected, setSelected] = useState<File | null>(null)

  return (
    <div style={{ display: 'grid', gap: 12, maxWidth: 520 }}>
      <input
        type="file"
        accept=".csv,text/csv"
        disabled={disabled}
        onChange={(e) => {
          const file = e.target.files?.[0] ?? null
          setSelected(file)
        }}
      />

      <button
        type="button"
        disabled={disabled || !selected}
        onClick={() => {
          if (selected) onUpload(selected)
        }}
      >
        Upload
      </button>

      {selected && (
        <small>
          Selected: <b>{selected.name}</b> ({selected.size} bytes)
        </small>
      )}
    </div>
  )
}
