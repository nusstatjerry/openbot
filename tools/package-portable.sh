#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
OUT_FILE="${1:-openbot-portable.zip}"

cd "$ROOT_DIR"

if [ ! -f src/Bin/Bot.exe ]; then
  echo "error: src/Bin/Bot.exe 不存在，请先在 Windows 环境构建项目。" >&2
  exit 1
fi

zip -r "$OUT_FILE" src/Bin src/data README.md

echo "已生成: $OUT_FILE"
