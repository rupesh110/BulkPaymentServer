export type CsvValidationErrorCode =
  | 'NOT_A_CSV'
  | 'EMPTY_FILE'
  | 'TOO_LARGE'

export type CsvValidationResult =
  | { ok: true }
  | { ok: false; code: CsvValidationErrorCode; message: string }

const MAX_BYTES = 5 * 1024 * 1024 

export function validateCsv(file: File): CsvValidationResult {
  const name = file.name.toLowerCase()

  if (!name.endsWith('.csv')) {
    return { ok: false, code: 'NOT_A_CSV', message: 'Only .csv files are allowed.' }
  }

  if (file.size === 0) {
    return { ok: false, code: 'EMPTY_FILE', message: 'The CSV file is empty.' }
  }

  if (file.size > MAX_BYTES) {
    return { ok: false, code: 'TOO_LARGE', message: 'CSV file is too large (max 5MB).' }
  }

  return { ok: true }
}
