# Вспомогательный скрипт: получить SHA-256 пароля для отправки в API
param([string]$Password = "password")

$bytes  = [System.Text.Encoding]::UTF8.GetBytes($Password)
$sha    = [System.Security.Cryptography.SHA256]::Create()
$hash   = $sha.ComputeHash($bytes)
$hex    = ($hash | ForEach-Object { $_.ToString("x2") }) -join ""

Write-Output $hex
