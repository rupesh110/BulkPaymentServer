import { validateCsv } from "@/domain/csv/validateCsv";
import { logger } from "@/infrastructure/logging/logger";

export type UploadCsvUsecaseResult = 
    | { ok: true; fileName: string; size: number}
    | { ok: false; message: string}

export async function uploadCsvUsecase(file: File): Promise<UploadCsvUsecaseResult> {
    logger.info('Upload CSV started', {fileName: file.name, size: file.size})

    const validation = validateCsv(file)
    if(!validation.ok){
        logger.warn('Upload CSV validation failed', {
            fileName: file.name,
            size: file.size,
            code: validation.code,
        })
        return { ok: false, message: validation.message}
    }
    
    await new Promise((r) => setTimeout(r, 600))

    logger.info('Upload CSV completed', { fileName: file.name, size: file.size})

    return { ok: true, fileName: file.name, size: file.size}
}