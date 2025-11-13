#!/usr/bin/env bash
set -euo pipefail

export ASPNETCORE_URLS="${ASPNETCORE_URLS:-http://+:${PORT:-8080}}"

exec dotnet backSendify.dll
