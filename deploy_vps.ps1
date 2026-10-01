[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Install-PackageProvider -Name NuGet -Force -Scope CurrentUser
Install-Module -Name Posh-SSH -Force -Scope CurrentUser -AllowClobber -ErrorAction SilentlyContinue
Import-Module Posh-SSH
$secpasswd = ConvertTo-SecureString "Resident1992@" -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential ("root", $secpasswd)
$session = New-SSHSession -ComputerName "72.61.70.56" -Credential $creds -AcceptKey
if (-not $session) {
    Write-Error "Failed to connect via SSH"
    exit 1
}
Write-Host "Connected successfully. Cloning and deploying..."
$command = "cd OPA_BACKEND_CREDITS; git pull; docker compose up -d"
$result = Invoke-SSHCommand -SessionId $session.SessionId -Command $command -TimeOut 300
Write-Host "--- Output ---"
Write-Host $result.Output
Write-Host "--- Error ---"
Write-Host $result.Error
Remove-SSHSession -SessionId $session.SessionId
