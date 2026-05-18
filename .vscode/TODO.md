CORRIGIR LAUNCH JSON:

launch.json:
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Watch Launch",
            "type": "coreclr",
            "request": "launch",
            "program": "dotnet",
            "args": [
                "watch",
                "run",
                "--project",
                "${workspaceFolder}/user-api.cs.csproj"
            ],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
            "console": "integratedTerminal",
            "internalConsoleOptions": "neverOpen",
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            },
            "serverReadyAction": {
                "action": "openExternally",
                "pattern": "\\bNow listening on:\\s+(https?://\\S+)",
                "uriFormat": "%s/scalar"
            }
        },
        {
            "name": ".NET Core Attach",
            "type": "coreclr",
            "request": "attach"
        }
    ]
}
```
TASKS.json: 
```json
{
    "label": "watch",
    "type": "process",
    "command": "dotnet",
    "args": [
        "watch",
        "run",
        "--project",
        "${workspaceFolder}/user-api.cs.csproj"
    ],
    "isBackground": true,
    "problemMatcher": "$msCompile"
}
```

Mas sinceramente?
Depois da mudança no launch.json, você quase nem precisa mais do task watch.
Porque o próprio debugger já sobe com watch.

=====================

Outra melhoria importante
NÃO use .sln no watch
Você colocou:

"${workspaceFolder}/user-api.cs.sln"

Mas o watch run normalmente deve apontar para:

"${workspaceFolder}/user-api.cs.csproj"

Porque:

watch run roda um projeto executável
solution pode causar comportamento estranho
mais lento
múltiplos projetos podem confundir o host