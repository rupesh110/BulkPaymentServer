export const env ={
    appEnv: import.meta.env.VITE_ENV ?? 'local',
    enableDebugLogs: (import.meta.env.VITE_ENABLE_DEBUG_LOGS ?? 'true') === 'true',
} as const