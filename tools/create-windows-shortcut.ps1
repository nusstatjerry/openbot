param(
  [string]$TargetExe = "src\Bin\Bot.exe",
  [string]$ShortcutName = "灵桥客服"
)

$root = Resolve-Path (Join-Path $PSScriptRoot "..")
$exePath = Join-Path $root $TargetExe
if (!(Test-Path $exePath)) {
  throw "未找到可执行文件: $exePath"
}

$desktop = [Environment]::GetFolderPath('Desktop')
$shortcutPath = Join-Path $desktop ($ShortcutName + ".lnk")

$wsh = New-Object -ComObject WScript.Shell
$shortcut = $wsh.CreateShortcut($shortcutPath)
$shortcut.TargetPath = $exePath
$shortcut.WorkingDirectory = Split-Path $exePath -Parent
$shortcut.IconLocation = "$exePath,0"
$shortcut.Description = "灵桥客服"
$shortcut.Save()

Write-Host "已创建桌面快捷方式: $shortcutPath"
