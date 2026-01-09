if (!(Test-Path ".env")) {
    Write-Error ".env file not found"
    exit 1
}

Get-Content .env | ForEach-Object {
    if ($_ -match "^\s*([^#=]+)=(.+)$") {
        $key = $matches[1].Trim().Replace("__", ":")
        $value = $matches[2].Trim()
        dotnet user-secrets set $key $value | Out-Null
    }
}

Write-Host "User Secrets configured successfully"
