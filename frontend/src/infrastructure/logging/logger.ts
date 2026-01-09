import {env} from '@/config/env'

export type LogLevel =  'debug' | 'info' | 'warn' | 'error'

export interface Logger {
    debug(message: string, meta?: Record<string, unknown>): void
    info(message: string, meta?: Record<string, unknown>): void
    warn(message: string, meta?: Record<string, unknown>): void
    error(message: string, meta?: Record<string, unknown>): void
}

function write(level: LogLevel, message: string, meta?:Record<string, unknown>){

    const payload = meta ? { ...meta }: undefined

    if (level === 'error') console.error(`[${level.toUpperCase()}] ${message}`, payload)
    else if (level === 'warn') console.warn(`[${level.toUpperCase()}] ${message}`, payload)
    else console.log(`[${level.toUpperCase()}] ${message}`, payload)
}

export const logger: Logger = {
  debug(message, meta) {
    if (!env.enableDebugLogs) return
    write('debug', message, meta)
  },
  info(message, meta) {
    write('info', message, meta)
  },
  warn(message, meta) {
    write('warn', message, meta)
  },
  error(message, meta) {
    write('error', message, meta)
  },
}